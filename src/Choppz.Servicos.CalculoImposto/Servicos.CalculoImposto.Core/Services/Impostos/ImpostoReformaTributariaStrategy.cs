using Servicos.CalculoImposto.Core.Abstractions.Services;

namespace Servicos.CalculoImposto.Core.Services.Impostos
{
    public class ImpostoReformaTributariaStrategy : IImpostoStrategy
    {
        public decimal Calcular(decimal valorTotalItens) => valorTotalItens * 0.2m;
    }
}