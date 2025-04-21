using FluentAssertions;
using FluentValidation.TestHelper;
using Servicos.CalculoImposto.Application.Queries.PegarPeloProdutoId;

namespace Servicos.CalculoImposto.Application.Tests.Queries.PegarPeloProdutoId
{
    public class PegarPeloProdutoIdQueryValidatorTests
    {
        private readonly PegarPeloProdutoIdQueryValidator _validador;

        public PegarPeloProdutoIdQueryValidatorTests()
        {
            _validador = new PegarPeloProdutoIdQueryValidator();
        }

        [Fact]
        public void Validador_DeveTerErro_QuandoPedidoIdForVazio()
        {
            // Arrange
            var query = new PegarPeloPedidoIdQuery(0);

            // Act
            var resultado = _validador.TestValidate(query);

            // Assert
            resultado.ShouldHaveValidationErrorFor(x => x.PedidoId)
                .WithErrorMessage("O ID do pedido não pode ser vazio.");
        }

        [Fact]
        public void Validador_DeveTerErro_QuandoPedidoIdForNegativo()
        {
            // Arrange
            var query = new PegarPeloPedidoIdQuery(-1);

            // Act
            var resultado = _validador.TestValidate(query);

            // Assert
            resultado.ShouldHaveValidationErrorFor(x => x.PedidoId)
                .WithErrorMessage("O ID do pedido deve ser um valor valido.");
        }

        [Fact]
        public void Validador_DeveTerErro_QuandoPedidoIdForZero()
        {
            // Arrange
            var query = new PegarPeloPedidoIdQuery(0);

            // Act
            var resultado = _validador.TestValidate(query);

            // Assert
            resultado.ShouldHaveValidationErrorFor(x => x.PedidoId)
                .WithErrorMessage("O ID do pedido deve ser um valor valido.");
        }

        [Fact]
        public void Validador_DevePassar_QuandoPedidoIdForPositivo()
        {
            // Arrange
            var query = new PegarPeloPedidoIdQuery(1);

            // Act
            var resultado = _validador.TestValidate(query);

            // Assert
            resultado.ShouldNotHaveValidationErrorFor(x => x.PedidoId);
        }

        [Fact]
        public void Validador_DeveTerDoisErros_QuandoPedidoIdForZero()
        {
            // Arrange
            var query = new PegarPeloPedidoIdQuery(0);

            // Act
            var resultado = _validador.TestValidate(query);

            // Assert
            resultado.Errors.Should().HaveCount(2)
                .And.Contain(e => e.ErrorMessage == "O ID do pedido não pode ser vazio.")
                .And.Contain(e => e.ErrorMessage == "O ID do pedido deve ser um valor valido.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        public void Validador_DevePassar_QuandoPedidoIdForValido(int pedidoId)
        {
            // Arrange
            var query = new PegarPeloPedidoIdQuery(pedidoId);

            // Act
            var resultado = _validador.TestValidate(query);

            // Assert
            resultado.ShouldNotHaveValidationErrorFor(x => x.PedidoId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Validador_DeveFalhar_QuandoPedidoIdForInvalido(int pedidoId)
        {
            // Arrange
            var query = new PegarPeloPedidoIdQuery(pedidoId);

            // Act
            var resultado = _validador.TestValidate(query);

            // Assert
            resultado.ShouldHaveValidationErrorFor(x => x.PedidoId);
        }
    }
}