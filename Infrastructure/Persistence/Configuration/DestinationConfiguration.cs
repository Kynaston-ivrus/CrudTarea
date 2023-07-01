using Domain.Places;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class PlaceConfiguration : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasConversion(
            placeId => placeId.Value,
            value => new PlaceId(value));

        builder.Property(c => c.Name).HasMaxLength(30);
        builder.Property(c => c.Description).HasMaxLength(50);
        builder.Property(c => c.Ubication).HasMaxLength(50);
    }
}