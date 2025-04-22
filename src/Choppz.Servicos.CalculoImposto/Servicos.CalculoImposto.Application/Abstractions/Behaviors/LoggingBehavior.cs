using MediatR;
using Microsoft.Extensions.Logging;

namespace Servicos.CalculoImposto.Application.Abstractions.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var operationId = Guid.NewGuid();

            try
            {
                _logger.LogInformation(
                    "[{OperationId}-{RequestType}] Iniciando - Dados: {@RequestData}",
                    operationId,
                    typeof(TRequest).Name.ToUpper(),
                    request);

                var response = await next();

                _logger.LogInformation(
                    "[{OperationId}-{RequestType}] Finalizado com sucesso",
                    operationId,
                    typeof(TRequest).Name.ToUpper());

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "[{OperationId}-{RequestType}] Falha ao processar",
                    operationId,
                    typeof(TRequest).Name.ToUpper());
                throw;
            }
        }
    }
}