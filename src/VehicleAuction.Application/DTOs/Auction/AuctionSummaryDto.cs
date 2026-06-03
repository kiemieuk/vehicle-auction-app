using VehicleAuction.Domain.Enums;

namespace VehicleAuction.Application.DTOs.Auction;

public class AuctionSummaryDto
{
    public Guid Id { get; set; }
    public string VehicleTitle { get; set; } = string.Empty;
    public DateTime EndTime { get; set; }
    public AuctionStatus Status { get; set; }
}
