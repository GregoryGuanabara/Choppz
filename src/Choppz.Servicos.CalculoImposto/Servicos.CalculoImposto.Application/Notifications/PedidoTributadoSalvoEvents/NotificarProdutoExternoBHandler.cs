using MediatR;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Abstractions.UnitOfWork;
using Servicos.CalculoImposto.Core.Entities.OutboxMessage;
using Servicos.CalculoImposto.Core.Events;
using System.Text.Json;

namespace Servicos.CalculoImposto.Application.Notifications.PedidoTributadoSalvoEvents
{
    public sealed class NotificarProdutoExternoBHandler : INotificationHandler<PedidoTributadoSalvoEvent>
    {
        private readonly IOutboxMessageRepository _outboxMessageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NotificarProdutoExternoBHandler(IOutboxMessageRepository outboxMessageRepository, IUnitOfWork unitOfWork)
        {
            _outboxMessageRepository = outboxMessageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(PedidoTributadoSalvoEvent notification, CancellationToken cancellationToken)
        {
            await _outboxMessageRepository.InserirAsync(new OutboxMessage(nameof(PedidoTributadoSalvoEvent), JsonSerializer.Serialize(notification)));
            await _unitOfWork.SaveChangesAsync();
        }
    }
}