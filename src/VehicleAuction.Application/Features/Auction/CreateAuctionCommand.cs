using MediatR;
using VehicleAuction.Application.DTOs.Auction;
using VehicleAuction.Domain.Enums;
using VehicleAuction.Domain.Interfaces;

namespace VehicleAuction.Application.Features.Auction;

public record CreateAuctionCommand(CreateAuctionRequest Request) : IRequest<Guid>;

public class CreateAuctionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateAuctionCommand, Guid>
{
    public async Task<Guid> Handle(CreateAuctionCommand command, CancellationToken cancellationToken)
    {
        var vehicle = await unitOfWork.Vehicles.GetByIdAsync(command.Request.VehicleId, cancellationToken)
            ?? throw new KeyNotFoundException("Vehicle not found.");

        if (vehicle.Status != VehicleStatus.Approved)
        {
            throw new InvalidOperationException("Vehicle must be approved before auction creation.");
        }

        var auction = new Domain.Entities.Auction
        {
            VehicleId = command.Request.VehicleId,
            StartTime = command.Request.StartTime,
            EndTime = command.Request.EndTime,
            MinBidIncrement = command.Request.MinBidIncrement,
            Status = AuctionStatus.Scheduled
        };

        await unitOfWork.Auctions.AddAsync(auction, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return auction.Id;
    }
}
