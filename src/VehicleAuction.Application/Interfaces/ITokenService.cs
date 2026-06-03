namespace VehicleAuction.Application.Interfaces;

public interface ITokenService
{
    Task<string> TopUpAsync(Guid userId, decimal fiatAmount, string paymentMethod, CancellationToken cancellationToken = default);
    Task<bool> ConfirmTopUpAsync(Guid topUpRequestId, CancellationToken cancellationToken = default);
    Task<decimal> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default);
    Task ReserveTokensAsync(Guid userId, decimal amount, CancellationToken cancellationToken = default);
    Task RefundTokensAsync(Guid userId, decimal amount, CancellationToken cancellationToken = default);
    Task DeductTokensAsync(Guid userId, decimal amount, CancellationToken cancellationToken = default);
}
