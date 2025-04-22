using Bogus;
using FluentAssertions;
using NSubstitute;
using Servicos.CalculoImposto.Application.Notifications.PedidoTributadoSalvoEvents;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Abstractions.UnitOfWork;
using Servicos.CalculoImposto.Core.Entities.OutboxMessage;
using Servicos.CalculoImposto.Core.Enums;
using Servicos.CalculoImposto.Core.Events;
using Servicos.CalculoImposto.UnitTests.Servicos.CalculoImposto.Core.Tests.Builders;
using System.Text.Json;

namespace Servicos.CalculoImposto.Application.Tests.Notifications.PedidoTributadoSalvoEvents
{
    public class NotificarProdutoExternoBHandlerTests
    {
        private readonly NotificarProdutoExternoBHandler _handler;
        private readonly IOutboxMessageRepository _outboxMessageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Faker _faker;

        public NotificarProdutoExternoBHandlerTests()
        {
            _outboxMessageRepository = Substitute.For<IOutboxMessageRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _handler = new NotificarProdutoExternoBHandler(_outboxMessageRepository, _unitOfWork);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_DeveInserirMensagemOutboxCorretamente()
        {
            // Arrange
            var notification = new PedidoTributadoSalvoEvent(_faker.Random.Int(1, 100), _faker.Random.Int(1, 100), _faker.Random.Int(1, 100), _faker.Random.Decimal(1, 100), PedidoItemDTOBuilders.CriarItensValidos(), EPedidoTributadoStatus.Criado);

            OutboxMessage? mensagemInserida = null;
            _outboxMessageRepository.When(repo => repo.InserirAsync(Arg.Any<OutboxMessage>()))
                .Do(x => mensagemInserida = x.Arg<OutboxMessage>());

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            mensagemInserida.Should().NotBeNull();
            mensagemInserida!.TipoDoEvento.Should().Be(nameof(PedidoTributadoSalvoEvent));

            var payload = JsonSerializer.Deserialize<PedidoTributadoSalvoEvent>(mensagemInserida.Payload);
            payload.Should().NotBeNull();
            payload!.PedidoId.Should().Be(notification.PedidoId);
            payload.ClienteId.Should().Be(notification.ClienteId);
        }

        [Fact]
        public async Task Handle_DeveChamarSaveChangesAsync()
        {
            // Arrange
            var notification = new PedidoTributadoSalvoEvent(1, 1, 1, 10m, PedidoItemDTOBuilders.CriarItensValidos(), EPedidoTributadoStatus.Criado);

            // Act
            await _handler.Handle(notification, CancellationToken.None);

            // Assert
            await _unitOfWork.Received(1).SaveChangesAsync();
        }
    }
}