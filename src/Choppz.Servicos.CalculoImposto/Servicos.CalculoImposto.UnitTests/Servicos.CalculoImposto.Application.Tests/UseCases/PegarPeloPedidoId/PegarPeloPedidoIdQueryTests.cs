using Bogus;
using FluentAssertions;
using MediatR;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Application.Queries.PegarPeloProdutoId;

namespace Servicos.CalculoImposto.Application.Tests.UseCases.PegarPeloPedidoId
{
    public class PegarPeloPedidoIdQueryTests
    {
        private readonly Faker _faker;

        public PegarPeloPedidoIdQueryTests()
        {
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public void Construtor_DeveInicializarPedidoIdCorretamente()
        {
            // Arrange
            var pedidoIdEsperado = _faker.Random.Int(1, 1000);

            // Act
            var query = new PegarPeloPedidoIdQuery(pedidoIdEsperado);

            // Assert
            query.PedidoId.Should().Be(pedidoIdEsperado);
        }

        [Fact]
        public void PropriedadePedidoId_DeveSerModificavel()
        {
            // Arrange
            var pedidoIdInicial = _faker.Random.Int(1, 1000);
            var query = new PegarPeloPedidoIdQuery(pedidoIdInicial);
            var novoPedidoId = pedidoIdInicial + 1;

            // Act
            query.PedidoId = novoPedidoId;

            // Assert
            query.PedidoId.Should().Be(novoPedidoId);
        }

        [Fact]
        public void Record_DeveTerIgualdadePorValor()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 1000);
            var query1 = new PegarPeloPedidoIdQuery(pedidoId);
            var query2 = new PegarPeloPedidoIdQuery(pedidoId);

            // Act & Assert
            query1.Should().Be(query2);
            query1.GetHashCode().Should().Be(query2.GetHashCode());
        }

        [Fact]
        public void DeveImplementarIRequest()
        {
            // Arrange
            var query = new PegarPeloPedidoIdQuery(_faker.Random.Int());

            // Act & Assert
            query.Should().BeAssignableTo<IRequest<RespostaPadronizadaModel>>();
        }
    }
}