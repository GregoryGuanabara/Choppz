using Microsoft.Extensions.DependencyInjection;
using Servicos.CalculoImposto.Application.Abstractions.EventDispacherService;
using Servicos.CalculoImposto.Infra.Abstractions.EventDispatcher;

namespace Servicos.CalculoImposto.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModuleServices(this IServiceCollection services)
        {
            services
                .AddMediatR()
                .AddEventDispatcher();

            return services;
        }

        private static IServiceCollection AddMediatR(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(ApplicationModule).Assembly);
            });

            return services;
        }

        private static IServiceCollection AddEventDispatcher(this IServiceCollection services)
        {
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            return services;
        }
    }
}