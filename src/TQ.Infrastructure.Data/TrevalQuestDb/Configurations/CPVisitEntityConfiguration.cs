using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TQ.Core.Aggregates.CPsAggregate;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Aggregates.CPVisitsAggregate;
using TQ.Core.Aggregates.RunsAggregate;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Aggregates.TeamsAggregate;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Infrastructure.Data.TravelQuestDb.Configurations
{
    public class CPVisitEntityConfiguration : IEntityTypeConfiguration<CPVisit>
    {
        public void Configure(EntityTypeBuilder<CPVisit> builder)
        {
            builder.HasKey(x => x.Id);

            // Конфигурация IntId (Primary Key)
            builder
                .Property(p => p.Id)
                .HasConversion(
                    id => id.Value,
                    dbId => new IntId(dbId))
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder
                .Property(p => p.Id)
                .Metadata.SetValueComparer(
                    new ValueComparer<IntId>(
                        (l, r) => l != null && r != null && l.Value == r.Value,
                        id => id.Value.GetHashCode(),
                        id => new IntId(id.Value)));

            // Конфигурация TeamId (Foreign Key)
            builder
                .Property(p => p.TeamId)
                .HasConversion(
                    id => id.Value,
                    dbId => new IntId(dbId))
                .IsRequired();

            builder
                .Property(p => p.TeamId)
                .Metadata.SetValueComparer(
                    new ValueComparer<IntId>(
                        (l, r) => l != null && r != null && l.Value == r.Value,
                        id => id.Value.GetHashCode(),
                        id => new IntId(id.Value)));

            // Конфигурация CPId (Foreign Key)
            builder
                .Property(p => p.CPId)
                .HasConversion(
                    id => id != null ? id.Value : null,
                    dbId => dbId != null ? new CPId(dbId) : null)
                .HasMaxLength(36);

            builder
                .Property(p => p.CPId)
                .Metadata.SetValueComparer(
                    new ValueComparer<CPId>(
                        (l, r) => (l == null && r == null) || (l != null && r != null && l.Value == r.Value),
                        id => id != null ? id.Value.GetHashCode() : 0,
                        id => id != null ? new CPId(id.Value) : null));

            builder
                .HasOne<CP>()
                .WithMany()
                .HasForeignKey(u => u.CPId)
                .OnDelete(DeleteBehavior.Cascade);

            // Конфигурация свойств
            builder.Property(p => p.IsValid)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(p => p.VisitTime)
                .IsRequired()
                .HasConversion(
                    t => t.Ticks,
                    ticks => TimeSpan.FromTicks(ticks)
                );

            // Индексы для ускорения запросов
            builder.HasIndex(p => p.TeamId);
            builder.HasIndex(p => p.CPId);

            builder
                .HasOne<Team>()
                .WithMany()
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
