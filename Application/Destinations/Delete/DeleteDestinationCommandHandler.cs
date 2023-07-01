
using Domain.Places;
using Domain.Primitives;
using ErrorOr;
using MediatR;

namespace Application.Places.Delete;

internal sealed class DeletePlaceCommandHandler : IRequestHandler<DeletePlaceCommand, ErrorOr<Unit>>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeletePlaceCommandHandler(IPlaceRepository placeRepository, IUnitOfWork unitOfWork)
    {
        _placeRepository = placeRepository ?? throw new ArgumentNullException(nameof(placeRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<ErrorOr<Unit>> Handle(DeletePlaceCommand command, CancellationToken cancellationToken)
    {
        if (await _placeRepository.GetByIdAsync(new PlaceId(command.Id)) is not Place place)
        {
            return Error.NotFound("Place.NotFound", "The place with the provide Id was not found.");
        }

        _placeRepository.Remove(place);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}