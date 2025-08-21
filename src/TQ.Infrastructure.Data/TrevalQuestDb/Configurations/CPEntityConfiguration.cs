using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TQ.Core.Aggregates.CPsAggregate;
using TQ.Core.Aggregates.CPsAggregate.ValueObjects;

namespace TQ.Infrastructure.Data.TravelQuestDb.Configurations
{
    public class CPEntityConfiguration : IEntityTypeConfiguration<CP>
    {
        public void Configure(EntityTypeBuilder<CP> builder)
        {
            builder.HasKey(x => x.Id);

            // Конфигурация RunId (Primary Key)
            builder
                .Property(p => p.Id)
                .HasConversion(
                    id => id.Value,
                    dbId => new CPId(dbId))
                .IsRequired()
                .HasMaxLength(36)
                .ValueGeneratedNever();

            builder
                .Property(p => p.Id)
                .Metadata.SetValueComparer(
                    new ValueComparer<CPId>(
                        (l, r) => l != null && r != null && l.Value == r.Value,
                        id => id.Value.GetHashCode(),
                        id => new CPId(id.Value)));

            // Конфигурация свойств
            builder.Property(p => p.Number);

            builder.Property(p => p.Legend)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Id)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(p => p.Latitude)
                .IsRequired();

            builder.Property(p => p.Longitude)
                .IsRequired();
            
            // Примечание: в будущем можно рассмотреть использование типа Point (PostGIS) вместо двух полей
            // для поддержки геозапросов и карт
        }
    }
}
