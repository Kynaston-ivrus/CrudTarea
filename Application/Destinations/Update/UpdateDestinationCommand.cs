using ErrorOr;
using MediatR;

namespace Application.Places.Update;

public record UpdatePlaceCommand(
    Guid Id,
    string Name,
    string Description,
    string Ubication
    ) : IRequest<ErrorOr<Unit>>;