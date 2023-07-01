namespace Domain.Places;

public interface IPlaceRepository
{
    Task<Place?> GetByIdAsync(PlaceId id);
    Task<bool> ExistsAsync(PlaceId id);
    Task<List<Place>> GetAll();

    void Add(Place place);

    void Update(Place place);

    void Remove(Place place);
    void Delete(Place place);
}