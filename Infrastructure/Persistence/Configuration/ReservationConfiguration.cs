using Domain.Packages;
using Domain.Reservations;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configuration;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{

    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasConversion(
            ReservationId => ReservationId.Value,
            value => new ReservationId(value)
        );

        builder.Property(c => c.Name).HasMaxLength(30);
        builder.Property(c => c.Email).HasMaxLength(200);
        builder.HasIndex(c => c.Email).IsUnique();

        builder.Property(c => c.PhoneNumber).HasConversion(
           phoneNumber => phoneNumber.Value,
           value => PhoneNumber.Create(value)!)
           .HasMaxLength(9);

        builder.HasOne<Package>()
        .WithMany()
        .HasForeignKey(o => o.PackageId);

        // builder.Property(li => li.TravelDate);
    }
}