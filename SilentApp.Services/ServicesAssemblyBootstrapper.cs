using System.Reflection;
using SilentApp.Infrastructure;
using SilentApp.Infrastructure.Configuration;
using SilentApp.Services.Contracts;
using SilentApp.Services.DataProviders;
using SilentApp.Services.DataProviders.Contracts;
using SilentApp.Services.Decorators;
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
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(CommandHandlerLoggingDecorator<>));
            RegisterHandlers(container, appAssemblies, typeof(IQueryRunner<,>));
            container.RegisterDecorator(typeof(IQueryRunner<,>), typeof(QueryRunnerLoggingDecorator<,>));
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
