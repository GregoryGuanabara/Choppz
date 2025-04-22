using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Servicos.CalculoImposto.Core.Enums;
using Servicos.CalculoImposto.Infra.FeatureFlagProviderService;

namespace Servicos.CalculoImposto.Infra.Tests.Services
{
    public class FeatureFlagProviderTests
    {
        private readonly FeatureFlagProvider _featureFlagProvider;
        private readonly IConfiguration _config;
        private readonly EFeatureFlagType _testFeature = EFeatureFlagType.CalculoReformaTributaria;

        public FeatureFlagProviderTests()
        {
            _config = Substitute.For<IConfiguration>();
            _featureFlagProvider = new FeatureFlagProvider(_config);
        }

        [Fact]
        public void IsEnabled_DeveRetornarValorDoCache_QuandoFeatureEstaEmCache()
        {
            // Arrange
            var featureName = _testFeature.ToString();
            var cacheField = _featureFlagProvider.GetType()
                .GetField("_cache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Verificação de segurança
            cacheField.Should().NotBeNull("O campo '_cache' não foi encontrado na classe FeatureFlagProvider");

            var cache = new Dictionary<string, bool> { { featureName, true } };
            cacheField!.SetValue(_featureFlagProvider, cache);

            // Act
            var result = _featureFlagProvider.IsEnabled(_testFeature);

            // Assert
            result.Should().BeTrue();
            _config.DidNotReceive().GetSection(Arg.Any<string>());
        }

        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("", false)]
        [InlineData("invalid", false)]
        public void IsEnabled_DeveRetornarCorretamente_BaseadoNaConfiguracao(string configValue, bool expected)
        {
            // Arrange
            var configSection = Substitute.For<IConfigurationSection>();
            configSection.Value.Returns(configValue);
            _config.GetSection($"FeatureFlags:{_testFeature}").Returns(configSection);

            // Act
            var result = _featureFlagProvider.IsEnabled(_testFeature);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void IsEnabled_DeveAdicionarAoCache_AposPrimeiraConsulta()
        {
            // Arrange
            var configSection = Substitute.For<IConfigurationSection>();
            configSection.Value.Returns("true");
            _config.GetSection($"FeatureFlags:{_testFeature}").Returns(configSection);

            // Primeira chamada - deve consultar config
            var firstResult = _featureFlagProvider.IsEnabled(_testFeature);

            // Act - Segunda chamada - deve usar cache
            var secondResult = _featureFlagProvider.IsEnabled(_testFeature);

            // Assert
            firstResult.Should().BeTrue();
            secondResult.Should().BeTrue();
            _config.Received(1).GetSection(Arg.Any<string>());
        }

        [Fact]
        public void IsEnabled_DeveRetornarFalse_QuandoConfiguracaoNaoContemFeature()
        {
            // Arrange
            var configSection = Substitute.For<IConfigurationSection>();
            configSection.Value.Returns((string?)null);
            _config.GetSection($"FeatureFlags:{_testFeature}").Returns(configSection);

            // Act
            var result = _featureFlagProvider.IsEnabled(_testFeature);

            // Assert
            result.Should().BeFalse();
        }
    }
}