using System;
using System.Reflection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentApp.FunctionsApp;
using SilentApp.Infrastructure;
using SilentApp.Services.Contracts;
using SimpleInjector;

[assembly: FunctionsStartup(typeof(Startup))]
namespace SilentApp.FunctionsApp
{
    public class Startup : FunctionsStartup
    {
        private readonly Container _container = new();

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(this);
            services.AddSingleton<Completion>();

            services.AddScoped<IRequestDispatcher,RequestDispatcher>();

            services.AddSimpleInjector(_container, options =>
            {
                // Prevent the use of hosted services (not supported by Azure Functions).
                options.EnableHostedServiceResolution = false;

                // Allow injecting ILogger into application components
                options.AddLogging();
            });

            DependencyInjectionExtensions.LoadAppAssemblies();

            _container.RegisterAssemblyBootstrappers(configuration);
        }

        public void Configure(IServiceProvider app)
        {
            // Complete the Simple Injector integration (enables cross wiring).
            app.UseSimpleInjector(_container);

            _container.Verify();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            //var configConnectionString = Environment.GetEnvironmentVariable(ConfigurationKeyConstants.ConfigConnectionString);
            var configConnectionString =
                "Endpoint=https://steres-function-app-config.azconfig.io;Id=r9zi-l0-s0:5/IG+vf4WmnWQ7AY8zy5;Secret=FU1zDT2r1yw768Z5JF/e/Kn3lhkhU3Bnhu4Rwd+7E1I=";
            builder.ConfigurationBuilder.AddAzureAppConfiguration(configConnectionString);
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            this.ConfigureServices(builder.Services, configuration);
        }

        // HACK: Triggers the completion of the Simple Injector integration
        public sealed class Completion
        {
            public Completion(Startup s, IServiceProvider app) => s.Configure(app);
        }
    }
}
