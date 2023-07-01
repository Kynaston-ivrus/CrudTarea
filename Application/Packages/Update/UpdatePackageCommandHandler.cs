using Application.Packages.Update;
using Domain.Reservations;
using Domain.Primitives;
using Domain.Places;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Packages;

namespace Application.Packages.Update
{
    public sealed class UpdatePackageCommandHandler : IRequestHandler<UpdatePackageCommand, ErrorOr<Unit>>
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IPlaceRepository _placeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePackageCommandHandler(
            IReservationRepository reservationRepository,
            IPackageRepository packageRepository,
            IPlaceRepository placeRepository,
            IUnitOfWork unitOfWork)
        {
            _reservationRepository = reservationRepository;
            _packageRepository = packageRepository;
            _placeRepository = placeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Unit>> Handle(UpdatePackageCommand command, CancellationToken cancellationToken)
        {
            if (!await _packageRepository.ExistsAsync(new PackageId(command.Id)))
            {
                return Error.NotFound("Customer.NotFound", "The customer with the provided Id was not found.");
            }

            var package = await _packageRepository.GetByIdAsync(new PackageId(command.Id));

            package.Update(
                command.Name,
                command.Description,
                command.TravelDate,
                command.Price);

            if (!command.Items.Any())
            {
                return Error.Conflict("Package.Detail", "To update a tourist package, you need to specify the line items.");
            }

            var existingLineItemIds = package.LineItems.Select(li => li.Id).ToList();

            foreach (var item in command.Items)
            {
                if (existingLineItemIds.Contains(new LineItemId(item.PlaceId)))
                {
                    package.UpdateLineItem(new LineItemId(item.PlaceId), new PlaceId(item.PlaceId));
                }
                else
                {
                    package.Add(new PlaceId(item.PlaceId));
                }
            }

            // Remove line items that were not included in the command
            var lineItemsToRemove = package.LineItems.Where(li => !command.Items.Any(item => new LineItemId(item.PlaceId) == li.Id)).ToList();
            foreach (var lineItem in lineItemsToRemove)
            {
                package.RemoveLineItem(lineItem.Id, _packageRepository);
            }



            foreach (var item in command.Items)
            {
                package.Add(new PlaceId(item.PlaceId));
            }

            _packageRepository.Update(package);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
