using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NPCConsoleTesting.Characters;
using NPCConsoleTesting.Combat;
using NPCConsoleTesting.DB_Connection;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace NPCConsoleTesting
{
    public class Application : IApplication
    {
        ICombatantBuilder _combatantBuilder;
        IFullCombat _fullCombat;
        IMultipleCombats _multipleCombats;

        public Application(ICombatantBuilder combatantBuilder, IFullCombat fullCombat, IMultipleCombats multipleCombats)
        {
            _combatantBuilder = combatantBuilder;
            _fullCombat = fullCombat;
            _multipleCombats = multipleCombats;
        }

        public void Run()
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



            //-------------------------------------- App starts here --------------------------------------

            Console.WriteLine("Welcome to old-school combat simulator.");
            Console.WriteLine();

            bool userIsDoneWithProgram = false;
            while (!userIsDoneWithProgram)
            {
                bool isTeamBattle = _fullCombat.DetermineIfTeamBattle();
                bool runningASingleBattle = _fullCombat.DetermineIfSingleBattle();

                if (runningASingleBattle)
                {
                    int numberBattling = _combatantBuilder.DetermineNumberBattling(false);
                    List<Combatant> combatants = _combatantBuilder.BuildListOfCombatants(connectionStringSvc.GetConnectionString(), numberBattling);

                    _fullCombat.DisplayPreCombatInformation(combatants, isTeamBattle);
                    _multipleCombats.PredictWinner(combatants, isTeamBattle);
                    _fullCombat.DisplayCountdown();
                    List<string> combatLog = _fullCombat.DoAFullCombat(combatants, isTeamBattle);
                    _fullCombat.DisplayPostCombatInformation(combatLog);
                }
                else //running multiple battles
                {
                    int numberBattling = _combatantBuilder.DetermineNumberBattling(true);
                    List<Combatant> combatants = _combatantBuilder.BuildListOfCombatants(connectionStringSvc.GetConnectionString(), numberBattling);

                    int numberOfTimesToRun = _multipleCombats.GetNumberOfTimesToRun();
                    _fullCombat.DisplayPreCombatInformation(combatants, isTeamBattle);
                    _fullCombat.DisplayCountdown();
                    List<Winner> winners = _multipleCombats.DoMultipleCombats(combatants, numberOfTimesToRun, isTeamBattle);
                    _multipleCombats.DisplayWinRates(winners);
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
