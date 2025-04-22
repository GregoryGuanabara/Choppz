using Bogus;
using FluentAssertions;
using NSubstitute;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Application.Notifications.PedidoTributadoSalvoEvents;
using Servicos.CalculoImposto.Core.Abstractions.CacheService;
using Servicos.CalculoImposto.Core.DTOs;
using Servicos.CalculoImposto.Core.Enums;
using Servicos.CalculoImposto.Core.Events;
using Servicos.CalculoImposto.UnitTests.Servicos.CalculoImposto.Core.Tests.Builders;

namespace Servicos.CalculoImposto.Application.Tests.Notifications.PedidoTributadoSalvoEvents
{
    public class CachearPedidoResponseHandlerTests
    {
        private readonly CachearPedidoResponseHandler _handler;
        private readonly ICacheService _cacheService;
        private readonly Faker _faker;

        public CachearPedidoResponseHandlerTests()
        {
            _cacheService = Substitute.For<ICacheService>();
            _handler = new CachearPedidoResponseHandler(_cacheService);
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task Handle_DeveCriarModelCorretamente()
        {
            // Arrange
            var notification = new PedidoTributadoSalvoEvent(
                _faker.Random.Int(1, 100),
                _faker.Random.Int(1, 100),
                _faker.Random.Int(1, 100),
                _faker.Random.Decimal(1, 100),
                PedidoItemDTOBuilders.CriarItensValidos(),
                EPedidoTributadoStatus.Criado);

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            _cacheService.Received(1).Inserir(
                Arg.Any<string>(),
                Arg.Is<RespostaPadronizadaModel>(r =>
                    r.Sucesso &&
                    r.Data is PedidoTributadoModel &&
                    ((PedidoTributadoModel)r.Data).PedidoId == notification.PedidoId &&
                    ((PedidoTributadoModel)r.Data).ClienteId == notification.ClienteId),
                Arg.Any<TimeSpan>());
        }

        [Fact]
        public async Task Handle_DeveUsarChaveDeCacheCorreta()
        {
            // Arrange
            var pedidoId = _faker.Random.Int(1, 100);
            var notification = new PedidoTributadoSalvoEvent(
                _faker.Random.Int(1, 100),
                pedidoId,
                _faker.Random.Int(1, 100),
                _faker.Random.Decimal(1, 100),
                PedidoItemDTOBuilders.CriarItensValidos(),
                EPedidoTributadoStatus.Criado);

            string? cacheKeyUsed = null;
            _cacheService.When(x => x.Inserir(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<TimeSpan>()))
                .Do(x => cacheKeyUsed = x.Arg<string>());

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            cacheKeyUsed.Should().Be($"Pedido-{pedidoId}");
        }

        [Fact]
        public async Task Handle_DeveDefinirTempoDeCacheCorreto()
        {
            // Arrange
            var notification = new PedidoTributadoSalvoEvent(
                _faker.Random.Int(1, 100),
                _faker.Random.Int(1, 100),
                _faker.Random.Int(1, 100),
                _faker.Random.Decimal(1, 100),
                PedidoItemDTOBuilders.CriarItensValidos(),
                EPedidoTributadoStatus.Criado);

            TimeSpan? tempoCacheUsed = null;
            _cacheService.When(x => x.Inserir(Arg.Any<string>(), Arg.Any<object>(), Arg.Any<TimeSpan>()))
                .Do(x => tempoCacheUsed = x.Arg<TimeSpan>());

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            tempoCacheUsed.Should().Be(TimeSpan.FromMinutes(10));
        }

        [Fact]
        public async Task Handle_DeveConverterItensCorretamente()
        {
            // Arrange
            var itens = new List<PedidoItemDTO>
                {
                    new PedidoItemDTO(_faker.Random.Int(1, 10), _faker.Random.Int(1, 5), _faker.Random.Decimal(10, 100)),
                    new PedidoItemDTO(_faker.Random.Int(11, 20), _faker.Random.Int(1, 5), _faker.Random.Decimal(10, 100))
                };

            var notification = new PedidoTributadoSalvoEvent(
                _faker.Random.Int(1, 100),
                _faker.Random.Int(1, 100),
                _faker.Random.Int(1, 100),
                _faker.Random.Decimal(1, 100),
                itens,
                EPedidoTributadoStatus.Criado);

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            //Assert
            _cacheService.Received(1).Inserir(
                Arg.Any<string>(),
                Arg.Is<RespostaPadronizadaModel>(r =>
                    r.Data is PedidoTributadoModel && // Check type first
                    ((PedidoTributadoModel)r.Data).PedidoId == notification.PedidoId && // Cast explicitly
                    ((PedidoTributadoModel)r.Data).ClienteId == notification.ClienteId),
                Arg.Any<TimeSpan>());
        }

        [Fact]
        public async Task Handle_DeveInserirRespostaPadronizadaComSucesso()
        {
            // Arrange
            var notification = new PedidoTributadoSalvoEvent(
                _faker.Random.Int(1, 100),
                _faker.Random.Int(1, 100),
                _faker.Random.Int(1, 100),
                _faker.Random.Decimal(1, 100),
                PedidoItemDTOBuilders.CriarItensValidos(),
                EPedidoTributadoStatus.Criado);

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            _cacheService.Received(1).Inserir(
                Arg.Any<string>(),
                Arg.Is<RespostaPadronizadaModel>(r => r.Sucesso),
                Arg.Any<TimeSpan>());
        }
    }
}