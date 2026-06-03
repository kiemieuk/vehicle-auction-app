namespace VehicleAuction.Application.DTOs.Bid;

public class PlaceBidRequest
{
    public Guid AuctionId { get; set; }
    public decimal TokenAmount { get; set; }
}
