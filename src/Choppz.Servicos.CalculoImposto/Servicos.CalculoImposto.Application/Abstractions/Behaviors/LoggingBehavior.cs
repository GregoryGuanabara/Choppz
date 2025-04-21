using MediatR;
using Microsoft.Extensions.Logging;

namespace Servicos.CalculoImposto.Application.Abstractions.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var operationId = Guid.NewGuid();
            _logger.LogInformation($"[{operationId}-{typeof(TRequest).Name.ToUpper()}] - Dados: {request}");

            var response = await next();

            _logger.LogInformation($"[{operationId}-{typeof(TRequest).Name.ToUpper()}] - finalizado");

            return response;
        }
    }
}