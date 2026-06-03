using MediatR;
using VehicleAuction.Application.Interfaces;
using VehicleAuction.Domain.Enums;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Auction;

public record StartAuctionCommand(Guid AuctionId) : IRequest<bool>;

public class StartAuctionCommandHandler(IUnitOfWork unitOfWork, IAuctionSchedulerService schedulerService, IAuctionNotificationService notificationService) : IRequestHandler<StartAuctionCommand, bool>
{
    public async Task<bool> Handle(StartAuctionCommand command, CancellationToken cancellationToken)
    {
        var auction = await unitOfWork.Auctions.GetByIdAsync(command.AuctionId, cancellationToken);
        if (auction is null)
        {
            return false;
        }

        auction.Status = AuctionStatus.Live;
        await unitOfWork.Auctions.UpdateAsync(auction, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await schedulerService.ScheduleAuctionEndAsync(auction.Id, auction.EndTime, cancellationToken);
        await notificationService.NotifyAuctionStartedAsync(auction.Id, cancellationToken);

        return true;
    }
}
