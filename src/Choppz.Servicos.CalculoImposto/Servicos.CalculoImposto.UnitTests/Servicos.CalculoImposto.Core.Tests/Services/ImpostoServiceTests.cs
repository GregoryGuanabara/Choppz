using FluentAssertions;
using NSubstitute;
using Servicos.CalculoImposto.Core.Services.Impostos;

namespace Servicos.CalculoImposto.Core.Tests.Services
{
    public class ImpostoServiceTests
    {
        private readonly IImpostoStrategyFactory _factorySub;
        private readonly ImpostoService _service;

        public ImpostoServiceTests()
        {
            _factorySub = Substitute.For<IImpostoStrategyFactory>();
            _service = new ImpostoService(_factorySub);
        }

        [Fact]
        public void CalcularImposto_ComStrategyReformaTributaria_DeveRetornar20PorCento()
        {
            // Arrange
            var valor = 1000m;
            var strategy = new ImpostoReformaTributariaStrategy(); 
            _factorySub.ObterStrategy().Returns(strategy);

            // Act
            var resultado = _service.CalcularImposto(valor);

            // Assert
            resultado.Should().Be(200m);
            _factorySub.Received(1).ObterStrategy();
        }

        [Fact]
        public void CalcularImposto_ComStrategyImpostoEmVigor_DeveRetornarPor30Cento()
        {
            // Arrange
            var valor = 1000m;
            var strategy = new ImpostoEmVigorStrategy();
            _factorySub.ObterStrategy().Returns(strategy);

            // Act
            var resultado = _service.CalcularImposto(valor);

            // Assert
            resultado.Should().Be(300m); 
            _factorySub.Received(1).ObterStrategy();
        }

        [Fact]
        public void CalcularImposto_DeveUsarStrategyCorretaConformeContexto()
        {
            // Arrange
            var valor = 1000m;

            _factorySub.ObterStrategy().Returns(
                new ImpostoReformaTributariaStrategy(),
                new ImpostoEmVigorStrategy()
            );

            // Act & Assert
            var resultado1 = _service.CalcularImposto(valor);
            resultado1.Should().Be(200m);

            // Segunda
            var resultado2 = _service.CalcularImposto(valor);
            resultado2.Should().Be(300m);

            _factorySub.Received(2).ObterStrategy();
        }

        [Theory]
        [InlineData(-100)]
        public void CalcularImposto_ComValorInvalido_DeveLancarArgumentException(decimal valorInvalido)
        {
            Assert.Throws<ArgumentException>(
                () => _service.CalcularImposto(valorInvalido)
            );
        }
    }
}