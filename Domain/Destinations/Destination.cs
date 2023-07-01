using Domain.ValueObjects;

namespace Domain.Places;

public sealed class Place
{
    public Place(PlaceId id, string name, string description, string ubication)
    {
        Id = id;
        Name = name;
        Description = description;
        Ubication = ubication;
    }

    private Place()
    {

    }

    public PlaceId Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Ubication { get; private set; } = string.Empty;


    public void Update(string name, string description, string ubication)
    {
        Name = name;
        Description = description;
        Ubication = ubication;
    }

    public static Place UpdatePlace(Guid id, string name, string description, string ubication)
    {
        return new Place(new PlaceId(id), name, description, ubication);
    }
}