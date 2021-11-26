using Autofac;
using Microsoft.Extensions.Configuration;
using NPCConsoleTesting.Combat;

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

            return builder.Build();
        }
    }
}
