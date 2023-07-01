using Application.Reservations.Common;
using Domain.ValueObjects;
using Domain.Reservations;
using Domain.Packages;
using ErrorOr;
using MediatR;

namespace Application.Reservations.Update;

public record UpdateReservationCommand(
    Guid Id,
    string Name,
    string Email,
    string PhoneNumber,
    PackageId PackageId,
    DateTime Traveldate
    ) : IRequest<ErrorOr<Unit>>;