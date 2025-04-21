using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servicos.CalculoImposto.Core.Entities.OutboxMessage;

namespace Servicos.CalculoImposto.Infra.Persistence.EntitiesConfig
{
    internal sealed class OutboxMessageConfig : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.TipoDoEvento)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Payload)
                   .IsRequired();
        }
    }
}
