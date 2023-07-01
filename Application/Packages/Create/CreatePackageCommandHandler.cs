using Application.Packages.Common;
using Domain.Primitives;
using Domain.Places;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;
using Domain.Packages;
using Domain.Reservations;
using System.Runtime.InteropServices;

namespace Application.Packages.Create;
public sealed class CreatePackageCommandHandler : IRequestHandler<CreatePackageCommand, ErrorOr<Unit>>
{
    private readonly IPackageRepository _packageRepository;
    private readonly IPlaceRepository _placeRepository;
    private readonly IUnitOfWork _unitofwork;
    public CreatePackageCommandHandler(IPackageRepository packageRepository, IPlaceRepository placeRepository, IUnitOfWork unitofwork)
    {
        _packageRepository = packageRepository;
        _placeRepository = placeRepository;
        _unitofwork = unitofwork;
    }


    public async Task<ErrorOr<Unit>> Handle(CreatePackageCommand command, CancellationToken cancellationToken)
    {

        //var name = new Package(new PackageId(Guid.NewGuid()), command.Name);
        var package = Package.Create(
            command.Name,
            command.Description,
            command.Traveldate,
            command.Price);

        if (!command.Items.Any())
        {
            return Error.Conflict("Package.Detail", "For create at reservation you need to specify the details of the reservation");
        }

        foreach (var item in command.Items)
        {
            package.Add(new PlaceId(item.PlaceId));
        }
        _packageRepository.Add(package);

        await _unitofwork.SaveChangesAsync(cancellationToken);

        // return new PackageResponse();
        return Unit.Value;
    }
}