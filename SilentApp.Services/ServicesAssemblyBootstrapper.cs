using System.Reflection;
using SilentApp.Infrastructure;
using SilentApp.Infrastructure.Configuration;
using SilentApp.Services.Contracts;
using SilentApp.Services.DataProviders;
using SilentApp.Services.DataProviders.Contracts;
using SilentApp.Services.Integrations;
using SimpleInjector;

namespace SilentApp.Services
{
    public class ServicesAssemblyBootstrapper : IAssemblyBootstrapper
    {
        public void RegisterDependencies(Container container, IEnumerable<Assembly> appAssemblies, SilentAppConfiguration configuration)
        {
            container.RegisterInstance(TelegramBotClientFactory.Create(configuration));

            container.Register<IAlertsApiDataProvider, AlertsApiDataProvider>(Lifestyle.Scoped);

            container.Register<IStorageNameIndex, StorageNameIndex>(Lifestyle.Singleton);
            container.Register<IAzureStorageClientFactory, AzureStorageClientFactory>(Lifestyle.Scoped);
            container.Register<IAzureTableDataProvider, AzureTableDataProvider>(Lifestyle.Scoped);
            container.Register<IAzureQueueDataProvider, AzureQueueDataProvider>(Lifestyle.Scoped);

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
