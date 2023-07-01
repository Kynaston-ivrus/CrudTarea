using Domain.Places;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

internal sealed class PlaceRepository : IPlaceRepository
{
    private readonly ApplicationDbContext _context;

    public PlaceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Place?> GetByIdAsync(PlaceId id) => await _context.Places.SingleOrDefaultAsync(p => p.Id == id);
    public async Task<List<Place>> GetAll() => await _context.Places.ToListAsync();
    public async Task<bool> ExistsAsync(PlaceId id) => await _context.Places.AnyAsync(place => place.Id == id);
    public async Task<Place?> GetById(PlaceId id) => await _context.Places.SingleOrDefaultAsync(p => p.Id == id);
    public void Delete(Place place) => _context.Places.Remove(place);


    public void Add(Place place)
    {
        _context.Places.Add(place);
    }
    public void Update(Place place)
    {
        _context.Places.Update(place);
    }
    public void Remove(Place place)
    {
        _context.Places.Remove(place);
    }
}