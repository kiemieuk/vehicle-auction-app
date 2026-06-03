using MediatR;
using VehicleAuction.Application.Interfaces;
using VehicleAuction.Domain.Enums;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Auction;

public record EndAuctionCommand(Guid AuctionId) : IRequest<bool>;

public class EndAuctionCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IAuctionNotificationService notificationService) : IRequestHandler<EndAuctionCommand, bool>
{
    public async Task<bool> Handle(EndAuctionCommand command, CancellationToken cancellationToken)
    {
        var auction = await unitOfWork.Auctions.GetByIdAsync(command.AuctionId, cancellationToken);
        if (auction is null)
        {
            return false;
        }

        var allBids = (await unitOfWork.Bids.GetByAuctionIdAsync(command.AuctionId, cancellationToken)).ToList();
        var highestBid = allBids.OrderByDescending(x => x.TokenAmount).ThenBy(x => x.PlacedAt).FirstOrDefault();

        if (highestBid is not null)
        {
            auction.WinnerUserId = highestBid.UserId;
            auction.FinalTokenAmount = highestBid.TokenAmount;
            await tokenService.DeductTokensAsync(highestBid.UserId, highestBid.TokenAmount, cancellationToken);

            var reservedByUser = allBids
                .GroupBy(b => b.UserId)
                .Select(g => new { UserId = g.Key, ReservedAmount = g.Sum(x => x.TokenAmount) });

            foreach (var userReserve in reservedByUser)
            {
                var refundableAmount = userReserve.UserId == highestBid.UserId
                    ? userReserve.ReservedAmount - highestBid.TokenAmount
                    : userReserve.ReservedAmount;

                if (refundableAmount > 0)
                {
                    await tokenService.RefundTokensAsync(userReserve.UserId, refundableAmount, cancellationToken);
                }
            }
        }

        auction.Status = AuctionStatus.Ended;
        await unitOfWork.Auctions.UpdateAsync(auction, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await notificationService.NotifyAuctionEndedAsync(auction.Id, auction.WinnerUserId, cancellationToken);
        return true;
    }
}
