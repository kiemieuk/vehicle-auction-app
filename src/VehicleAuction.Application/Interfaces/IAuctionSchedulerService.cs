namespace VehicleAuction.Application.Interfaces;

public interface IAuctionSchedulerService
{
    Task ScheduleAuctionEndAsync(Guid auctionId, DateTime endTime, CancellationToken cancellationToken = default);
    Task ScheduleAuctionStartAsync(Guid auctionId, DateTime startTime, CancellationToken cancellationToken = default);
}
