using Application.Reservations.Common;
using Domain.Reservations;
using Domain.Packages;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Reservations.Create;

public record CreateReservationCommand(
    string Name,
    string Email,
    string PhoneNumber,
    PackageId PackageId,
    DateTime Traveldate
    ) : IRequest<ErrorOr<Unit>>;