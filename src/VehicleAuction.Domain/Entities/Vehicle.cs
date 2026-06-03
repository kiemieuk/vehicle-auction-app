using VehicleAuction.Domain.Common;
using VehicleAuction.Domain.Enums;

namespace VehicleAuction.Domain.Entities;

public class Vehicle : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public List<string> ImageUrls { get; set; } = [];
    public decimal ReservePrice { get; set; }
    public VehicleStatus Status { get; set; } = VehicleStatus.Draft;
    public Guid AuctioneerId { get; set; }

    public Auction? Auction { get; set; }
}
