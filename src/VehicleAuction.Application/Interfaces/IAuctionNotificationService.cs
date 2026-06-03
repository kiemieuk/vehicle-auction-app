namespace VehicleAuction.Application.Interfaces;

public interface IAuctionNotificationService
{
    Task NotifyBidPlacedAsync(Guid auctionId, Guid bidId, CancellationToken cancellationToken = default);
    Task NotifyAuctionEndedAsync(Guid auctionId, Guid? winnerUserId, CancellationToken cancellationToken = default);
    Task NotifyAuctionStartedAsync(Guid auctionId, CancellationToken cancellationToken = default);
}
