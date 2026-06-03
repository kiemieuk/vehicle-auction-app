namespace VehicleAuction.Application.DTOs.Auction;

public class CreateAuctionRequest
{
    public Guid VehicleId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal MinBidIncrement { get; set; }
}
