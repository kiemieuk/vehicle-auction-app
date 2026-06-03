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
            Amount = decimal.ToInt64(decimal.Round(amount * 100m, 0, MidpointRounding.AwayFromZero)),
            Currency = currency,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        }, cancellationToken: cancellationToken);

        return intent.ClientSecret ?? throw new InvalidOperationException("Stripe did not return a payment client secret.");
    }

public async Task<bool> VerifyPaymentAsync(string paymentReference, CancellationToken cancellationToken = default)
{
    // Accept either a PaymentIntent id (pi_...) or a client secret (pi_..._secret_...)
    var intentId = paymentReference;
    const string secretMarker = "_secret_";
    var idx = paymentReference.IndexOf(secretMarker, StringComparison.Ordinal);
    if (idx > 0)
    {
        intentId = paymentReference[..idx];
    }

    var service = new PaymentIntentService();
    var intent = await service.GetAsync(intentId, cancellationToken: cancellationToken);
    return string.Equals(intent.Status, "succeeded", StringComparison.OrdinalIgnoreCase);
}
    }
}
