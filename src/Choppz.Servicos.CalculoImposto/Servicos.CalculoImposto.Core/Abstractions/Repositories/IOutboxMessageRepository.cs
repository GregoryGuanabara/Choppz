using Servicos.CalculoImposto.Core.Entities.OutboxMessage;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Core.Abstractions.Repositories
{
    public interface IOutboxMessageRepository
    {
        Task InserirAsync(OutboxMessage outboxMessage);

        Task<List<OutboxMessage>> PegarTodosAsync(EOutboxMessageStatus status);
    }
}