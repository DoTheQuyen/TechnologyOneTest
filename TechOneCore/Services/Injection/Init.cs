using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechOneCore.Services.Interface;
using TechOneCore.Services.Service;

namespace TechOneCore.Services.Injection
{
    /// <summary>
    /// Provides extension methods for registering shared services in the dependency injection container.
    /// So that the services can be used in other projects like TechOneAPI, and no need to register in Program.cs of TechOneAPI
    /// </summary>
    public static class Init
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            #region Services registration
            // Register the NumberToWordsConverter service with a scoped lifetime
            // Scoped lifetime means a new instance of the service will be created for each HTTP request
            services.AddScoped<INumberToWordsConverter, NumberToWordsConverter>();

            #endregion


            return services;
        }

    }
}
