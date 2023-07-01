using Domain.Reservations;
using Domain.Packages;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class PackageRepository : IPackageRepository
{

    private readonly ApplicationDbContext _context;

    public PackageRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public void Delete(Package package) => _context.Packages.Remove(package);
    public void UpdatePackage(Package package)
    {
        _context.Packages.Update(package);
    }

    public void Update(Package package)
    {
        _context.Packages.Update(package);
    }

    public async Task<bool> ExistsAsync(PackageId id) => await _context.Packages.AnyAsync(package => package.Id == id);
    public void Add(Package package)
    {
        _context.Add(package);
    }

    // public async Task<Package?> GetByIdAsync(PackageId id) 
    // {
    //     await _context.Customers.SingleOrDefaultAsync(c => c.Id == id);
    // }

    public async Task<Package?> GetByIdWithLineItemAsync(PackageId id)
    {
        return await _context.Packages
            .Include(o => o.LineItems)
            .SingleOrDefaultAsync(o => o.Id == id);
    }


    public async Task<Package?> GetByIdAsync(PackageId id)
    {
        return await _context.Packages
            .Include(o => o.LineItems)
            .SingleOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Package>> Search(string name, string description, DateTime? travelDate, decimal? price, string ubication)
    {
        var query = _context.Packages.AsQueryable();

        if (!string.IsNullOrEmpty(name))
            query = query.Where(p => p.Name.Contains(name));

        if (!string.IsNullOrEmpty(description))
            query = query.Where(p => p.Description.Contains(description));

        if (travelDate.HasValue)
            query = query.Where(p => p.TravelDate.Date == travelDate.Value.Date);

        if (price.HasValue)
            query = query.Where(p => p.Price.Amount <= price.Value);

        if (!string.IsNullOrEmpty(ubication))
        {
            query = query.Where(p => p.LineItems.Any(li =>
                _context.Places.Any(d => d.Id == li.PlaceId && d.Ubication.Contains(ubication))));
        }

        return await query
            .Include(o => o.LineItems)
            .ToListAsync();
    }


    // public async Task<Package?> GetByIdAsync(PackageId id) => await _context.Packages.SingleOrDefaultAsync(c => c.Id == id);

    public async Task<List<Package>> GetAll()
    {
        return await _context.Packages
            .Include(o => o.LineItems)
            .ToListAsync();
    }

    // public async Task<List<Package>> GetAll() => await _context.Packages.ToListAsync();

    public Task<Reservation?> GetByIdWithLineItemAsync(ReservationId id)
    {
        throw new NotImplementedException();
    }

    public bool HasOneLineItem(Package package)
    {
        throw new NotImplementedException();
    }
}