using Application.Places.Common;
using Domain.Primitives;
using Domain.Places;
using ErrorOr;
using MediatR;
using Domain.ValueObjects;

namespace Application.Places;
internal class CreatePlaceCommandHandler : IRequestHandler<CreatePlaceCommand, ErrorOr<Unit>>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePlaceCommandHandler(IPlaceRepository placeRepository, IUnitOfWork unitOfWork)
    {
        _placeRepository = placeRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Unit>> Handle(CreatePlaceCommand command, CancellationToken cancellationToken)
    {
        var place = new Place(
            new PlaceId(Guid.NewGuid()), command.Name, command.Description, command.Ubication);

        _placeRepository.Add(place);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // return new PlaceResponse();
        return Unit.Value;
    }
}