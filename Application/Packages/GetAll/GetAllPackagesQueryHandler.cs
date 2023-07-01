using Application.Packages.Common;
using Domain.Places;
using ErrorOr;
using MediatR;

using Domain.Primitives;
using Domain.ValueObjects;
using System.Runtime.InteropServices;
using Domain.Packages;

namespace Application.Packages.GetAll;


internal sealed class GetAllPackagesQueryHandler : IRequestHandler<GetAllPackagesQuery, ErrorOr<IReadOnlyList<packageResponse>>>
{
    private readonly IPackageRepository _packageRepository;
    private readonly IPlaceRepository _placeRepository;
    private readonly IUnitOfWork _unitofwork;

    public GetAllPackagesQueryHandler(IPackageRepository packageRepository, IPlaceRepository placeRepository, IUnitOfWork unitofwork)
    {
        _packageRepository = packageRepository;
        _placeRepository = placeRepository;
        _unitofwork = unitofwork;
    }

    public async Task<ErrorOr<IReadOnlyList<packageResponse>>> Handle(GetAllPackagesQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyList<Package> packages = await _packageRepository.GetAll();

        var responses = new List<packageResponse>();

        foreach (var package in packages)
        {
            var lineItemResponses = new List<LineItemResponse>();

            foreach (var lineItem in package.LineItems)
            {
                var place = await _placeRepository.GetByIdAsync(lineItem.PlaceId);
                string Name = place != null ? place.Name : string.Empty;
                string Ubication = place != null ? place.Ubication : string.Empty;

                var lineItemResponse = new LineItemResponse(Name, Ubication);
                lineItemResponses.Add(lineItemResponse);
            }

            var PackageResponse = new packageResponse(
                package.Id.Value,
                package.Name,
                package.Description,
                new MoneyResponse(
                    package.Price.Currency,
                    package.Price.Amount),
                package.TravelDate,
                lineItemResponses);

            responses.Add(PackageResponse);
        }

        return responses;
    }
}