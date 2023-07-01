using MediatR;
using ErrorOr;

namespace Application.Places.Common;


public record PlaceResponse(Guid Id,
    string name,
    string description,
    string ubication);

