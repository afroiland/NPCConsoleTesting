using Autofac;

namespace NPCConsoleTesting
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Application>().As<IApplication>();
            builder.RegisterType<CombatantBuilder>().As<ICombatantBuilder>();

            return builder.Build();
        }
    }
}
