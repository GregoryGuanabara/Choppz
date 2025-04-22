namespace Servicos.CalculoImposto.Core.Abstractions.Services
{
    public interface IImpostoService
    {
        decimal CalcularImposto(decimal valorTotalItens);
    }
}