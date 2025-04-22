using Microsoft.EntityFrameworkCore;
using Servicos.CalculoImposto.Core.Entities.OutboxMessage;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;
using Servicos.CalculoImposto.Infra.Abstractions;
using Servicos.CalculoImposto.Infra.Persistence.EntitiesConfig;

namespace Servicos.CalculoImposto.Infra.Persistence
{
    public sealed class ApplicationDbContext : DbContext
    {
        public DbSet<PedidoTributado> PedidosTributados { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PedidoTributadoConfig());
            modelBuilder.ApplyConfiguration(new OutboxMessageConfig());
        }
    }
}