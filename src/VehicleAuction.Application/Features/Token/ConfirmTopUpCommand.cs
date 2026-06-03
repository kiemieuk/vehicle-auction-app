using MediatR;
using VehicleAuction.Application.Interfaces;
using VehicleAuction.Domain.Entities;
using VehicleAuction.Domain.Enums;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Token;

public record ConfirmTopUpCommand(Guid TopUpRequestId) : IRequest<bool>;

public class ConfirmTopUpCommandHandler(IUnitOfWork unitOfWork, IPaymentService paymentService) : IRequestHandler<ConfirmTopUpCommand, bool>
{
    public async Task<bool> Handle(ConfirmTopUpCommand command, CancellationToken cancellationToken)
    {
        var topUp = await unitOfWork.TopUpRequests.GetByIdAsync(command.TopUpRequestId, cancellationToken)
                   ?? throw new KeyNotFoundException("Top-up request not found.");

        if (topUp.Status == TopUpStatus.Completed)
        {
            return true;
        }

        if (topUp.Status == TopUpStatus.Failed || string.IsNullOrWhiteSpace(topUp.StripePaymentIntentId))
        {
            return false;
        }

        var verified = await paymentService.VerifyPaymentAsync(topUp.StripePaymentIntentId, cancellationToken);
        if (!verified)
        {
            topUp.Status = TopUpStatus.Failed;
            await unitOfWork.TopUpRequests.UpdateAsync(topUp, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return false;
        }

        var user = await unitOfWork.Users.GetByIdAsync(topUp.UserId, cancellationToken)
                   ?? throw new KeyNotFoundException("User not found.");

        user.TokenBalance += topUp.TokenAmount;
        topUp.Status = TopUpStatus.Completed;

        await unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await unitOfWork.TopUpRequests.UpdateAsync(topUp, cancellationToken);
        await unitOfWork.TokenTransactions.AddAsync(new TokenTransaction
        {
            UserId = user.Id,
            Amount = topUp.TokenAmount,
            Type = TokenTransactionType.TopUp,
            PaymentReference = topUp.StripePaymentIntentId,
            Description = "Token top-up completed"
        }, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
