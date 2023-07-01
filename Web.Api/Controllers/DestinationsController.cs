using Application.Places;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Places.GetAll;

using Web.Api.Controllers;
using Application.Places.GetById;
using Application.Places.Update;
using Application.Places.Delete;
using ErrorOr;

[Route("places")]
public class Places : ApiController
{
    private readonly ISender _mediator;

    public Places(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var placesResult = await _mediator.Send(new GetAllPlacesQuery());

        return placesResult.Match(
            Place => Ok(placesResult.Value),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var placeResult = await _mediator.Send(new GetPlaceByIdQuery(id));

        return placeResult.Match(
            place => Ok(place),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePlaceCommand command)
    {
        var createPlaceResult = await _mediator.Send(command);

        return createPlaceResult.Match(
            Place => Ok(createPlaceResult.Value),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePlaceCommand command)
    {
        if (command.Id != id)
        {
            List<Error> errors = new()
            {
                Error.Validation("Place.UpdateInvalid", "The request Id does not match with the url Id.")
            };
            return Problem(errors);
        }

        var updateResult = await _mediator.Send(command);

        return updateResult.Match(
            placeId => NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteResult = await _mediator.Send(new DeletePlaceCommand(id));

        return deleteResult.Match(
            placeId => NoContent(),
            errors => Problem(errors)
        );
    }
}