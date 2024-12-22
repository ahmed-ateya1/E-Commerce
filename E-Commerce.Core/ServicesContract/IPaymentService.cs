using E_Commerce.Core.Dtos;
using Stripe;

namespace E_Commerce.Core.ServicesContract
{
    public interface IPaymentService
    {
        Task<PaymentIntent> CreatePaymentIntent(decimal amount, string currency, string description);
        Task<PaymentIntent> ConfirmPayment(string paymentIntentId);
        Task<Refund> RefundPayment(string paymentIntentId);
        Task<ServiceResponse> ProcessRefund(string paymentIntentId);
    }
}
