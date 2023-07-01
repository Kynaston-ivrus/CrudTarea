using Application.Packages.Common;
using Domain.Packages;
using Domain.Places;
using ErrorOr;
using MediatR;

using Domain.Primitives;
using Domain.ValueObjects;
using System.Runtime.InteropServices;
using Application.Packages.GetById;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Application.Packages.GetAll;


internal sealed class GetPackageByIdQueryHandler : IRequestHandler<GetPackageByIdQuery, ErrorOr<packageResponse>>
{
    private readonly IPackageRepository _packageRepository;
    private readonly IPlaceRepository _placeRepository;
    private readonly IUnitOfWork _unitofwork;

    public GetPackageByIdQueryHandler(IPackageRepository packageRepository, IPlaceRepository placeRepository, IUnitOfWork unitofwork)
    {
        _packageRepository = packageRepository;
        _placeRepository = placeRepository;
        _unitofwork = unitofwork;
    }

    public async Task<ErrorOr<packageResponse>> Handle(GetPackageByIdQuery query, CancellationToken cancellationToken)
    {
        if (await _packageRepository.GetByIdAsync(new PackageId(query.Id)) is not Package package)
        {
            return Error.NotFound("Customer.NotFound", "The customer with the provided Id was not found.");
        }

        var packageResponse = new packageResponse(
            package.Id.Value,
            package.Name,
            package.Description,
            new MoneyResponse(
                package.Price.Currency,
                package.Price.Amount),
            package.TravelDate,
            new List<LineItemResponse>()); // Crear una lista vacía de LineItemResponse

        foreach (var lineItem in package.LineItems)
        {
            var place = await _placeRepository.GetByIdAsync(lineItem.PlaceId);
            string name = place != null ? place.Name : string.Empty;
            string ubication = place != null ? place.Ubication : string.Empty;

            var lineItemResponse = new LineItemResponse(name, ubication);
            packageResponse.LineItems.Add(lineItemResponse); // Agregar el lineItemResponse a la lista de LineItems
        }

        return packageResponse;
    }



}