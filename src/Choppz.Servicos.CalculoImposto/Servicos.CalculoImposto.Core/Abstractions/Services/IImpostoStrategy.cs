namespace Servicos.CalculoImposto.Core.Abstractions.Services
{
    public interface IImpostoStrategy
    {
        decimal Calcular(decimal valorTotalItens);
    }
}