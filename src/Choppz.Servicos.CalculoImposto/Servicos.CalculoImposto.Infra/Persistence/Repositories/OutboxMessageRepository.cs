using Microsoft.EntityFrameworkCore;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Entities.OutboxMessage;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Infra.Persistence.Repositories
{
    public sealed class OutboxMessageRepository : IOutboxMessageRepository
    {
        private readonly DbSet<OutboxMessage> _outboxMessages;

        public OutboxMessageRepository(ApplicationDbContext context)
        {
            _outboxMessages = context.Set<OutboxMessage>();
        }

        public async Task<List<OutboxMessage>> PegarTodosAsync(EOutboxMessageStatus status)
        {
            return await _outboxMessages.Where(p => p.Status == status)
                                        .OrderBy(p => p.CriadoEm)
                                        .ToListAsync();
        }

        public async Task InserirAsync(OutboxMessage outboxMessage)
        {
            await _outboxMessages.AddAsync(outboxMessage);
        }
    }
}