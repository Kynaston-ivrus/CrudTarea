using Application.Packages.Common;
using ErrorOr;
using MediatR;

namespace Application.Packages.GetById;

public record GetPackageByIdQuery(Guid Id) : IRequest<ErrorOr<packageResponse>>;