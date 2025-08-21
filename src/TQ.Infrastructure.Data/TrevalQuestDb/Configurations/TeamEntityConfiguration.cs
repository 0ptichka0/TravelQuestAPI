using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TQ.Core.Aggregates.RunsAggregate;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Aggregates.TeamsAggregate;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Infrastructure.Data.TravelQuestDb.Configurations
{
    public class TeamEntityConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
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

            // Конфигурация свойств
            builder.Property(p => p.RegistrationDate)
                .IsRequired();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Code)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(p => p.Area)
                .HasMaxLength(50);

            builder.Property(p => p.Group)
                .HasMaxLength(50);

            // Индекс на пару (RunId, Code) — уникальный
            builder
                .HasIndex(u => new { u.RunId, u.Code })
                .IsUnique();

            builder
                .HasOne<Run>()
                .WithMany()
                .HasForeignKey(u => u.RunId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
