using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using Servicos.CalculoImposto.Infra.Service.Cache;

namespace Servicos.CalculoImposto.Infra.Tests.Services
{
    public class CacheServiceTests
    {
        private readonly CacheService _cacheService;
        private readonly IMemoryCache _memoryCache;

        public CacheServiceTests()
        {
            _memoryCache = Substitute.For<IMemoryCache>();
            _cacheService = new CacheService(_memoryCache);
        }

        [Fact]
        public void Pegar_DeveRetornarValor_QuandoChaveExiste()
        {
            // Arrange
            var expectedValue = "valor_teste";
            var key = "valid_key";

            _memoryCache.TryGetValue(key, out Arg.Any<object?>())
                .Returns(x =>
                {
                    x[1] = expectedValue;
                    return true;
                });

            // Act
            var result = _cacheService.Pegar<string>(key);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Fact]
        public void Pegar_DeveRetornarDefault_QuandoChaveNaoExiste()
        {
            // Arrange
            object? outValue = null;
            _memoryCache.TryGetValue("invalid_key", out outValue)
                .Returns(x => false);

            // Act
            var result = _cacheService.Pegar<string>("invalid_key");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Remover_DeveRemoverItemDoCache()
        {
            // Arrange
            var key = "key_to_remove";

            // Act
            _cacheService.Remover(key);

            // Assert
            _memoryCache.Received(1).Remove(key);
        }

        [Fact]
        public void Existe_DeveRetornarTrue_QuandoChaveExiste()
        {
            // Arrange
            object? dummy;
            _memoryCache.TryGetValue("existing_key", out dummy)
                .Returns(x => true);

            // Act
            var existe = _cacheService.Existe("existing_key");

            // Assert
            Assert.True(existe);
        }

        [Fact]
        public void Existe_DeveRetornarFalse_QuandoChaveNaoExiste()
        {
            // Arrange
            object? dummy;
            _memoryCache.TryGetValue("nonexistent_key", out dummy)
                .Returns(x => false);

            // Act
            var existe = _cacheService.Existe("nonexistent_key");

            // Assert
            Assert.False(existe);
        }
    }
}