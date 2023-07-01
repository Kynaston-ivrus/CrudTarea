using Application.Places.Common;
using Domain.Places;
using ErrorOr;
using MediatR;

namespace Application.Places.GetById;


internal sealed class GetPlaceByIdQueryHandler : IRequestHandler<GetPlaceByIdQuery, ErrorOr<PlaceResponse>>
{
    private readonly IPlaceRepository _placeRepository;

    public GetPlaceByIdQueryHandler(IPlaceRepository placeRepository)
    {
        _placeRepository = placeRepository ?? throw new ArgumentNullException(nameof(placeRepository));
    }

    public async Task<ErrorOr<PlaceResponse>> Handle(GetPlaceByIdQuery query, CancellationToken cancellationToken)
    {
        if (await _placeRepository.GetByIdAsync(new PlaceId(query.Id)) is not Place Place)
        {
            return Error.NotFound("Place.NotFound", "The Place with the provide Id was not found.");
        }

        return new PlaceResponse(
            Place.Id.Value,
            Place.Name,
            Place.Description,
            Place.Ubication
            );
    }
}