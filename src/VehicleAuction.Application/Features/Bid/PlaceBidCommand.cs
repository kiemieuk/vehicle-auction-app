using MediatR;
using VehicleAuction.Application.Interfaces;
using VehicleAuction.Domain.Enums;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Bid;

public record PlaceBidCommand(Guid AuctionId, Guid UserId, decimal TokenAmount) : IRequest<Guid>;

public class PlaceBidCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IAuctionNotificationService notificationService) : IRequestHandler<PlaceBidCommand, Guid>
{
    public async Task<Guid> Handle(PlaceBidCommand command, CancellationToken cancellationToken)
    {
        var auction = await unitOfWork.Auctions.GetByIdAsync(command.AuctionId, cancellationToken)
                     ?? throw new KeyNotFoundException("Auction not found.");

        if (auction.Status != AuctionStatus.Live)
        {
            throw new InvalidOperationException("Auction is not live.");
        }

        var highestBid = await unitOfWork.Bids.GetHighestBidAsync(command.AuctionId, cancellationToken);
        if (highestBid is not null && command.TokenAmount < highestBid.TokenAmount + auction.MinBidIncrement)
        {
            throw new InvalidOperationException("Bid does not meet minimum increment.");
        }

        await tokenService.ReserveTokensAsync(command.UserId, command.TokenAmount, cancellationToken);

        var bid = new VehicleAuction.Domain.Entities.Bid
        {
            AuctionId = command.AuctionId,
            UserId = command.UserId,
            TokenAmount = command.TokenAmount,
            IsWinning = true,
            PlacedAt = DateTime.UtcNow
        };

        if (highestBid is not null)
        {
            highestBid.IsWinning = false;
            await unitOfWork.Bids.UpdateAsync(highestBid, cancellationToken);
        }

        await unitOfWork.Bids.AddAsync(bid, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await notificationService.NotifyBidPlacedAsync(command.AuctionId, bid.Id, cancellationToken);
        return bid.Id;
    }
}
