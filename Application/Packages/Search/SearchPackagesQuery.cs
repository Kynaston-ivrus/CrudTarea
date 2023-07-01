using MediatR;
using ErrorOr;
using Application.Packages.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using Domain.Packages;
using Domain.Places;

namespace Application.Packages.Search;
public record SearchPackagesQuery(
    string Name,
    string Description,
    DateTime? TravelDate,
    decimal? Price,
    string Ubication
) : IRequest<ErrorOr<List<packageResponse>>>;