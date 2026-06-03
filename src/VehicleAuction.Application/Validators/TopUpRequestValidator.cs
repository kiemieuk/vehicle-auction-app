using FluentValidation;
using VehicleAuction.Application.DTOs.Token;

namespace VehicleAuction.Application.Validators;

public class TopUpRequestValidator : AbstractValidator<TopUpRequest>
{
    public TopUpRequestValidator()
    {
        RuleFor(x => x.FiatAmount).GreaterThan(0);
    }
}
