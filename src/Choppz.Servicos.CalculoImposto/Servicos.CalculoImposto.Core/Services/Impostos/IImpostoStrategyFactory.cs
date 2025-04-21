using Servicos.CalculoImposto.Core.Abstractions.Services;

namespace Servicos.CalculoImposto.Core.Services.Impostos
{
    public interface IImpostoStrategyFactory
    {
        IImpostoStrategy ObterStrategy();
    }
}