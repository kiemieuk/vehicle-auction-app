using VehicleAuction.Domain.Enums;

namespace VehicleAuction.Application.DTOs.Auction;

public class AuctionDto
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal MinBidIncrement { get; set; }
    public AuctionStatus Status { get; set; }
    public Guid? WinnerUserId { get; set; }
    public decimal? FinalTokenAmount { get; set; }
}
