using Application.Reservations.Common;
using Domain.Reservations;
using Domain.Primitives;
using Domain.Places;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Reservations.Create;
public sealed class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, ErrorOr<Unit>>
{

    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitofwork;
    public CreateReservationCommandHandler(IReservationRepository reservationRepository, IUnitOfWork unitofwork)
    {

        _reservationRepository = reservationRepository;
        _unitofwork = unitofwork;
    }


    public async Task<ErrorOr<Unit>> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
    {
        if (PhoneNumber.Create(command.PhoneNumber) is not PhoneNumber phoneNumber)
        {
            return Error.Validation("Customer.PhoneNumber", " is not a valid phone number");
        }

        var reservation = Reservation.Create(command.Name, command.Email, phoneNumber, command.PackageId, command.Traveldate);

        _reservationRepository.Add(reservation);

        await _unitofwork.SaveChangesAsync(cancellationToken);

        // return new ReservationResponse();
        return Unit.Value;
    }
}