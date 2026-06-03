using VehicleAuction.Domain.Common;
using VehicleAuction.Domain.Enums;

namespace VehicleAuction.Domain.Entities;

public class Auction : BaseEntity
{
    public Guid VehicleId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal MinBidIncrement { get; set; }
    public AuctionStatus Status { get; set; } = AuctionStatus.Scheduled;
    public Guid? WinnerUserId { get; set; }
    public decimal? FinalTokenAmount { get; set; }

    public Vehicle? Vehicle { get; set; }
    public ICollection<Bid> Bids { get; set; } = new List<Bid>();
}
