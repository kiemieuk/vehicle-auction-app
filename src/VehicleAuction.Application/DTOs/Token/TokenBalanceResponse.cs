namespace VehicleAuction.Application.DTOs.Token;

public class TokenBalanceResponse
{
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
}
