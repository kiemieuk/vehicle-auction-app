using VehicleAuction.Application.Interfaces;

namespace VehicleAuction.Infrastructure.Services;

public class AuctionSchedulerService : IAuctionSchedulerService
{
    public Task ScheduleAuctionEndAsync(Guid auctionId, DateTime endTime, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task ScheduleAuctionStartAsync(Guid auctionId, DateTime startTime, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
