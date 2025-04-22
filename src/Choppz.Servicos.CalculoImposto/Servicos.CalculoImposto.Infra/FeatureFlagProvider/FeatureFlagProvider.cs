using Microsoft.Extensions.Configuration;
using Servicos.CalculoImposto.Core.Abstractions.FeatureFlag;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Infra.FeatureFlagProviderService
{
    public sealed class FeatureFlagProvider : IFeatureFlagProvider
    {
        private readonly Dictionary<string, bool> _cache;
        private readonly IConfiguration _config;

        public FeatureFlagProvider(IConfiguration config)
        {
            _config = config;
            _cache = new Dictionary<string, bool>();
        }

        public bool IsEnabled(EFeatureFlagType feature)
        {
            var featureName = feature.ToString();

            if (_cache.TryGetValue(featureName, out var isEnabled))
            {
                return isEnabled;
            }

            var configValue = _config.GetSection($"FeatureFlags:{featureName}").Value;
            isEnabled = bool.TryParse(configValue, out var parsedValue) && parsedValue;

            _cache[featureName] = isEnabled;
            return isEnabled;
        }
    }

}

