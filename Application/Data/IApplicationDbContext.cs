// using Domain.Customers;
using Domain.Places;
using Domain.Reservations;
using Microsoft.EntityFrameworkCore;

namespace Application.Data;

public interface IApplicationDbContext
{
    // DbSet<Customer> Customers { get; set; }
    DbSet<Reservation> Reservations { get; set; }
    DbSet<Place> Places { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}