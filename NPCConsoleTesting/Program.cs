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
        }
    }
}
