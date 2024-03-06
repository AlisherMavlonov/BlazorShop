using Stripe.Checkout;

namespace RazorShop.Server.Services.PaymentService;

public interface IPaymentService
{
    Task<Session> CreateCheckoutSession();
}