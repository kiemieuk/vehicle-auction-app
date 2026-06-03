using VehicleAuction.Application.Interfaces;

namespace VehicleAuction.Infrastructure.Services;

public class AuctionNotificationService : IAuctionNotificationService
{
    public Task NotifyBidPlacedAsync(Guid auctionId, Guid bidId, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task NotifyAuctionEndedAsync(Guid auctionId, Guid? winnerUserId, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task NotifyAuctionStartedAsync(Guid auctionId, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
