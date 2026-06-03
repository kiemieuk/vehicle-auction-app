using MediatR;
using VehicleAuction.Application.Interfaces;
using VehicleAuction.Domain.Enums;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Token;

public record TopUpTokenCommand(Guid UserId, decimal FiatAmount, PaymentMethod PaymentMethod) : IRequest<string>;

public class TopUpTokenCommandHandler(IUnitOfWork unitOfWork, IPaymentService paymentService) : IRequestHandler<TopUpTokenCommand, string>
{
    public async Task<string> Handle(TopUpTokenCommand command, CancellationToken cancellationToken)
    {
        var topUp = new Domain.Entities.TopUpRequest
        {
            UserId = command.UserId,
            FiatAmount = command.FiatAmount,
            TokenAmount = command.FiatAmount,
            PaymentMethod = command.PaymentMethod,
            Status = TopUpStatus.Pending
        };

        topUp.StripePaymentIntentId = await paymentService.CreatePaymentIntentAsync(command.FiatAmount, "usd", cancellationToken);

        await unitOfWork.TopUpRequests.AddAsync(topUp, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return topUp.StripePaymentIntentId;
    }
}
