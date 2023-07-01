using Application.Reservations.Create;
using MediatR;
using Application.Reservations.GetAll;

using Microsoft.AspNetCore.Mvc;
using ErrorOr;

namespace Web.Api.Controllers;

[Route("Reservation")]
public class Reservations : ApiController
{
    private readonly ISender _mediator;

    public Reservations(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var reservationsResult = await _mediator.Send(new GetAllReservationsQuery());

        return reservationsResult.Match(
            Reservation => Ok(reservationsResult.Value),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReservationCommand command)
    {
        var createReservationResult = await _mediator.Send(command);

        return createReservationResult.Match(
            Reservation => Ok(createReservationResult.Value),
            errors => Problem(errors)
        );
    }
}