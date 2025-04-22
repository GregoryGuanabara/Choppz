using FluentAssertions;
using FluentValidation.TestHelper;
using Servicos.CalculoImposto.Application.Queries.PegarTodos;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Application.Tests.UseCases.PegarTodos
{
    public class PegarTodosQueryValidatorTests
    {
        private readonly PegarTodosQueryValidator _validador;

        public PegarTodosQueryValidatorTests()
        {
            _validador = new PegarTodosQueryValidator();
        }

        [Fact]
        public void Validador_DeveAceitarStatusValido()
        {
            // Arrange
            var query = new PegarTodosQuery(EPedidoTributadoStatus.Criado, 1, 10);

            // Act & Assert
            var resultado = _validador.TestValidate(query);
            resultado.ShouldNotHaveValidationErrorFor(x => x.Status);
        }

        [Fact]
        public void Validador_DeveAceitarStatusNulo()
        {
            // Arrange
            var query = new PegarTodosQuery(null, 1, 10);

            // Act & Assert
            var resultado = _validador.TestValidate(query);
            resultado.ShouldNotHaveValidationErrorFor(x => x.Status);
        }

        [Fact]
        public void Validador_DeveRejeitarStatusInvalido()
        {
            // Arrange
            var query = new PegarTodosQuery((EPedidoTributadoStatus)999, 1, 10);

            // Act & Assert
            var resultado = _validador.TestValidate(query);
            resultado.ShouldHaveValidationErrorFor(x => x.Status)
                .WithErrorMessage("O status do pedido é inválido.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Validador_DeveRejeitarPaginaInvalida(int paginaInvalida)
        {
            // Arrange
            var query = new PegarTodosQuery(null, paginaInvalida, 10);

            // Act & Assert
            var resultado = _validador.TestValidate(query);
            resultado.ShouldHaveValidationErrorFor(x => x.Pagina)
                .WithErrorMessage("O valor de pagina não é valido.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(100)]
        public void Validador_DeveAceitarPaginaValida(int paginaValida)
        {
            // Arrange
            var query = new PegarTodosQuery(null, paginaValida, 10);

            // Act & Assert
            var resultado = _validador.TestValidate(query);
            resultado.ShouldNotHaveValidationErrorFor(x => x.Pagina);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validador_DeveRejeitarItensPorPaginaMenorQue1(int itensInvalidos)
        {
            // Arrange
            var query = new PegarTodosQuery(null, 1, itensInvalidos);

            // Act & Assert
            var resultado = _validador.TestValidate(query);
            resultado.ShouldHaveValidationErrorFor(x => x.ItensPorPagina)
                .WithErrorMessage("O valor de itens por pagina não é valido. Mínimo permitido = 1.");
        }

        [Theory]
        [InlineData(101)]
        [InlineData(200)]
        public void Validador_DeveRejeitarItensPorPaginaMaiorQue100(int itensInvalidos)
        {
            // Arrange
            var query = new PegarTodosQuery(null, 1, itensInvalidos);

            // Act & Assert
            var resultado = _validador.TestValidate(query);
            resultado.ShouldHaveValidationErrorFor(x => x.ItensPorPagina)
                .WithErrorMessage("O valor de itens por pagina não é valido. Máximo permitido = 100.");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(100)]
        public void Validador_DeveAceitarItensPorPaginaValidos(int itensValidos)
        {
            // Arrange
            var query = new PegarTodosQuery(null, 1, itensValidos);

            // Act & Assert
            var resultado = _validador.TestValidate(query);
            resultado.ShouldNotHaveValidationErrorFor(x => x.ItensPorPagina);
        }

        [Fact]
        public void Validador_DeveRetornarMultiplosErros_QuandoMultiplosCamposInvalidos()
        {
            // Arrange
            var query = new PegarTodosQuery((EPedidoTributadoStatus)999, 0, 0);

            // Act
            var resultado = _validador.TestValidate(query);

            // Assert
            resultado.Errors.Should().HaveCount(3);
            resultado.ShouldHaveValidationErrorFor(x => x.Status);
            resultado.ShouldHaveValidationErrorFor(x => x.Pagina);
            resultado.ShouldHaveValidationErrorFor(x => x.ItensPorPagina);
        }
    }
}