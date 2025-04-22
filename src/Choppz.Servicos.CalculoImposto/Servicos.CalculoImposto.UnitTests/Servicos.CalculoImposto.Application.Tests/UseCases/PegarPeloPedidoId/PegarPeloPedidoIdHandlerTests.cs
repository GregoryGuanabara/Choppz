using Bogus;
using FluentAssertions;
using NSubstitute;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Application.Queries.PegarPeloProdutoId;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Application.Tests.UseCases.PegarPeloPedidoId
{
    public class PegarPeloPedidoIdHandlerTests
    {
        private readonly PegarPeloPedidoIdHandler _handler;
        private readonly IPedidoTributadoRepository _pedidoTributadoRepository;
        private readonly Faker _faker;

        public PegarPeloPedidoIdHandlerTests()
        {
            _pedidoTributadoRepository = Substitute.For<IPedidoTributadoRepository>();
            _handler = new PegarPeloPedidoIdHandler(_pedidoTributadoRepository);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_QuandoPedidoExiste_DeveRetornarSucessoComDados()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 100);

            var pedidoTributado = new PedidoTributado(
                pedidoId,
                _faker.Random.Int(1, 100),
                _faker.Random.Decimal(1, 100),
                new List<PedidoItem>
                {
                    new PedidoItem(_faker.Random.Int(1, 10), _faker.Random.Int(1, 5), _faker.Random.Decimal(10, 100))
                });

            _pedidoTributadoRepository.PegarPeloPedidoIdAsNoTrackingAsync(pedidoId)
                .Returns(pedidoTributado);

            var query = new PegarPeloPedidoIdQuery(pedidoId);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Sucesso.Should().BeTrue();
            resultado.Data.Should().BeOfType<PedidoTributadoModel>();
            await _pedidoTributadoRepository.Received(1).PegarPeloPedidoIdAsNoTrackingAsync(pedidoId);
        }

        [Fact]
        public async Task Handle_QuandoPedidoNaoExiste_DeveRetornarErro()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 100);
            _pedidoTributadoRepository.PegarPeloPedidoIdAsNoTrackingAsync(pedidoId)
                .Returns((PedidoTributado?)null);

            var query = new PegarPeloPedidoIdQuery(pedidoId);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            resultado.Sucesso.Should().BeFalse();
            resultado.Data.Should().BeOfType<ResultadoDeErrosModel>().
                Which.Erros.Should().OnlyContain(x => x.Mensagem == "O pedido não foi localizado.");

            await _pedidoTributadoRepository.Received(1).PegarPeloPedidoIdAsNoTrackingAsync(pedidoId);
        }

        [Fact]
        public async Task Handle_DeveConverterItensCorretamente()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 100);
            var itens = new List<PedidoItem>
            {
                new PedidoItem(1, 2, 100m),
                new PedidoItem(2, 1, 50m)
            };

            var pedidoTributado = new PedidoTributado(
                pedidoId,
                _faker.Random.Int(1, 100),
                _faker.Random.Decimal(1, 100),
                itens);

            _pedidoTributadoRepository.PegarPeloPedidoIdAsNoTrackingAsync(pedidoId)
                .Returns(pedidoTributado);

            var query = new PegarPeloPedidoIdQuery(pedidoId);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var model = resultado.Data.Should().BeOfType<PedidoTributadoModel>().Subject;
            model.Itens.Should().HaveCount(2);
            model.Itens[0].ProdutoId.Should().Be(1);
            model.Itens[0].Quantidade.Should().Be(2);
            model.Itens[0].Valor.Should().Be(100m);
            model.Itens[1].ProdutoId.Should().Be(2);
            model.Itens[1].Quantidade.Should().Be(1);
            model.Itens[1].Valor.Should().Be(50m);
        }

        [Fact]
        public async Task Handle_DeveMapearTodosCamposCorretamente()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 100);
            var clienteId = _faker.Random.Int(1, 100);
            var imposto = _faker.Random.Decimal(1, 100);
            var status = EPedidoTributadoStatus.Criado;

            var pedidoTributado = new PedidoTributado(
                pedidoId,
                clienteId,
                imposto,
             new List<PedidoItem>
             {
                 new PedidoItem(1, 2, 100m),
                 new PedidoItem(2, 1, 50m)
             });

            _pedidoTributadoRepository.PegarPeloPedidoIdAsNoTrackingAsync(pedidoId)
                .Returns(pedidoTributado);

            var query = new PegarPeloPedidoIdQuery(pedidoId);

            // Act
            var resultado = await _handler.Handle(query, CancellationToken.None);

            // Assert
            var model = resultado.Data.Should().BeOfType<PedidoTributadoModel>().Subject;
            model.PedidoId.Should().Be(pedidoId);
            model.ClienteId.Should().Be(clienteId);
            model.Imposto.Should().Be(imposto);
            model.Status.Should().Be(status.ToString());
        }
    }
}