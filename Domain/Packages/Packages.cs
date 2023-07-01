using Domain.Places;
using Domain.Primitives;
using Domain.ValueObjects;
using System.Collections.ObjectModel;

namespace Domain.Packages;

public sealed class Package : AgregateRoot
{
    public Package(PackageId id, string name, string description, DateTime traveldate, Money price)
    {
        Id = id;
        Name = name;
        Description = description;
        TravelDate = traveldate;
        Price = price;
    }
    // private readonly List<LineItem> _lineItems = new();
    private readonly List<LineItem> _lineItems = new List<LineItem>();
    private Package()
    {

    }

    public PackageId Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime TravelDate { get; private set; }
    public Money Price { get; private set; }
    // public IReadOnlyList<LineItem> LineItems => _lineItems.AsReadOnly();
    public IReadOnlyList<LineItem> LineItems => new ReadOnlyCollection<LineItem>(_lineItems);

    public static Package Create(string name, string description, DateTime traveldate, Money price)
    {
        var package = new Package
        {
            Id = new PackageId(Guid.NewGuid()),
            Name = name,
            Description = description,
            TravelDate = traveldate,
            Price = price
        };

        return package;
    }
    public void Update(string name, string description, DateTime traveldate, Money price)
    {
        Name = name;
        Description = description;
        TravelDate = traveldate;
        Price = price;
    }
    public void Add(PlaceId placeId)
    {
        var LineItem = new LineItem(new LineItemId(Guid.NewGuid()), Id, placeId);

        _lineItems.Add(LineItem);
    }
    public void Update(PlaceId placeId)
    {
        var LineItem = new LineItem(new LineItemId(Guid.NewGuid()), Id, placeId);

        _lineItems.Add(LineItem);
    }


    public static Package UpdatePackage(
        Guid id,
        string name,
        string description,
        DateTime traveldate,
        Money price)
    {
        return new Package(new PackageId(id), name, description, traveldate, price);
    }

    // public static Package UpdatePackage(Guid id, string name, string description, DateTime traveldate, Money price)
    // {
    //     // return new Package(new PackageId(id), name, description, traveldate, price);
    //     var package = new Package
    //     {
    //         Id = new PackageId(Guid.NewGuid()),
    //         Name = name,
    //         Description = description,
    //         TravelDate = traveldate,
    //         Price = price
    //     };

    //     return package;
    // }

    public void Get(PlaceId placeId)
    {
        var LineItem = new LineItem(new LineItemId(Guid.NewGuid()), Id, placeId);

        _lineItems.Add(LineItem);
    }

    public void UpdateLineItem(LineItemId lineItemId, PlaceId placeId)
    {
        var lineItem = _lineItems.FirstOrDefault(li => li.Id == lineItemId);
        if (lineItem != null)
        {
            lineItem.Update(placeId);
        }
    }

    public void RemoveLineItem(LineItemId lineItemId, IPackageRepository PackageRepository)
    {
        var lineItem = _lineItems.FirstOrDefault(li => li.Id == lineItemId);
        if (lineItem != null)
        {
            _lineItems.Remove(lineItem);
            PackageRepository.Update(this);
        }
    }

    public void RemoveLineItems(LineItemId lineItemId, IPackageRepository PackageRepository)
    {
        if (PackageRepository.HasOneLineItem(this))
        {
            return;
        }

        var lineItem = _lineItems.FirstOrDefault(li => li.Id == lineItemId);

        if (lineItem == null)
        {
            return;
        }

        _lineItems.Remove(lineItem);
    }
}