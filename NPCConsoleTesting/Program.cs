using Microsoft.Extensions.Configuration;
using NPCConsoleTesting.DB_Connection;
using System.IO;
using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NPCConsoleTesting.Combat;
using System.Collections.Generic;
using System;
using NPCConsoleTesting.Characters;

namespace NPCConsoleTesting
{
    class Program
    {
        static void Main()
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            //Log.Logger.Information("App Starting");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IConnectionStringService, ConnectionStringService>();
                })
                .UseSerilog()
                .Build();

            var connectionStringSvc = ActivatorUtilities.CreateInstance<ConnectionStringService>(host.Services);
            CombatantBuilder combatantBuilder = new();

            Console.WriteLine("Welcome to old-school combat simulator.");
            Console.WriteLine();

            bool userIsDoneWithProgram = false;
            while (!userIsDoneWithProgram)
            {
                //TODO: refactor these two
                Console.WriteLine("1 = Simulate a free-for-all battle, 2 = Simulate a team battle");
                int teamResponse = 0;
                while (teamResponse != 1 && teamResponse != 2)
                {
                    teamResponse = CombatantBuilder.GetPositiveIntFromUser();

                    if (teamResponse != 1 && teamResponse != 2)
                    {
                        Console.WriteLine("1 or 2, those are your options.");
                    }
                }

                bool isTeamBattle = teamResponse == 2;

                Console.WriteLine("1 = Simulate a single combat instance, 2 = Run a simulation multiple times");
                int response = 0;
                while (response != 1 && response != 2)
                {
                    response = CombatantBuilder.GetPositiveIntFromUser();

                    if (response != 1 && response != 2)
                    {
                        Console.WriteLine("1 or 2, those are your options.");
                    }
                }

                if (response == 1)
                {
                    int numberBattling = combatantBuilder.DetermineNumberBattling(false);
                    List<Combatant> combatants = combatantBuilder.BuildListOfCombatants(connectionStringSvc.GetConnectionString(), numberBattling);

                    FullCombat.DisplayPreCombatInformation(combatants);
                    if (!isTeamBattle)
                    {
                        MultipleCombats.PredictWinner(combatants);
                    }
                    FullCombat.DisplayCountdown();
                    List<string> combatLog = FullCombat.DoAFullCombat(combatants, isTeamBattle);
                    FullCombat.DisplayPostCombatInformation(combatLog);
                }
                else if (response == 2)
                {
                    int numberBattling = combatantBuilder.DetermineNumberBattling(true);
                    List <Combatant> combatants = combatantBuilder.BuildListOfCombatants(connectionStringSvc.GetConnectionString(), numberBattling);

                    int numberOfTimesToRun = MultipleCombats.GetNumberOfTimesToRun();
                    FullCombat.DisplayPreCombatInformation(combatants);
                    FullCombat.DisplayCountdown();
                    List<Winner> winners = MultipleCombats.DoMultipleCombats(combatants, numberOfTimesToRun, isTeamBattle);
                    MultipleCombats.DisplayWinRates(winners);
                }

                Console.WriteLine();
                Console.WriteLine($"Go again? Y/N");
                if (Console.ReadLine().ToLower() != "y")
                {
                    userIsDoneWithProgram = true;
                }
            }
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("")}")
                .AddEnvironmentVariables();
        }
    }
}
