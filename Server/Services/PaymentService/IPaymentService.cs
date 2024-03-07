using Stripe.Checkout;

namespace RazorShop.Server.Services.PaymentService;

public interface IPaymentService
{
    Task<Session> CreateCheckoutSession();
    Task<ServiceResponse<bool>> FulFillOrder(HttpRequest request);
}