using Servicos.CalculoImposto.Core.Abstractions.Services;

namespace Servicos.CalculoImposto.Core.Services.Impostos
{
    public class ImpostoEmVigorStrategy : IImpostoStrategy
    {
        public decimal Calcular(decimal valorTotalItens) => valorTotalItens * 0.3m;
    }
}