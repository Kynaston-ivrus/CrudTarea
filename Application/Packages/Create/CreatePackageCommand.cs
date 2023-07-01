using Application.Packages.Common;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Packages.Create;

public record CreatePackageCommand(
    string Name,
    string Description,
    DateTime Traveldate,
    Money Price,
    List<CreateLineItemCommand> Items
    ) : IRequest<ErrorOr<Unit>>;

public record CreateLineItemCommand(Guid PlaceId)
{

}
