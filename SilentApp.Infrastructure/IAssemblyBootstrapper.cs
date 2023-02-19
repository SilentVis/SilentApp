using System.Reflection;
using SilentApp.Infrastructure.Configuration;
using SimpleInjector;

namespace SilentApp.Infrastructure
{
    public interface IAssemblyBootstrapper
    {
        void RegisterDependencies(Container container, IEnumerable<Assembly> appAssemblies, SilentAppConfiguration configuration);
    }
}