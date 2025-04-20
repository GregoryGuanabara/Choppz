using Microsoft.EntityFrameworkCore;
using Servicos.CalculoImposto.Core.Abstractions.UnitOfWork;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;
using Servicos.CalculoImposto.Infra.Abstractions.EventDispatcher;

namespace Servicos.CalculoImposto.Infra.Persistence.UnitOfWork
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventDispatcher _eventDispatcher;

        public UnitOfWork(ApplicationDbContext context,
                          IEventDispatcher eventDispatcher)
        {
            _context = context;
            _eventDispatcher = eventDispatcher;
        }

        public async Task SaveChangesAsync()
        {
            var entidadesAdicionadas = _context.ChangeTracker.Entries<PedidoTributado>()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity)
                .ToList();

            await _context.SaveChangesAsync();

            var eventos = entidadesAdicionadas.SelectMany(e => e.Events).ToList();

            foreach (var domainEvent in eventos)
                await _eventDispatcher.PublishAsync(domainEvent);
        }
    }
}