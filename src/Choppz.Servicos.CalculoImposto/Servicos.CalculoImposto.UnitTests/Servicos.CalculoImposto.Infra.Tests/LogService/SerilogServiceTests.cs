using Microsoft.Extensions.Logging;
using NSubstitute;
using Servicos.CalculoImposto.Infra.LogService;

namespace Servicos.CalculoImposto.Infra.Tests.Services
{
    public class SerilogServiceTests
    {
        private readonly SerilogService _serilogService;
        private readonly ILogger<SerilogService> _logger;

        public SerilogServiceTests()
        {
            _logger = Substitute.For<ILogger<SerilogService>>();
            _serilogService = new SerilogService(_logger);
        }

        [Fact]
        public void LogInformation_DeveChamarMetodoCorretoNoLogger()
        {
            // Arrange
            var message = "Mensagem informativa";

            // Act
            _serilogService.LogInformation(message);

            // Assert
            _logger.Received(1).Log(
                LogLevel.Information,
                Arg.Any<EventId>(),
                Arg.Is<object>(x => x.ToString() == message),
                Arg.Any<Exception?>(),
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public void LogWarning_DeveChamarMetodoCorretoNoLogger()
        {
            // Arrange
            var message = "Mensagem de aviso";

            // Act
            _serilogService.LogWarning(message);

            // Assert
            _logger.Received(1).Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(x => x.ToString() == message),
                Arg.Any<Exception?>(),
                Arg.Any<Func<object, Exception?, string>>());
        }

        [Fact]
        public void LogError_DeveChamarMetodoCorretoNoLogger()
        {
            // Arrange
            var exception = new Exception("Erro teste");
            var message = "Mensagem de erro";

            // Act
            _serilogService.LogError(exception, message);

            // Assert
            _logger.Received(1).Log(
                LogLevel.Error,
                Arg.Any<EventId>(),
                Arg.Is<object>(x => x.ToString() == message),
                Arg.Is<Exception>(ex => ex == exception),
                Arg.Any<Func<object, Exception?, string>>());
        }
    }
}