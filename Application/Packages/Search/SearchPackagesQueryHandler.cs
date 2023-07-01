using MediatR;
using ErrorOr;
using Application.Packages.Common;
using Domain.ValueObjects;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Places;
using Domain.Packages;

namespace Application.Packages.Search
{
    public class SearchPackagesQueryHandler : IRequestHandler<SearchPackagesQuery, ErrorOr<List<packageResponse>>>
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IPlaceRepository _placeRepository;

        public SearchPackagesQueryHandler(IPackageRepository packageRepository, IPlaceRepository placeRepository)
        {
            _packageRepository = packageRepository;
            _placeRepository = placeRepository;
        }

        public async Task<ErrorOr<List<packageResponse>>> Handle(SearchPackagesQuery query, CancellationToken cancellationToken)
        {
            var packages = await _packageRepository.Search(query.Name, query.Description, query.TravelDate, query.Price, query.Ubication);

            var packageResponses = new List<packageResponse>();

            foreach (var package in packages)
            {
                var lineItemResponses = new List<LineItemResponse>();

                foreach (var lineItem in package.LineItems)
                {
                    var place = await _placeRepository.GetByIdAsync(lineItem.PlaceId);
                    var name = place?.Name ?? string.Empty;
                    var ubication = place?.Ubication ?? string.Empty;

                    var lineItemResponse = new LineItemResponse(name, ubication);
                    lineItemResponses.Add(lineItemResponse);
                }

                var packageResponse = new packageResponse(
                    (Guid)package.Id.Value,
                    package.Name,
                    package.Description,
                    new MoneyResponse(package.Price.Currency, package.Price.Amount),
                    package.TravelDate,
                    lineItemResponses);

                packageResponses.Add(packageResponse);
            }

            return packageResponses;
        }
    }
}
