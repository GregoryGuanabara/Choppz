using Microsoft.Extensions.DependencyInjection;
using Servicos.CalculoImposto.Core.Abstractions.Services;
using Servicos.CalculoImposto.Core.Services.Impostos;

namespace Servicos.CalculoImposto.Core
{
    public static class CoreModule
    {
        public static IServiceCollection AddCoreModuleServices(this IServiceCollection services)
        {
            services
                .AddCoreServices();

            return services;
        }

        private static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IImpostoService, ImpostoService>();

            services.AddTransient<ImpostoStrategyFactory>();

            services.AddScoped<IImpostoStrategyFactory, ImpostoStrategyFactory>();
            services.AddScoped<IImpostoStrategy, ImpostoEmVigorStrategy>();
            services.AddScoped<IImpostoStrategy, ImpostoReformaTributariaStrategy>();

            return services;
        }
    }
}