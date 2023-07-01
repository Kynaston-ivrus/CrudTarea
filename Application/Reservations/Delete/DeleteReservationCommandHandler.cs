
using Domain.Primitives;
using ErrorOr;
using MediatR;
using Domain.Packages;

namespace Application.Packages.Delete;

internal sealed class DeletePackageCommandHandler : IRequestHandler<DeletePackageCommand, ErrorOr<Unit>>
{
    private readonly IPackageRepository _packageRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeletePackageCommandHandler(IPackageRepository packageRepository, IUnitOfWork unitOfWork)
    {
        _packageRepository = packageRepository ?? throw new ArgumentNullException(nameof(packageRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<ErrorOr<Unit>> Handle(DeletePackageCommand command, CancellationToken cancellationToken)
    {
        if (await _packageRepository.GetByIdAsync(new PackageId(command.Id)) is not Package package)
        {
            return Error.NotFound("Package.NotFound", "The package with the provide Id was not found.");
        }

        _packageRepository.Delete(package);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
