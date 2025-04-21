using Microsoft.Extensions.Logging;
using Servicos.CalculoImposto.Infra.Abstractions.LogService;

namespace Servicos.CalculoImposto.Infra.LogService
{
    public class SerilogService : ILoggerService
    {
        private readonly ILogger<SerilogService> _logger;

        public SerilogService(ILogger<SerilogService> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(Exception ex, string message)
        {
            _logger.LogError(ex, message);
        }
    }
}