using Microsoft.Extensions.Configuration;
using NPCConsoleTesting.DB_Connection;
using System.IO;
using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NPCConsoleTesting.Combat;
using System.Collections.Generic;
using System;

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
                    List<string> combatLog = FullCombat.DoAFullCombat(combatants);
                    FullCombat.DisplayPostCombatInformation(combatLog);
                }
                else if (response == 2)
                {
                    int numberBattling = combatantBuilder.DetermineNumberBattling(true);
                    List <Combatant> combatants = combatantBuilder.BuildListOfCombatants(connectionStringSvc.GetConnectionString(), numberBattling);

                    int numberOfTimesToRun = MultipleCombats.GetNumberOfTimesToRun();
                    MultipleCombats.DoMultipleCombats(combatants, numberOfTimesToRun);
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
