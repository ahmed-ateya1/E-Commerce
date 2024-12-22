using E_Commerce.Core.ServicesContract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace E_Commerce.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(ILogger<PaymentService> logger, IConfiguration configuration)
        {
            _logger = logger;
            StripeConfiguration.ApiKey = configuration["Stripe:Secretkey"];
        }

        public async Task<PaymentIntent> ConfirmPayment(string paymentIntentId)
        {
            _logger.LogInformation("Confirming payment intent with ID {PaymentIntentId}.", paymentIntentId);

            var service = new PaymentIntentService();
            var paymentIntent = await service.ConfirmAsync(paymentIntentId);

            _logger.LogInformation("Payment intent {PaymentIntentId} confirmed.", paymentIntent.Id);
            return paymentIntent;
        }

        public async Task<PaymentIntent> CreatePaymentIntent(decimal amount, string currency, string description)
        {
            _logger.LogInformation("Creating a payment intent for amount {Amount} {Currency}.", amount, currency);

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100), 
                Currency = currency,
                Description = description,
                PaymentMethodTypes = new List<string> { "card" }
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);

            _logger.LogInformation("Payment intent created with ID {PaymentIntentId}.", paymentIntent.Id);
            return paymentIntent;
        }

        public async Task<Refund> RefundPayment(string paymentIntentId)
        {
            _logger.LogInformation("Processing refund for payment intent ID {PaymentIntentId}.", paymentIntentId);

            var options = new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId
            };

            var service = new RefundService();
            var refund = await service.CreateAsync(options);

            _logger.LogInformation("Refund processed for payment intent ID {PaymentIntentId}.", paymentIntentId);
            return refund;
        }
    }
}
