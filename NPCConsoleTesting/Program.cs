using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using NPCConsoleTesting.DB_Connection;
using System.IO;
using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

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

            Log.Logger.Information("App Starting");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IConnectionStringService, ConnectionStringService>();
                })
                .UseSerilog()
                .Build();

            Console.WriteLine($"How many are battling?");
            int numberBattling = int.Parse(Console.ReadLine());
            Console.WriteLine($"1 = Random, 2 = Custom, 3 = Get from db");
            int charOrigin = int.Parse(Console.ReadLine());

            CombatantBuilder cBuilder = new();
            List<ICombatant> combatants = new();
            string connectionString = "";

            if (charOrigin == 3)
            {
                var connectionStringSvc = ActivatorUtilities.CreateInstance<ConnectionStringService>(host.Services);
                connectionString = connectionStringSvc.GetConnectionString();
            }

            while (combatants.Count < numberBattling)
            {
                if (charOrigin == 2)
                {
                    combatants.Add(CombatantBuilder.BuildCombatantViaConsole());
                }
                else if (charOrigin == 3)
                {
                    string name = CombatantRetriever.GetNameFromUserInput();
                    try
                    {
                        combatants.Add(CombatantRetriever.GetCombatantByName(connectionString, name));
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine($"Exception: {ex}");
                        //Console.WriteLine();
                        Console.WriteLine("That didn't work. Try again.");
                    }
                }
                else
                {
                    combatants.Add(cBuilder.BuildCombatantRandomly());
                }
            }

            //combatants fight until only one* remains.  (*in rare cases, zero)
            List<string> wholeFightLog = new() {" ", "Here's what happened:"};
            bool downToOne = false;
            int roundNumber = 0;

            while (!downToOne)
            {
                List<string> logResults = CombatRound.DoACombatRound(combatants);

                roundNumber++;
                wholeFightLog.Add($"------Round {roundNumber}------");

                //TODO: ensure there is not a shorter way to do this. No luck briefly with Join, Concat
                //add roundLog to wholeFightLog
                foreach (string logEntry in logResults)
                {
                    wholeFightLog.Add(logEntry);
                }

                //TODO: clean this up, likely using LINQ
                //check if we're down to one
                int numberOfSurvivors = 0;
                foreach (ICombatant ch in combatants)
                {
                    if (ch.HP > 0)
                    {
                        numberOfSurvivors++;
                    }
                }
                if (numberOfSurvivors == 1)
                {
                    //the fight has ended
                    downToOne = true;

                    List<string> winner = combatants.Where(x => x.HP > 0).Select(x => x.Name).ToList();
                    wholeFightLog.Add($"{winner[0]} won.");

                    wholeFightLog.ForEach(i => Console.WriteLine(i));
                    Console.ReadLine();
                }

                //lol
                if (numberOfSurvivors < 1)
                {
                    Console.WriteLine("lol");
                    break;
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
