using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Servicos.CalculoImposto.Application.Abstractions.Behaviors;
using Servicos.CalculoImposto.Application.Abstractions.EventDispacherService;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Application.Queries.PegarPeloProdutoId;
using Servicos.CalculoImposto.Application.UseCases.CalcularImposto;
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

            services.AddValidatorsFromAssemblyContaining<CalcularImpostoCommandValidator>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient<IPipelineBehavior<PegarPeloPedidoIdQuery, RespostaPadronizadaModel>, PegarPeloPedidoIdQueryBehavior<PegarPeloPedidoIdQuery, RespostaPadronizadaModel>>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            return services;
        }

        private static IServiceCollection AddEventDispatcher(this IServiceCollection services)
        {
            services.AddScoped<IEventDispatcher, EventDispatcher>();
            return services;
        }
    }
}