using Application.Packages.Create;
using Application.Packages.GetAll;
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
}