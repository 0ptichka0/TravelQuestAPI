using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TQ.Core.Aggregates.RunsAggregate;
using TQ.Core.Aggregates.RunsAggregate.ValueObjects;
using TQ.Core.Aggregates.TeamResultsAggregate;
using TQ.Core.Aggregates.TeamsAggregate;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Infrastructure.Data.TravelQuestDb.Configurations
{
    public class TeamResultEntityConfiguration : IEntityTypeConfiguration<TeamResult>
    {
        public void Configure(EntityTypeBuilder<TeamResult> builder)
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

            // Конфигурация свойств
            builder.Property(p => p.TotalScore)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(p => p.ElapsedTime)
                .IsRequired()
                .HasConversion(
                    t => t.Ticks,
                    ticks => TimeSpan.FromTicks(ticks)
                );

            builder.Property(p => p.Penalty)
                .IsRequired()
                .HasDefaultValue(0);

            // Индексы для ускорения запросов
            builder.HasIndex(p => p.TeamId);

            builder
                .HasOne<Team>()
                .WithMany()
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
