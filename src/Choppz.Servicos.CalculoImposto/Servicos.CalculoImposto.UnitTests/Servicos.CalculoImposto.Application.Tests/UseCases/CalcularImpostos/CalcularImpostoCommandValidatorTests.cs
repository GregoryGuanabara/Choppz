using Bogus;
using FluentAssertions;
using FluentValidation.TestHelper;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Application.UseCases.CalcularImposto;
using Servicos.CalculoImposto.UnitTests.Servicos.CalculoImposto.Application.Tests.Builders;

namespace Servicos.CalculoImposto.Application.Tests.UseCases.CalcularImpostos
{
    public class CalcularImpostoCommandValidatorTests
    {
        private readonly CalcularImpostoCommandValidator _validador;
        private readonly Faker _geradorDados;

        public CalcularImpostoCommandValidatorTests()
        {
            _validador = new CalcularImpostoCommandValidator();
            _geradorDados = new Faker();
        }

        [Fact]
        public void Validador_DeveTerErro_QuandoPedidoIdForVazio()
        {
            // Arrange
            var comando = new CalcularImpostoCommand(0, _geradorDados.Random.Int(1, 1000), PedidoItemModelBuilder.CriarItensValidos());

            // Act & Assert
            var resultado = _validador.TestValidate(comando);
            resultado.ShouldHaveValidationErrorFor(x => x.PedidoId)
                .WithErrorMessage("O ID do pedido não pode ser vazio.");
        }

        [Fact]
        public void Validador_DevePassar_QuandoForValido()
        {
            // Arrange
            var comando = new CalcularImpostoCommand(_geradorDados.Random.Int(1, 1000), _geradorDados.Random.Int(1, 1000), PedidoItemModelBuilder.CriarItensValidos());

            // Act & Assert
            var resultado = _validador.TestValidate(comando);
            resultado.ShouldNotHaveValidationErrorFor(x => x.PedidoId);
        }

        [Fact]
        public void Validador_DeveTerErro_QuandoClienteIdForVazio()
        {
            // Arrange
            var comando = new CalcularImpostoCommand(_geradorDados.Random.Int(1, 1000), 0, PedidoItemModelBuilder.CriarItensValidos());

            // Act & Assert
            var resultado = _validador.TestValidate(comando);
            resultado.ShouldHaveValidationErrorFor(x => x.ClienteId)
                .WithErrorMessage("O ID do cliente não pode ser vazio.");
        }

        [Fact]
        public void Validador_DeveTerErro_QuandoListaItensForVazia()
        {
            // Arrange
            var comando = new CalcularImpostoCommand(_geradorDados.Random.Int(1, 1000), 0, new List<PedidoItemModel>());

            // Act & Assert
            var resultado = _validador.TestValidate(comando);
            resultado.ShouldHaveValidationErrorFor(x => x.Itens)
                .WithErrorMessage("A lista de itens não pode ser vazia.");
        }

        [Fact]
        public void Validador_DeveTerErrosParaTodosCamposInvalidos()
        {
            // Arrange
            var comando = new CalcularImpostoCommand(0, 0, new List<PedidoItemModel>());

            // Act
            var resultado = _validador.TestValidate(comando);

            // Assert
            resultado.Errors.Should().HaveCount(5);
            resultado.ShouldHaveValidationErrorFor(x => x.PedidoId);
            resultado.ShouldHaveValidationErrorFor(x => x.ClienteId);
            resultado.ShouldHaveValidationErrorFor(x => x.Itens);
        }
    }
}