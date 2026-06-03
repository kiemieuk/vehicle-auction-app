using VehicleAuction.Domain.Enums;

namespace VehicleAuction.Application.DTOs.Vehicle;

public class VehicleDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal ReservePrice { get; set; }
    public VehicleStatus Status { get; set; }
}
