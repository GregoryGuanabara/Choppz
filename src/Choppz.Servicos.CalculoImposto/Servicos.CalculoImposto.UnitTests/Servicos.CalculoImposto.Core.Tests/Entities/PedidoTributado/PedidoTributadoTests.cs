using Bogus;
using FluentAssertions;
using Servicos.CalculoImposto.Core.Abstractions.DomainException;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Core.Tests.Entities
{
    public class PedidoTributadoTests
    {
        private readonly Faker _faker = new Faker();

        private List<PedidoItem> CriarItensValidos(int quantidade = 1)
        {
            var itens = new List<PedidoItem>();
            for (int i = 0; i < quantidade; i++)
            {
                itens.Add(new PedidoItem(
                    _faker.Random.Int(1, 100),
                    _faker.Random.Int(1, 10),
                    _faker.Random.Decimal(10, 1000)));
            }
            return itens;
        }

        [Fact]
        public void Constructor_ComParametrosValidos_DeveCriarObjetoCorretamente()
        {
            // Arrange
            var itens = CriarItensValidos();

            // Act
            var pedido = new PedidoTributado(1, 1, 10m, itens);

            // Assert
            pedido.PedidoId.Should().Be(1);
            pedido.ClienteId.Should().Be(1);
            pedido.Imposto.Should().Be(10m);
            pedido.Status.Should().Be(EPedidoTributadoStatus.Criado);
            pedido.Itens.Should().BeEquivalentTo(itens);
        }

        [Theory]
        [InlineData(0, 1, 10, "ID do pedido inválido")]
        [InlineData(-1, 1, 10, "ID do pedido inválido")]
        [InlineData(1, 0, 10, "ID do cliente inválido")]
        [InlineData(1, -1, 10, "ID do cliente inválido")]
        [InlineData(1, 1, -1, "Valor do imposto não pode ser negativo")]
        public void Constructor_ComParametrosInvalidos_DeveLancarArgumentException(
            int pedidoId, int clienteId, decimal imposto, string paramEsperado)
        {
            // Arrange
            var itens = CriarItensValidos();

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new PedidoTributado(pedidoId, clienteId, imposto, itens));

            ex.Message.Should().Be(paramEsperado);
        }

        [Fact]
        public void Constructor_ComListaItensVazia_DeveLancarException()
        {
            // Arrange
            var itensVazios = new List<PedidoItem>();

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new PedidoTributado(1, 1, 10m, itensVazios))
                .Message.Should().Be("Pedido deve conter pelo menos um item");
        }

        [Fact]
        public void Constructor_ComItensNulos_DeveLancarException()
        {
            // Arrange
            var itensNulos = (List<PedidoItem>?)null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new PedidoTributado(1, 1, 10m, itensNulos!))
                .Message.Should().Be("Lista de itens não pode ser nula");
        }

        [Fact]
        public void Cancelar_DeveAtualizarStatusParaCancelado()
        {
            // Arrange
            var pedido = new PedidoTributado(1, 1, 10m, CriarItensValidos());
            var dataOriginal = pedido.ModificadoEm;

            // Act
            pedido.Cancelar();

            // Assert
            pedido.Status.Should().Be(EPedidoTributadoStatus.Cancelado);
            pedido.ModificadoEm.Should().BeAfter(dataOriginal);
        }

        [Fact]
        public void Cancelar_QuandoJaCancelado_NaoDeveAlterarDataNovamente()
        {
            // Arrange
            var pedido = new PedidoTributado(1, 1, 10m, CriarItensValidos());
            pedido.Cancelar();
            var dataCancelamento = pedido.ModificadoEm;

            // Act
            pedido.Cancelar();

            // Assert
            pedido.ModificadoEm.Should().Be(dataCancelamento);
        }

        [Fact]
        public void Propriedades_DevemTerAcessibilidadeCorreta()
        {
            // Arrange
            var type = typeof(PedidoTributado);

            // Assert
            type.GetProperty(nameof(PedidoTributado.PedidoId))!.SetMethod!.IsPrivate.Should().BeTrue();
            type.GetProperty(nameof(PedidoTributado.ClienteId))!.SetMethod!.IsPrivate.Should().BeTrue();
            type.GetProperty(nameof(PedidoTributado.Status))!.SetMethod!.IsPrivate.Should().BeTrue();
            type.GetProperty(nameof(PedidoTributado.Imposto))!.SetMethod!.IsPrivate.Should().BeTrue();
            type.GetProperty(nameof(PedidoTributado.Itens))!.PropertyType.Should().BeAssignableTo<IReadOnlyList<PedidoItem>>();
        }
    }
}