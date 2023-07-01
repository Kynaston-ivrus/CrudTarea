using Domain.Places;
namespace Domain.Packages;

public sealed class LineItem
{
    internal LineItem(LineItemId id, PackageId packageId, PlaceId placeId)
    {
        Id = id;
        PackageId = packageId;
        PlaceId = placeId;
    }

    private LineItem()
    {

    }

    public LineItemId Id { get; private set; }
    public PackageId PackageId { get; private set; }
    public PlaceId PlaceId { get; private set; }
    public Guid PlaceIdValue => PlaceId.Value;

    public void Update(PlaceId placeId)
    {
        PlaceId = placeId;
    }
}