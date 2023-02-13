using System.Reflection;
using Microsoft.Extensions.Configuration;
using SilentApp.Infrastructure;
using SilentApp.Services.Contracts;
using SilentApp.Services.DataProviders;
using SilentApp.Services.DataProviders.Contracts;
using SilentApp.Services.Integrations;
using SimpleInjector;

namespace SilentApp.Services
{
    public class ServicesAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, IEnumerable<Assembly> appAssemblies, IConfiguration configuration)
        {
            container.RegisterInstance(TelegramBotClientFactory.Create(configuration));

            container.Register<IAzureStorageTableDataProvider, AzureStorageTableDataProvider>(Lifestyle.Singleton);
            container.Register<IAlertsApiDataProvider, AlertsApiDataProvider>(Lifestyle.Singleton);
            container.Register<IAlertsQueueDataProvider, AlertsQueueProvider>(Lifestyle.Singleton);

            RegisterHandlers(container, appAssemblies, typeof(ICommandHandler<>));
            RegisterHandlers(container, appAssemblies, typeof(IQueryRunner<,>));
        }

        private void RegisterHandlers(Container container, IEnumerable<Assembly> appAssemblies, Type genericType)
        {
            var typesToRegister = container.GetTypesToRegister(
                genericType, 
                appAssemblies,
                new TypesToRegisterOptions()
            {
                IncludeDecorators = false,
                IncludeComposites = false,
                IncludeGenericTypeDefinitions = true
            });

            container.Register(genericType, typesToRegister);
        }
    }
}
