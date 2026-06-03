namespace VehicleAuction.Application.Interfaces;

public interface IPaymentService
{
    Task<string> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken cancellationToken = default);
    Task<bool> VerifyPaymentAsync(string paymentReference, CancellationToken cancellationToken = default);
}
