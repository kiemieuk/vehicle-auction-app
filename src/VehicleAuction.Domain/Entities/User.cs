using VehicleAuction.Domain.Common;
using VehicleAuction.Domain.Enums;

namespace VehicleAuction.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public decimal TokenBalance { get; set; }
    public UserRole Role { get; set; } = UserRole.Bidder;

    public ICollection<Bid> Bids { get; set; } = new List<Bid>();
    public ICollection<TokenTransaction> TokenTransactions { get; set; } = new List<TokenTransaction>();
    public ICollection<TopUpRequest> TopUpRequests { get; set; } = new List<TopUpRequest>();
}
