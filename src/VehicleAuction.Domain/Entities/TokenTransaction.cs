using VehicleAuction.Domain.Common;
using VehicleAuction.Domain.Enums;

namespace VehicleAuction.Domain.Entities;

public class TokenTransaction : BaseEntity
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public TokenTransactionType Type { get; set; }
    public string? PaymentReference { get; set; }
    public string Description { get; set; } = string.Empty;

    public User? User { get; set; }
}
