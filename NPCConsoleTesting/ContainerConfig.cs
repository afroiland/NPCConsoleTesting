using Autofac;
using Microsoft.Extensions.Configuration;
using NPCConsoleTesting.Combat;
using NPCConsoleTesting.Spells;

namespace NPCConsoleTesting
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Application>().As<IApplication>();
            builder.RegisterType<ConfigurationBuilder>().As<IConfigurationBuilder>();
            builder.RegisterType<CombatantBuilder>().As<ICombatantBuilder>();
            builder.RegisterType<FullCombat>().As<IFullCombat>();
            builder.RegisterType<MultipleCombats>().As<IMultipleCombats>();
            builder.RegisterType<CombatantRetriever>().As<ICombatantRetriever>();
            builder.RegisterType<CombatRound>().As<ICombatRound>();
            builder.RegisterType<CombatMethods>().As<ICombatMethods>();
            builder.RegisterType<SpellMethods>().As<ISpellMethods>();

            return builder.Build();
        }
    }
}
