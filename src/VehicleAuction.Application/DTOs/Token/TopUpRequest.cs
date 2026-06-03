using VehicleAuction.Domain.Enums;

namespace VehicleAuction.Application.DTOs.Token;

public class TopUpRequest
{
    public decimal FiatAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
}
