using ErrorOr;
using MediatR;
using Application.Packages.Common;

namespace Application.Packages.GetAll;

public record GetAllPackagesQuery() : IRequest<ErrorOr<IReadOnlyList<packageResponse>>>;