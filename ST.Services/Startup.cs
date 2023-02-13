﻿using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SilentApp.Infrastructure;
using SimpleInjector;

namespace SilentApp.FunctionApp
{
    public class Startup : FunctionsStartup
    {
        private readonly Container _container = new();

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(this);
            services.AddSingleton<Completion>();

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
