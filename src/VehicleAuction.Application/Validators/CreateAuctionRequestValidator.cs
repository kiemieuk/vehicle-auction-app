using FluentValidation;
using VehicleAuction.Application.DTOs.Auction;

namespace VehicleAuction.Application.Validators;

public class CreateAuctionRequestValidator : AbstractValidator<CreateAuctionRequest>
{
    public CreateAuctionRequestValidator()
    {
        RuleFor(x => x.VehicleId).NotEmpty();
        RuleFor(x => x.MinBidIncrement).GreaterThan(0);
        RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime);
    }
}
