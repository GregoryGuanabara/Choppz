namespace Servicos.CalculoImposto.Infra.Abstractions.LogService
{
    public interface ILoggerService
    {
        void LogInformation(string message);

        void LogWarning(string message);

        void LogError(Exception ex, string message);
    }
}