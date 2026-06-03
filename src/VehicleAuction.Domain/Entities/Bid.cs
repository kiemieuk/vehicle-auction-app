using VehicleAuction.Domain.Common;

namespace VehicleAuction.Domain.Entities;

public class Bid : BaseEntity
{
    public Guid AuctionId { get; set; }
    public Guid UserId { get; set; }
    public decimal TokenAmount { get; set; }
    public DateTime PlacedAt { get; set; } = DateTime.UtcNow;
    public bool IsWinning { get; set; }

    public Auction? Auction { get; set; }
    public User? User { get; set; }
}
