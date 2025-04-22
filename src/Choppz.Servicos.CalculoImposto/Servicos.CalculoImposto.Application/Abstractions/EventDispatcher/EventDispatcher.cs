using MediatR;
using Servicos.CalculoImposto.Core.BaseEntities;
using Servicos.CalculoImposto.Infra.Abstractions.EventDispatcher;

namespace Servicos.CalculoImposto.Application.Abstractions.EventDispacherService
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IMediator _mediator;

        public EventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            await _mediator.Publish(domainEvent);
        }
    }
}