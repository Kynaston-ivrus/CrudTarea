using ErrorOr;
using MediatR;
using Application.Places.Common;

namespace Application.Places.GetAll;

public record GetAllPlacesQuery() : IRequest<ErrorOr<IReadOnlyList<PlaceResponse>>>;