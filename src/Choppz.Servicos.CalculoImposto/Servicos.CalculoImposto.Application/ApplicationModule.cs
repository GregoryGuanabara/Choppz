using Microsoft.Extensions.DependencyInjection;

namespace Servicos.CalculoImposto.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModuleServices(this IServiceCollection services)
        {
            services
                .AddMediatR();

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
    }
}