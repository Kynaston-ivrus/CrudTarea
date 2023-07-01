using Application.Places.Common;
using ErrorOr;
using MediatR;

namespace Application.Places.GetById;

public record GetPlaceByIdQuery(Guid Id) : IRequest<ErrorOr<PlaceResponse>>;