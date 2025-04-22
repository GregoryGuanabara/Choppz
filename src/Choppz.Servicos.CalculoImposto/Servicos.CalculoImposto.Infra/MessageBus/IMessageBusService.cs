using Servicos.CalculoImposto.Core.Entities.OutboxMessage;

namespace Servicos.CalculoImposto.Infra.MessageBus
{
    internal interface IMessageBusService
    {
        Task<bool> PublishAsync(OutboxMessage message);
    }
}