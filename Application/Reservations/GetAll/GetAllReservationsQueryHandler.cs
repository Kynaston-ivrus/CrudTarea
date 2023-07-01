using Application.Reservations.Common;
using Domain.Reservations;
using Domain.Places;
using ErrorOr;
using MediatR;

using Domain.Primitives;
using Domain.ValueObjects;
using System.Runtime.InteropServices;
using Domain.Packages;

namespace Application.Reservations.GetAll;

internal sealed class GetAllReservationsQueryHandler : IRequestHandler<GetAllReservationsQuery, ErrorOr<IReadOnlyList<ReservationResponse>>>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IPackageRepository _packageRepository;
    private readonly IPlaceRepository _placeRepository;

    public GetAllReservationsQueryHandler(IReservationRepository reservationRepository, IPackageRepository packageRepository, IPlaceRepository placeRepository)
    {
        _reservationRepository = reservationRepository;
        _packageRepository = packageRepository;
        _placeRepository = placeRepository;
    }

    public async Task<ErrorOr<IReadOnlyList<ReservationResponse>>> Handle(GetAllReservationsQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyList<Reservation> reservations = await _reservationRepository.GetAll();

        var responses = new List<ReservationResponse>();

        foreach (var reservation in reservations)
        {
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
                // reservation.PackageId,
                new PackageResponse(
                    package?.Name ?? string.Empty,
                    lineItemResponses // Agrega los LineItems a la respuesta
                    )
            );

            responses.Add(response);
        }

        return responses;
    }

}