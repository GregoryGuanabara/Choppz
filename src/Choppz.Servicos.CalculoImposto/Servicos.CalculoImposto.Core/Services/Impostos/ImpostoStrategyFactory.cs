using Servicos.CalculoImposto.Core.Abstractions.FeatureFlag;
using Servicos.CalculoImposto.Core.Abstractions.Services;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Core.Services.Impostos
{
    public class ImpostoStrategyFactory
    {
        private readonly IFeatureFlagProvider _featureFlagProvider;

        public ImpostoStrategyFactory(IFeatureFlagProvider featureFlagProvider)
        {
            _featureFlagProvider = featureFlagProvider;
        }

        public IImpostoStrategy ObterStrategy()
        {
            return _featureFlagProvider.IsEnabled(EFeatureFlagType.CalculoReformaTributaria)
                ? new ImpostoReformaTributariaStrategy()
                : new ImpostoEmVigorStrategy();
        }
    }
}