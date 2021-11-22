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
using Autofac;

namespace NPCConsoleTesting
{
    class Program
    {
        static void Main()
        {
            var container = ContainerConfig.Configure();

            using var scope = container.BeginLifetimeScope();
            var app = scope.Resolve<IApplication>();
            app.Run();

            //var builder = new ConfigurationBuilder();
            //BuildConfig(builder);

            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(builder.Build())
            //    .Enrich.FromLogContext()
            //    .WriteTo.Console()
            //    .CreateLogger();

            ////Log.Logger.Information("App Starting");

            //var host = Host.CreateDefaultBuilder()
            //    .ConfigureServices((context, services) =>
            //    {
            //        services.AddTransient<IConnectionStringService, ConnectionStringService>();
            //    })
            //    .UseSerilog()
            //    .Build();

            //var connectionStringSvc = ActivatorUtilities.CreateInstance<ConnectionStringService>(host.Services);
            //CombatantBuilder combatantBuilder = new();

            //Console.WriteLine("Welcome to old-school combat simulator.");
            //Console.WriteLine();

            //bool userIsDoneWithProgram = false;
            //while (!userIsDoneWithProgram)
            //{
            //    bool isTeamBattle = FullCombat.DetermineIfTeamBattle();
            //    bool runningASingleBattle = FullCombat.DetermineIfSingleBattle();

            //    if (runningASingleBattle)
            //    {
            //        int numberBattling = combatantBuilder.DetermineNumberBattling(false);
            //        List<Combatant> combatants = combatantBuilder.BuildListOfCombatants(connectionStringSvc.GetConnectionString(), numberBattling);

            //        FullCombat.DisplayPreCombatInformation(combatants, isTeamBattle);
            //        MultipleCombats.PredictWinner(combatants, isTeamBattle);
            //        FullCombat.DisplayCountdown();
            //        List<string> combatLog = FullCombat.DoAFullCombat(combatants, isTeamBattle);
            //        FullCombat.DisplayPostCombatInformation(combatLog);
            //    }
            //    else //running multiple battles
            //    {
            //        int numberBattling = combatantBuilder.DetermineNumberBattling(true);
            //        List <Combatant> combatants = combatantBuilder.BuildListOfCombatants(connectionStringSvc.GetConnectionString(), numberBattling);

            //        int numberOfTimesToRun = MultipleCombats.GetNumberOfTimesToRun();
            //        FullCombat.DisplayPreCombatInformation(combatants, isTeamBattle);
            //        FullCombat.DisplayCountdown();
            //        List<Winner> winners = MultipleCombats.DoMultipleCombats(combatants, numberOfTimesToRun, isTeamBattle);
            //        MultipleCombats.DisplayWinRates(winners);
            //    }

            //    Console.WriteLine();
            //    Console.WriteLine($"Go again? Y/N");
            //    if (Console.ReadLine().ToLower() != "y")
            //    {
            //        userIsDoneWithProgram = true;
            //    }
            //}
        }

        //static void BuildConfig(IConfigurationBuilder builder)
        //{
        //    builder.SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("")}")
        //        .AddEnvironmentVariables();
        //}
    }
}
