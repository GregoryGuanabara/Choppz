using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;

namespace Servicos.CalculoImposto.Infra.Persistence.EntitiesConfig
{
    public sealed class PedidoTributadoConfig : IEntityTypeConfiguration<PedidoTributado>
    {
        public void Configure(EntityTypeBuilder<PedidoTributado> builder)
        {
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.Imposto)
                   .IsRequired()
                   .HasPrecision(18, 2);
        }
    }
}