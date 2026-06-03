using VehicleAuction.Domain.Common;
using VehicleAuction.Domain.Enums;

namespace VehicleAuction.Domain.Entities;

public class TopUpRequest : BaseEntity
{
    public Guid UserId { get; set; }
    public decimal FiatAmount { get; set; }
    public decimal TokenAmount { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public TopUpStatus Status { get; set; } = TopUpStatus.Pending;
    public string? StripePaymentIntentId { get; set; }

    public User? User { get; set; }
}
