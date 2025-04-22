using Servicos.CalculoImposto.Core.Entities.OutboxMessage;

namespace Servicos.CalculoImposto.Infra.MessageBus
{
    public interface IMessageBusService
    {
        Task<bool> PublishAsync(OutboxMessage message);
    }
}