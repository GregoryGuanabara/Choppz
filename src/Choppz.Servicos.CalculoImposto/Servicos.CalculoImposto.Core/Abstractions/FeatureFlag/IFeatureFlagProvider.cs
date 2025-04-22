using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Core.Abstractions.FeatureFlag
{
    public interface IFeatureFlagProvider
    {
        bool IsEnabled(EFeatureFlagType feature);
    }
}