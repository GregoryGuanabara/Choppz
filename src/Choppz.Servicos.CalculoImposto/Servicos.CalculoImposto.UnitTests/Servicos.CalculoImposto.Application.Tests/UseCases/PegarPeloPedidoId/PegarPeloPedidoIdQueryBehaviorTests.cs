using Bogus;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Application.Queries.PegarPeloProdutoId;
using Servicos.CalculoImposto.Core.Abstractions.CacheService;
using Servicos.CalculoImposto.Core.Enums;
using Servicos.CalculoImposto.UnitTests.Servicos.CalculoImposto.Application.Tests.Builders;

namespace Servicos.CalculoImposto.Application.Tests.UseCases.PegarPeloPedidoId
{
    public class PegarPeloPedidoIdQueryBehaviorTests
    {
        private readonly PegarPeloPedidoIdQueryBehavior<IRequest<RespostaPadronizadaModel>, RespostaPadronizadaModel> _behavior;
        private readonly ICacheService _cacheService;
        private readonly Faker _faker;

        public PegarPeloPedidoIdQueryBehaviorTests()
        {
            _cacheService = Substitute.For<ICacheService>();
            _behavior = new PegarPeloPedidoIdQueryBehavior<IRequest<RespostaPadronizadaModel>, RespostaPadronizadaModel>(_cacheService);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_QuandoPedidoEstaNoCache_DeveRetornarDoCache()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 100);
            var request = new PegarPeloPedidoIdQuery(pedidoId);
            var response = new PedidoTributadoModel(_faker.Random.Int(1, 10), pedidoId, _faker.Random.Int(1, 10), _faker.Random.Decimal(1, 10), PedidoItemModelBuilder.CriarItensValidos(), EPedidoTributadoStatus.Criado);
            var cachedResponse = RespostaPadronizadaModel.ComSucesso(response);

            _cacheService.Pegar<RespostaPadronizadaModel>($"Pedido-{pedidoId}")
                .Returns(cachedResponse);

            var result = await _behavior.Handle(request, _ => Task.FromResult(cachedResponse), CancellationToken.None);

            // Assert
            result.Should().Be(cachedResponse);
            _cacheService.Received(1).Pegar<RespostaPadronizadaModel>($"Pedido-{pedidoId}");
        }

        [Fact]
        public async Task Handle_QuandoPedidoNaoEstaNoCache_DeveChamarProximoHandler()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 100);
            var request = new PegarPeloPedidoIdQuery(pedidoId);
            var response = new PedidoTributadoModel(_faker.Random.Int(1, 10), pedidoId, _faker.Random.Int(1, 10), _faker.Random.Decimal(1, 10), PedidoItemModelBuilder.CriarItensValidos(), EPedidoTributadoStatus.Criado);
            var expectedResponse = RespostaPadronizadaModel.ComSucesso(response);

            _cacheService.Pegar<RespostaPadronizadaModel>($"Pedido-{pedidoId}")
                .Returns((RespostaPadronizadaModel?)null);

            RequestHandlerDelegate<RespostaPadronizadaModel> NextHandler = _ => Task.FromResult(expectedResponse);

            // Act
            var result = await _behavior.Handle(request, NextHandler, CancellationToken.None);

            // Assert
            result.Should().Be(expectedResponse);
            _cacheService.Received(1).Pegar<RespostaPadronizadaModel>($"Pedido-{pedidoId}");
        }

        [Fact]
        public async Task Handle_QuandoRespostaEhSucesso_DeveInserirNoCache()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 100);
            var request = new PegarPeloPedidoIdQuery(pedidoId);
            var response = new PedidoTributadoModel(_faker.Random.Int(1, 10), pedidoId, _faker.Random.Int(1, 10), _faker.Random.Decimal(1, 10), PedidoItemModelBuilder.CriarItensValidos(), EPedidoTributadoStatus.Criado);
            var expectedResponse = RespostaPadronizadaModel.ComSucesso(response);

            _cacheService.Pegar<RespostaPadronizadaModel>($"Pedido-{pedidoId}")
                .Returns((RespostaPadronizadaModel?)null);

            RequestHandlerDelegate<RespostaPadronizadaModel> NextHandler = _ => Task.FromResult(expectedResponse);

            // Act
            await _behavior.Handle(request, NextHandler, CancellationToken.None);

            // Assert
            _cacheService.Received(1).Inserir($"Pedido-{pedidoId}", Arg.Is<RespostaPadronizadaModel>(r => r.Sucesso && r.Data is PedidoTributadoModel), TimeSpan.FromMinutes(10));
        }

        [Fact]
        public async Task Handle_QuandoRespostaEhFalha_NaoDeveInserirNoCache()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 100);
            var request = new PegarPeloPedidoIdQuery(pedidoId);
            var expectedResponse = RespostaPadronizadaModel.ComErros("Error");

            _cacheService.Pegar<RespostaPadronizadaModel>($"Pedido-{pedidoId}")
                .Returns((RespostaPadronizadaModel?)null);

            RequestHandlerDelegate<RespostaPadronizadaModel> NextHandler = _ => Task.FromResult(expectedResponse);

            // Act
            await _behavior.Handle(request, NextHandler, CancellationToken.None);

            // Assert
            _cacheService.DidNotReceiveWithAnyArgs().Inserir(Arg.Any<string>(), Arg.Any<RespostaPadronizadaModel>(), Arg.Any<TimeSpan>());
        }
    }
}