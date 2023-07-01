using Domain.Packages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configuration;

public class PackageConfiguration : IEntityTypeConfiguration<Package>
{

    public void Configure(EntityTypeBuilder<Package> builder)
    {

        builder.ToTable("Package");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasConversion(
            PackageId => PackageId.Value,
            value => new PackageId(value)
        );
        builder.Property(c => c.Name).HasMaxLength(30);
        builder.Property(c => c.Description).HasMaxLength(50);

        builder.OwnsOne(c => c.Price, priceBuilder => { priceBuilder.Property(m => m.Currency).HasMaxLength(3); });

        builder.HasMany(o => o.LineItems)
            .WithOne().HasForeignKey(li => li.PackageId);
    }
}