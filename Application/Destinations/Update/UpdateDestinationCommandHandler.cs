
using Domain.Places;
using Domain.Primitives;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Places.Update;

internal sealed class UpdatePlaceCommandHandler : IRequestHandler<UpdatePlaceCommand, ErrorOr<Unit>>
{
    private readonly IPlaceRepository _placeRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdatePlaceCommandHandler(IPlaceRepository placeRepository, IUnitOfWork unitOfWork)
    {
        _placeRepository = placeRepository ?? throw new ArgumentNullException(nameof(placeRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<ErrorOr<Unit>> Handle(UpdatePlaceCommand command, CancellationToken cancellationToken)
    {
        if (!await _placeRepository.ExistsAsync(new PlaceId(command.Id)))
        {
            return Error.NotFound("Customer.NotFound", "The customer with the provide Id was not found.");
        }

        Place place = Place.UpdatePlace(command.Id, command.Name, command.Description, command.Ubication);

        _placeRepository.Update(place);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}