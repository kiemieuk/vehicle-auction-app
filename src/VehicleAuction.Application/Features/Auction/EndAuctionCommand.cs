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

        var highestBid = await unitOfWork.Bids.GetHighestBidAsync(command.AuctionId, cancellationToken);

        if (highestBid is not null)
        {
            auction.WinnerUserId = highestBid.UserId;
            auction.FinalTokenAmount = highestBid.TokenAmount;
            await tokenService.DeductTokensAsync(highestBid.UserId, highestBid.TokenAmount, cancellationToken);

            var losingBids = (await unitOfWork.Bids.GetByAuctionIdAsync(command.AuctionId, cancellationToken))
                .Where(b => b.UserId != highestBid.UserId)
                .GroupBy(b => b.UserId)
                .Select(g => new { UserId = g.Key, Amount = g.Max(x => x.TokenAmount) });

            foreach (var bid in losingBids)
            {
                await tokenService.RefundTokensAsync(bid.UserId, bid.Amount, cancellationToken);
            }
        }

        auction.Status = AuctionStatus.Ended;
        await unitOfWork.Auctions.UpdateAsync(auction, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await notificationService.NotifyAuctionEndedAsync(auction.Id, auction.WinnerUserId, cancellationToken);
        return true;
    }
}
