using MediatR;
using ErrorOr;
using Application.Places.Common;
using Domain.ValueObjects;

namespace Application.Places;

public record CreatePlaceCommand(
    string Name,
    string Description,
    string Ubication
) : IRequest<ErrorOr<Unit>>;