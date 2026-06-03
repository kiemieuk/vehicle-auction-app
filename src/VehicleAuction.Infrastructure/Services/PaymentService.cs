using Stripe;
using VehicleAuction.Application.Interfaces;

namespace VehicleAuction.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    public async Task<string> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken cancellationToken = default)
    {
        var service = new PaymentIntentService();
        var intent = await service.CreateAsync(new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100),
            Currency = currency,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        }, cancellationToken: cancellationToken);

        return intent.ClientSecret ?? intent.Id;
    }

    public async Task<bool> VerifyPaymentAsync(string paymentReference, CancellationToken cancellationToken = default)
    {
        var service = new PaymentIntentService();
        var intent = await service.GetAsync(paymentReference, cancellationToken: cancellationToken);
        return string.Equals(intent.Status, "succeeded", StringComparison.OrdinalIgnoreCase);
    }
}
