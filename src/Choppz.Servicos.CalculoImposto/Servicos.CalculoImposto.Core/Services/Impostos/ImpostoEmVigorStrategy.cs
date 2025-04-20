using Servicos.CalculoImposto.Core.Abstractions.Services;

namespace Servicos.CalculoImposto.Core.Services.Impostos
{
    internal class ImpostoEmVigorStrategy : IImpostoStrategy
    {
        public decimal Calcular(decimal valorTotalItens) => valorTotalItens * 0.3m;
    }
}