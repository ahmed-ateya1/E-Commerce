using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Helper;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Text;

namespace E_Commerce.API.Controllers
{
    /// <summary>
    /// Controller for handling payment-related events and actions.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IOrderServices _orderServices;
        private readonly IUnitOfWork _unitOfWork;
        private const string StripeWebhookSecret = "whsec_4ff41a6d0d501015c895ced1d49519a09462910e26ddc3ac2a39722c37e2ca32";

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for logging information and errors.</param>
        /// <param name="orderServices">The service for managing orders.</param>
        /// <param name="unitOfWork">The unit of work for handling database operations.</param>
        public PaymentController(ILogger<PaymentController> logger, IOrderServices orderServices, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _orderServices = orderServices;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Handles Stripe webhook events.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        /// <remarks>
        /// This endpoint listens for Stripe webhook events such as payment succeeded, payment failed, or charge refunded.
        /// </remarks>
        [HttpPost("webHook")]
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

            _logger.LogInformation("Stripe webhook event received: {EventType}", stripeEvent.Type);

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    await HandlePaymentSucceeded(stripeEvent);
                    break;

                case "payment_intent.payment_failed":
                    await HandlePaymentFailed(stripeEvent);
                    break;

                case "charge.refunded":
                    await HandleChargeRefunded(stripeEvent);
                    break;

                default:
                    _logger.LogWarning("Unhandled Stripe event type: {EventType}", stripeEvent.Type);
                    break;
            }

            return Ok();
        }

        /// <summary>
        /// Retrieves the order associated with a specific payment intent ID.
        /// </summary>
        /// <param name="paymentIntentId">The ID of the payment intent.</param>
        /// <returns>The <see cref="Order"/> associated with the payment intent.</returns>
        private async Task<Order> GetOrder(string paymentIntentId)
        {
            return await _unitOfWork.Repository<Order>()
                .GetByAsync(o => o.PaymentIntentID == paymentIntentId, true);
        }

        /// <summary>
        /// Handles the "payment_intent.succeeded" event from Stripe.
        /// </summary>
        /// <param name="stripeEvent">The Stripe event containing the payment intent details.</param>
        private async Task HandlePaymentSucceeded(Event stripeEvent)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            if (paymentIntent == null)
            {
                _logger.LogError("Invalid payment intent object.");
                return;
            }

            var order = await GetOrder(paymentIntent.Id);
            if (order == null)
            {
                _logger.LogError("Order not found for PaymentIntent ID: {Id}", paymentIntent.Id);
                return;
            }

            await _orderServices.UpdateAsync(order, OrderStatus.Completed);
        }

        /// <summary>
        /// Handles the "payment_intent.payment_failed" event from Stripe.
        /// </summary>
        /// <param name="stripeEvent">The Stripe event containing the payment intent details.</param>
        private async Task HandlePaymentFailed(Event stripeEvent)
        {
            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            if (paymentIntent == null)
            {
                _logger.LogError("Invalid payment intent object.");
                return;
            }

            var order = await GetOrder(paymentIntent.Id);
            if (order == null)
            {
                _logger.LogError("Order not found for PaymentIntent ID: {Id}", paymentIntent.Id);
                return;
            }

            await _orderServices.UpdateAsync(order, OrderStatus.FailedPayment);
        }

        /// <summary>
        /// Handles the "charge.refunded" event from Stripe.
        /// </summary>
        /// <param name="stripeEvent">The Stripe event containing the refund details.</param>
        private async Task HandleChargeRefunded(Event stripeEvent)
        {
            var refund = stripeEvent.Data.Object as Refund;
            if (refund == null)
            {
                _logger.LogError("Invalid refund object.");
                return;
            }

            var order = await GetOrder(refund.PaymentIntentId);
            if (order == null)
            {
                _logger.LogError("Order not found for PaymentIntent ID: {Id}", refund.PaymentIntentId);
                return;
            }

            await _orderServices.UpdateAsync(order, OrderStatus.Cancelled);
        }
    }
}
