using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Servicos.CalculoImposto.Core.Abstractions.CacheService;
using Servicos.CalculoImposto.Core.Abstractions.FeatureFlag;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Abstractions.UnitOfWork;
using Servicos.CalculoImposto.Infra.FeatureFlagProviderService;
using Servicos.CalculoImposto.Infra.Persistence;
using Servicos.CalculoImposto.Infra.Persistence.Repositories;
using Servicos.CalculoImposto.Infra.Persistence.UnitOfWork;
using Servicos.CalculoImposto.Infra.Service.Cache;

namespace Servicos.CalculoImposto.Infra
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructureModuleServices(this IServiceCollection services)
        {
            services
                .AddDatabase()
                .AddRepositories()
                .AddFeatureFlagProvider()
                .AddCacheService();

            return services;
        }

        private static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("EmMemoria"));
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IPedidoTributadoRepository, PedidoTributadoRepository>();

            return services;
        }

        private static IServiceCollection AddFeatureFlagProvider(this IServiceCollection services)
        {
            services.AddSingleton<IFeatureFlagProvider, FeatureFlagProvider>();
            return services;
        }

        private static IServiceCollection AddCacheService(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, CacheService>();

            return services;
        }
    }
}