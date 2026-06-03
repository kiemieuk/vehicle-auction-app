using FluentValidation;
using VehicleAuction.Application.DTOs.Bid;

namespace VehicleAuction.Application.Validators;

public class PlaceBidRequestValidator : AbstractValidator<PlaceBidRequest>
{
    public PlaceBidRequestValidator()
    {
        RuleFor(x => x.AuctionId).NotEmpty();
        RuleFor(x => x.TokenAmount).GreaterThan(0);
    }
}
