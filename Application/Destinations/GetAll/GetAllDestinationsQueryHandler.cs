using Application.Places.Common;
using Domain.Places;
using ErrorOr;
using MediatR;

namespace Application.Places.GetAll;


internal sealed class GetAllPlacesQueryHandler : IRequestHandler<GetAllPlacesQuery, ErrorOr<IReadOnlyList<PlaceResponse>>>
{
    private readonly IPlaceRepository _placeRepository;

    public GetAllPlacesQueryHandler(IPlaceRepository placeRepository)
    {
        _placeRepository = placeRepository ?? throw new ArgumentNullException(nameof(placeRepository));
    }

    public async Task<ErrorOr<IReadOnlyList<PlaceResponse>>> Handle(GetAllPlacesQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyList<Place> places = await _placeRepository.GetAll();

        return places.Select(place => new PlaceResponse(
                place.Id.Value,
                place.Name,
                place.Description,
                place.Ubication
            )).ToList();
    }
}