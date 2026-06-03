using VehicleAuction.Application.Interfaces;
using VehicleAuction.Domain.Entities;
using VehicleAuction.Domain.Enums;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Infrastructure.Services;

public class TokenService(IUnitOfWork unitOfWork, IPaymentService paymentService) : ITokenService
{
    public async Task<string> TopUpAsync(Guid userId, decimal fiatAmount, string paymentMethod, CancellationToken cancellationToken = default)
    {
        var paymentIntentId = await paymentService.CreatePaymentIntentAsync(fiatAmount, "usd", cancellationToken);

        await unitOfWork.TopUpRequests.AddAsync(new TopUpRequest
        {
            UserId = userId,
            FiatAmount = fiatAmount,
            TokenAmount = fiatAmount,
            PaymentMethod = Enum.TryParse<PaymentMethod>(paymentMethod, true, out var parsed) ? parsed : PaymentMethod.CreditCard,
            StripePaymentIntentId = paymentIntentId,
            Status = TopUpStatus.Pending
        }, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return paymentIntentId;
    }

    public async Task<bool> ConfirmTopUpAsync(Guid topUpRequestId, CancellationToken cancellationToken = default)
    {
        var topUpRequest = await unitOfWork.TopUpRequests.GetByIdAsync(topUpRequestId, cancellationToken);
        if (topUpRequest is null)
        {
            return false;
        }

        if (topUpRequest.Status == TopUpStatus.Completed)
        {
            return true;
        }

        if (topUpRequest.Status == TopUpStatus.Failed || string.IsNullOrWhiteSpace(topUpRequest.StripePaymentIntentId))
        {
            return false;
        }

        var isPaid = await paymentService.VerifyPaymentAsync(topUpRequest.StripePaymentIntentId, cancellationToken);
        if (!isPaid)
        {
            topUpRequest.Status = TopUpStatus.Failed;
            await unitOfWork.TopUpRequests.UpdateAsync(topUpRequest, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return false;
        }

        var user = await unitOfWork.Users.GetByIdAsync(topUpRequest.UserId, cancellationToken)
                   ?? throw new KeyNotFoundException("User not found.");

        user.TokenBalance += topUpRequest.TokenAmount;
        topUpRequest.Status = TopUpStatus.Completed;

        await unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await unitOfWork.TopUpRequests.UpdateAsync(topUpRequest, cancellationToken);
        await unitOfWork.TokenTransactions.AddAsync(new TokenTransaction
        {
            UserId = user.Id,
            Amount = topUpRequest.TokenAmount,
            Type = TokenTransactionType.TopUp,
            PaymentReference = topUpRequest.StripePaymentIntentId,
            Description = "Top-up confirmed"
        }, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<decimal> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId, cancellationToken)
                   ?? throw new KeyNotFoundException("User not found.");
        return user.TokenBalance;
    }

    public async Task ReserveTokensAsync(Guid userId, decimal amount, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId, cancellationToken)
                   ?? throw new KeyNotFoundException("User not found.");

        if (user.TokenBalance < amount)
        {
            throw new InvalidOperationException("Insufficient token balance.");
        }

        user.TokenBalance -= amount;
        await unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await unitOfWork.TokenTransactions.AddAsync(new TokenTransaction
        {
            UserId = user.Id,
            Amount = -amount,
            Type = TokenTransactionType.BidReservation,
            Description = "Tokens reserved for bid"
        }, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task RefundTokensAsync(Guid userId, decimal amount, CancellationToken cancellationToken = default)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId, cancellationToken)
                   ?? throw new KeyNotFoundException("User not found.");

        user.TokenBalance += amount;
        await unitOfWork.Users.UpdateAsync(user, cancellationToken);
        await unitOfWork.TokenTransactions.AddAsync(new TokenTransaction
        {
            UserId = user.Id,
            Amount = amount,
            Type = TokenTransactionType.BidRefund,
            Description = "Bid token refund"
        }, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeductTokensAsync(Guid userId, decimal amount, CancellationToken cancellationToken = default)
    {
        await unitOfWork.TokenTransactions.AddAsync(new TokenTransaction
        {
            UserId = userId,
            Amount = -amount,
            Type = TokenTransactionType.AuctionWin,
            Description = "Auction win settlement"
        }, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
