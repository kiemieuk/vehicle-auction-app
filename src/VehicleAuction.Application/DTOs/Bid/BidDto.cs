namespace VehicleAuction.Application.DTOs.Bid;

public class BidDto
{
    public Guid Id { get; set; }
    public Guid AuctionId { get; set; }
    public Guid UserId { get; set; }
    public decimal TokenAmount { get; set; }
    public DateTime PlacedAt { get; set; }
    public bool IsWinning { get; set; }
}
