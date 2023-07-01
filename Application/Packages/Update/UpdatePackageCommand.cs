using Domain.Places;
using Application.Packages.Common;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;

namespace Application.Packages.Update
{
    public record UpdatePackageCommand(
        Guid Id,
        string Name,
        string Description,
        DateTime TravelDate,
        Money Price,
        List<UpdateLineItemCommand> Items
    ) : IRequest<ErrorOr<Unit>>;

    public record UpdateLineItemCommand(Guid PlaceId);
}