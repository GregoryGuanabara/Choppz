using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Servicos.CalculoImposto.Core.Abstractions.CacheService;
using Servicos.CalculoImposto.Core.Abstractions.FeatureFlag;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Abstractions.UnitOfWork;
using Servicos.CalculoImposto.Infra.Abstractions.LogService;
using Servicos.CalculoImposto.Infra.FeatureFlagProviderService;
using Servicos.CalculoImposto.Infra.LogService;
using Servicos.CalculoImposto.Infra.MessageBus;
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
                .AddLoggerService()
                .AddRepositories()
                .AddFeatureFlagProvider()
                .AddCacheService()
                .AddMessageBusService();

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
            services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();

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

        private static IServiceCollection AddMessageBusService(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IMessageBusService, MessageBusService>();

            return services;
        }

        private static IServiceCollection AddLoggerService(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            services.AddSingleton(Log.Logger);

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });

            services.AddSingleton<ILoggerService, SerilogService>();

            return services;
        }
    }
}