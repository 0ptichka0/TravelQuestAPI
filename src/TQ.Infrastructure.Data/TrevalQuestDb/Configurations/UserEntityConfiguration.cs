using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TQ.Core.Aggregates.TeamsAggregate;
using TQ.Core.Aggregates.UsersAggregate;
using TQ.SharedKernel.ValueObjects;

namespace TQ.Infrastructure.Data.TravelQuestDb.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
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
            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.Code);

            builder
                .HasOne<Team>()
                .WithMany()
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
