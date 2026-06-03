using VehicleAuction.Domain.Enums;

namespace VehicleAuction.Application.DTOs.Token;

public class TransactionDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public TokenTransactionType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
