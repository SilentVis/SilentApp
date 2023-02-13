using System.Reflection;
using Microsoft.Extensions.Configuration;
using SimpleInjector;

namespace SilentApp.Infrastructure
{
    public interface IAssemblyBootstrapper
    {
        void RegisterDependencies(Container container, IEnumerable<Assembly> appAssemblies, IConfiguration configuration);
    }
}