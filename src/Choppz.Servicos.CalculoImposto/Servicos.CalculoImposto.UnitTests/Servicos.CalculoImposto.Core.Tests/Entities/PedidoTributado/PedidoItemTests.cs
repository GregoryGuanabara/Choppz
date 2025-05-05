using Servicos.CalculoImposto.Core.Entities.PedidoTributado;
using FluentAssertions;
using Xunit;
using Bogus;
using Servicos.CalculoImposto.Core.Abstractions.DomainException;

namespace Servicos.CalculoImposto.Core.Tests.Entities
{
    public class PedidoItemTests
    {
        private readonly Faker _faker = new Faker();

        [Fact]
        public void Constructor_ComParametrosValidos_DeveCriarObjetoCorretamente()
        {
            // Arrange
            var produtoId = _faker.Random.Int(1, 1000);
            var quantidade = _faker.Random.Int(1, 10);
            var valor = _faker.Random.Decimal(10, 1000);

            // Act
            var item = new PedidoItem(produtoId, quantidade, valor);

            // Assert
            item.Should().NotBeNull();
            item.ProdutoId.Should().Be(produtoId);
            item.Quantidade.Should().Be(quantidade);
            item.Valor.Should().Be(valor);
        }

        [Theory]
        [InlineData(0, 1, 100.50, "ProdutoId deve ser maior que 0")]
        [InlineData(1, 0, 100.50, "Quantidade deve ser maior que 0")] 
        [InlineData(1, -1, 100.50, "Quantidade deve ser maior que 0")]
        [InlineData(1, 1, -100.50, "Valor unitário não pode ser menor que 0")]
        public void Constructor_ComParametrosInvalidos_DeveLancarArgumentException(
            int produtoId, int quantidade, decimal valor, string paramEsperado)
        {
            // Act & Assert
            var ex = Assert.Throws<DomainException>(
                () => new PedidoItem(produtoId, quantidade, valor)
            );

            ex.Message.Should().Be(paramEsperado);
        }

        [Fact]
        public void Propriedades_DevemTerSetterPrivado()
        {
            var type = typeof(PedidoItem);

            type.GetProperty(nameof(PedidoItem.ProdutoId))!.SetMethod!.IsPrivate.Should().BeTrue();
            type.GetProperty(nameof(PedidoItem.Quantidade))!.SetMethod!.IsPrivate.Should().BeTrue();
            type.GetProperty(nameof(PedidoItem.Valor))!.SetMethod!.IsPrivate.Should().BeTrue();
        }
    }
}