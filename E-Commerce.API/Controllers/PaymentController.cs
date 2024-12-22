using E_Commerce.Core.Domain.RepositoriesContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Text;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private const string StripeWebhookSecret = "whsec_JjHcC9SCvZRmKv59pu1wiSXhxVz3lQAG";
        private readonly IUnitOfWork _unitOfWork;

        public PaymentController(ILogger<PaymentController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> Handle()
        {
            string json = await new StreamReader(HttpContext.Request.Body, Encoding.UTF8).ReadToEndAsync();
            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    StripeWebhookSecret
                );
            }
            catch (StripeException ex)
            {
                _logger.LogError(ex, "Failed to verify Stripe webhook signature.");
                return BadRequest();
            }

            // Handle the event
            _logger.LogInformation("Stripe webhook event received: {EventType}", stripeEvent.Type);

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    _logger.LogInformation("Payment succeeded for PaymentIntent ID: {Id}", paymentIntent.Id);
                    var order = await _unitOfWork.Repository<Order>()
                        .GetByAsync(x => x.PaymentIntentID == paymentIntent.Id);
                    order.OrderStatus = Core.Helper.OrderStatus.Shipped;
                    break;

                case "payment_intent.payment_failed":
                    var failedPaymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    _logger.LogError("Payment failed for PaymentIntent ID: {Id}", failedPaymentIntent.Id);
                    // Handle payment failure.
                    break;

                case "charge.refunded":
                    var refund = stripeEvent.Data.Object as Refund;
                    _logger.LogInformation("Refund processed for PaymentIntent ID: {Id}", refund.PaymentIntentId);
                    // Update refund status in your system.
                    break;

                default:
                    _logger.LogWarning("Unhandled Stripe event type: {EventType}", stripeEvent.Type);
                    break;
            }

            return Ok();
        }
    }
}
