using MediatR;
using ErrorOr;
using Application.Packages.Common;
using Domain.ValueObjects;

namespace Application.Packages.Common;

public record packageResponse(
    Guid Id,
    string Name,
    string Description,
    MoneyResponse Price,
    DateTime TravelDate,
    List<LineItemResponse> LineItems
    ) : IRequest<ErrorOr<packageResponse>>;

public record MoneyResponse(
    string Currency,
    decimal Amount
    );

public record LineItemResponse(string Name, string Ubication);

