using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyRemittance.BuildingBlocks.Application.Outbox;

namespace MoneyRemittance.Infrastructure.Configuration.Mapping.OutboxMessages;

internal class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasConversion(x => x.ToString(), y => Guid.Parse(y));
        builder.Property(x => x.Data).HasColumnName("data");
        builder.Property(x => x.OccurredOn).HasColumnName("occurredOn");
        builder.Property(x => x.ProcessedDate).HasColumnName("processedDate");
        builder.Property(x => x.Type).HasColumnName("type");
        builder.Property(x => x.Error).HasColumnName("error");

        builder.HasNoDiscriminator();

        builder.ToTable("outboxMessages");
    }
}
