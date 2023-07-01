using Application.Reservations.Common;
using Domain.Reservations;
using Domain.Places;

using ErrorOr;
using MediatR;

using Domain.Primitives;
using Domain.ValueObjects;
using System.Runtime.InteropServices;
using Application.Reservations.GetById;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Domain.Packages;

namespace Application.Reservations.GetAll;

internal sealed class GetReservationByIdQueryHandler : IRequestHandler<GetReservationByIdQuery, ErrorOr<ReservationResponse>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IPackageRepository _packageRepository;
    private readonly IPlaceRepository _placeRepository;

    public GetReservationByIdQueryHandler(IReservationRepository reservationRepository, IPackageRepository packageRepository, IPlaceRepository placeRepository)
    {
        _reservationRepository = reservationRepository;
        _packageRepository = packageRepository;
        _placeRepository = placeRepository;
    }

    public async Task<ErrorOr<ReservationResponse>> Handle(GetReservationByIdQuery query, CancellationToken cancellationToken)
    {
        if (await _reservationRepository.GetByIdAsync(new ReservationId(query.Id)) is not Reservation reservation)
        {
            return Error.NotFound("Reservation.NotFound", "The reservation with the provided Id was not found.");
        }

        var package = await _packageRepository.GetByIdWithLineItemAsync(reservation.PackageId);

        var lineItemResponses = new List<LineItemResponse>();

        foreach (var lineItem in package.LineItems)
        {
            var place = await _placeRepository.GetByIdAsync(lineItem.PlaceId);
            string Name = place != null ? place.Name : string.Empty;
            string Ubication = place != null ? place.Ubication : string.Empty;

            var lineItemResponse = new LineItemResponse(Name, Ubication);
            lineItemResponses.Add(lineItemResponse);
        }

        var response = new ReservationResponse(
            reservation.Id.Value,
            reservation.Name,
            reservation.Email,
            reservation.PhoneNumber.Value,
            package?.TravelDate ?? DateTime.Now,
            reservation.TravelDate,
            new PackageResponse(
                package?.Name ?? string.Empty,
                lineItemResponses
            )
        );

        return response;
    }
}
