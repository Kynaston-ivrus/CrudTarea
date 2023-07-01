using MediatR;
using ErrorOr;
using Domain.Packages;
using Application.Reservations.Common;
using Domain.ValueObjects;

namespace Application.Reservations.Common;

public record ReservationResponse(
    Guid CodigoReserva,
    string Name,
    string Email,
    string PhoneNumber,
    DateTime TravelDate,
    DateTime FechaReserva,
    // PackageId PackageId,
    PackageResponse paqueteTuristico
) : IRequest<ErrorOr<ReservationResponse>>;

public record PackageResponse(
    string Nombre,
    List<LineItemResponse> Destinos  // Propiedad para los LineItems
    );

public record LineItemResponse(
    string Nombre,
    string Ubicacion);

