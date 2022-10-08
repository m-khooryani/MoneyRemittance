using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyRemittance.BuildingBlocks.Application.AggregateHistory;

namespace DArch.Samples.AppointmentService.Infrastructure.Configuration.DArchMapping;

internal class AggregateHistoryReferenceConfiguration : IEntityTypeConfiguration<AggregateRootHistoryItem>
{
    public void Configure(EntityTypeBuilder<AggregateRootHistoryItem> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.AggregateId, x.Version })
            .IsUnique();

        builder.Property(x => x.Id)
            .HasColumnName("Id");

        builder.Property(x => x.AggregateId)
            .HasColumnName("AggregateId");

        builder.Property(x => x.Version)
            .HasColumnName("Version")
            .IsConcurrencyToken();

        builder.Property(x => x.EventType)
            .HasColumnName("EventType");

        builder.Property(x => x.Type)
            .HasColumnName("Type");

        builder.Property(x => x.Datetime)
            .HasColumnName("Datetime");

        builder.Property(x => x.Data)
            .HasColumnName("Data");

        builder.ToTable("AggregateRootsHistory");
    }
}
