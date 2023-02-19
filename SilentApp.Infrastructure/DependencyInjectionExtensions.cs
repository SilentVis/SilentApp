using System.Reflection;
using Microsoft.Extensions.Configuration;
using SilentApp.Infrastructure.Configuration;
using SimpleInjector;

namespace SilentApp.Infrastructure
{
    public static class DependencyInjectionExtensions
    {
        private const string NamespacePrefix = "SilentApp";

        public static void RegisterAssemblyBootstrappers(
            this Container container,
            IConfiguration configuration)
        {

            var appConfig = ConfigurationParser.Parse(configuration);

            var bootstrapperInterface = typeof(IAssemblyBootstrapper);

            var appAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName != null && assembly.FullName.StartsWith(NamespacePrefix))
                .ToList();

            var bootstrappers = appAssemblies.SelectMany(a => a.GetTypes())
                .Where(type => type is { IsClass: true, IsAbstract: false } && bootstrapperInterface.IsAssignableFrom(type))
                .Select(type => Activator.CreateInstance(type) as IAssemblyBootstrapper)
                .ToList();

            foreach (var bootstrapper in bootstrappers)
            {
                bootstrapper?.RegisterDependencies(container, appAssemblies, appConfig); 
            }

            container.RegisterInstance(appConfig);
        }

        public static void LoadAppAssemblies()
        {
            LoadReferencedAppAssemblies(Assembly.GetCallingAssembly());
        }

        private static void LoadReferencedAppAssemblies(Assembly currentAssembly)
        {
            var referencesAssemblies =
                currentAssembly
                    .GetReferencedAssemblies()
                    .Where(assemblyName => assemblyName.Name != null && assemblyName.Name.StartsWith(NamespacePrefix))
                    .Select(Assembly.Load)
                    .ToList();

            foreach (var assembly in referencesAssemblies)
            {
                LoadReferencedAppAssemblies(assembly);
            }
        }
    }
}
