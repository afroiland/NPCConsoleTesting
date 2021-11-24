using Autofac;
using NPCConsoleTesting.Combat;

namespace NPCConsoleTesting
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Application>().As<IApplication>();
            builder.RegisterType<CombatantBuilder>().As<ICombatantBuilder>();
            builder.RegisterType<FullCombat>().As<IFullCombat>();
            builder.RegisterType<MultipleCombats>().As<IMultipleCombats>();

            return builder.Build();
        }
    }
}
