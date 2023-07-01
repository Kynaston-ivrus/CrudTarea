namespace Domain.Packages;

public interface IPackageRepository
{

    Task<Package?> GetByIdWithLineItemAsync(PackageId id);
    Task<Package?> GetByIdAsync(PackageId id);
    bool HasOneLineItem(Package package);
    Task<List<Package>> GetAll();
    void Add(Package package);
    Task<bool> ExistsAsync(PackageId id);
    void UpdatePackage(Package package);
    void Update(Package package);
    void Delete(Package package);
    Task<List<Package>> Search(string name, string description, DateTime? travelDate, decimal? price, string ubication);
}