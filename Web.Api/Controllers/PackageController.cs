using Application.Packages.Create;
using Application.Packages.Delete;
using Application.Packages.GetAll;
using Application.Packages.GetById;
using Application.Packages.Search;
using Application.Packages.Update;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers;

[Route("Package")]
public class Packages : ApiController
{
    private readonly ISender _mediator;

    public Packages(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var packagesResult = await _mediator.Send(new GetAllPackagesQuery());

        return packagesResult.Match(
            Package => Ok(packagesResult.Value),
            errors => Problem(errors)
        );
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePackageCommand command)
    {
        var createPackageResult = await _mediator.Send(command);

        return createPackageResult.Match(
            customer => Ok(),
            errors => Problem(errors)
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePackageCommand command)
    {
        if (command.Id != id)
        {
            List<Error> errors = new()
            {
                Error.Validation("Package.UpdateInvalid", "The request Id does not match with the url Id.")
            };
            return Problem(errors);
        }

        var updatePackageResult = await _mediator.Send(command);

        return updatePackageResult.Match(
            PackageId => NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var packageResult = await _mediator.Send(new GetPackageByIdQuery(id));

        return packageResult.Match(
            package => Ok(package),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deletePackageResult = await _mediator.Send(new DeletePackageCommand(id));

        return deletePackageResult.Match(
            ReservationId => NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpGet("search")]
    public async Task<IActionResult> Get(string? name, string? description, DateTime? travelDate, decimal? price, string? ubication)
    {
        var searchPackagesResult = await _mediator.Send(new SearchPackagesQuery(name, description, travelDate, price, ubication));

        return searchPackagesResult.Match(
            packages => Ok(packages),
            errors => Problem(errors)
        );
    }

}