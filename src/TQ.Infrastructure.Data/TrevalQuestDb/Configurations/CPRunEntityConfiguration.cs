using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TQ.Core.Aggregates.CPsAggregate;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;
using TQ.Core.Aggregates.CPsRunsAggregate;
using TQ.Core.Aggregates.RunsAggregate;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Infrastructure.Data.TravelQuestDb.Configurations
{
    public class CPRunEntityConfiguration : IEntityTypeConfiguration<CPRun>
    {
        public void Configure(EntityTypeBuilder<CPRun> builder)
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

            // Конфигурация RunId (Foreign Key)
            builder
                .Property(p => p.RunId)
                .HasConversion(
                    id => id != null ? id.Value : null,
                    dbId => dbId != null ? new RunId(dbId) : null)
                .HasMaxLength(36);

            builder
                .Property(p => p.RunId)
                .Metadata.SetValueComparer(
                    new ValueComparer<RunId>(
                        (l, r) => (l == null && r == null) || (l != null && r != null && l.Value == r.Value),
                        id => id != null ? id.Value.GetHashCode() : 0,
                        id => id != null ? new RunId(id.Value) : null));

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

            // Индекс на пару (RunId, CPId) — уникальный
            builder
                .HasIndex(u => new { u.RunId, u.CPId })
                .IsUnique();

            builder
                .HasOne<Run>()
                .WithMany()
                .HasForeignKey(u => u.RunId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne<CP>()
                .WithMany()
                .HasForeignKey(u => u.CPId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Конфигурация Scores
            builder.Property(p => p.Scores)
                .IsRequired()
                .HasDefaultValue(0);
        }
    }
}
