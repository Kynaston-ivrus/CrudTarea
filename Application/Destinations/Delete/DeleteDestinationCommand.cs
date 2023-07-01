using ErrorOr;
using MediatR;

namespace Application.Places.Delete;

public record DeletePlaceCommand(Guid Id) : IRequest<ErrorOr<Unit>>;