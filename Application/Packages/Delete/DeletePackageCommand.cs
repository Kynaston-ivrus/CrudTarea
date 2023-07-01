using ErrorOr;
using MediatR;

namespace Application.Packages.Delete;

public record DeletePackageCommand(Guid Id) : IRequest<ErrorOr<Unit>>;