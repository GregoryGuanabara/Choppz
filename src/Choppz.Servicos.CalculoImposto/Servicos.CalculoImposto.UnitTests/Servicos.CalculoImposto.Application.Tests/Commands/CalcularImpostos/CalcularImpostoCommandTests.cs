using Bogus;
using FluentAssertions;
using Servicos.CalculoImposto.Application.Commands.CalcularImposto;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.UnitTests.Servicos.CalculoImposto.Application.Tests.Builders;

namespace Servicos.CalculoImposto.Application.Tests.Commands.CalcularImposto
{
    public class CalcularImpostoCommandTests
    {
        private readonly Faker _faker = new Faker();

        [Fact]
        public void Constructor_DeveInicializarCorretamente()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 1000);
            var clienteId = _faker.Random.Int(1, 1000);
            var itens = PedidoItemModelBuilder.CriarItensValidos();

            // Act
            var command = new CalcularImpostoCommand(pedidoId, clienteId, itens);

            // Assert
            command.PedidoId.Should().Be(pedidoId);
            command.ClienteId.Should().Be(clienteId);
            command.Itens.Should().BeEquivalentTo(itens);
        }


        [Fact]
        public void CalcularValorTotal_ComItensValidos_DeveRetornarSomaCorreta()
        {
            // Arrange
            var itens = new List<PedidoItemModel>
            {
                new PedidoItemModel(1, 2, 100m),
                new PedidoItemModel(2, 3, 50m)  
            };
            var command = new CalcularImpostoCommand(1, 1, itens);

            // Act
            var total = command.CalcularValorTotal();

            // Assert
            total.Should().Be(350m);
        }


        [Fact]
        public void Propriedades_DevemSerPublicasParaEscrita()
        {
            // Arrange
            var command = new CalcularImpostoCommand(1, 1, PedidoItemModelBuilder.CriarItensValidos());

            // Act
            command.PedidoId = 2;
            command.ClienteId = 3;
            command.Itens = new List<PedidoItemModel>();

            // Assert
            command.PedidoId.Should().Be(2);
            command.ClienteId.Should().Be(3);
            command.Itens.Should().BeEmpty();
        }
    }
}