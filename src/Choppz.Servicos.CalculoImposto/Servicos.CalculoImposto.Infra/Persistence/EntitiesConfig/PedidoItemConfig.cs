using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;

namespace Servicos.CalculoImposto.Infra.Persistence.EntitiesConfig
{
    internal sealed class PedidoItemConfig : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(i => i.ProdutoId)
                   .IsRequired();

            builder.Property(i => i.Quantidade)
                        .IsRequired();

            builder.Property(i => i.Valor)
                    .IsRequired()
                    .HasPrecision(18, 2);
        }
    }
}