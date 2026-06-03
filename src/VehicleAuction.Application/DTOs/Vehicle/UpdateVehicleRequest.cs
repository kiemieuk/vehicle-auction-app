namespace VehicleAuction.Application.DTOs.Vehicle;

public class UpdateVehicleRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal ReservePrice { get; set; }
}
