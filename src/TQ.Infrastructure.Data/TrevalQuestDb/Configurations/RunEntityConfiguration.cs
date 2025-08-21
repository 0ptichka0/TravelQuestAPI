using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TQ.Core.Aggregates.RunsAggregate;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;

namespace TQ.Infrastructure.Data.TravelQuestDb.Configurations
{
    public class RunEntityConfiguration : IEntityTypeConfiguration<Run>
    {
        public void Configure(EntityTypeBuilder<Run> builder)
        {
            builder.HasKey(x => x.Id);

            // Конфигурация RunId (Primary Key)
            builder
                .Property(p => p.Id)
                .HasConversion(
                    id => id.Value,
                    dbId => new RunId(dbId))
                .IsRequired()
                .HasMaxLength(36)
                .ValueGeneratedNever();

            builder
                .Property(p => p.Id)
                .Metadata.SetValueComparer(
                    new ValueComparer<RunId>(
                        (l, r) => l != null && r != null && l.Value == r.Value,
                        id => id.Value.GetHashCode(),
                        id => new RunId(id.Value)));
            
            // Конфигурация свойств
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.RunStart)
                .IsRequired();

            builder.Property(p => p.Duration)
                .IsRequired()
                .HasConversion(
                    t => t.Ticks,
                    ticks => TimeSpan.FromTicks(ticks)
                );

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            // Индекс для даты старта забега
            builder.HasIndex(p => p.RunStart);
        }
    }
}
