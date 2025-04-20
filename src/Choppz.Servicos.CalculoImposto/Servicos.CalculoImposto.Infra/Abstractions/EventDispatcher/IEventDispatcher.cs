using Servicos.CalculoImposto.Core.BaseEntities;

namespace Servicos.CalculoImposto.Infra.Abstractions.EventDispatcher
{
    public interface IEventDispatcher
    {
        Task PublishAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
    }
}