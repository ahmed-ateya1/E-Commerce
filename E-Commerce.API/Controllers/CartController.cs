using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    /// <summary>
    /// Provides endpoints for managing the shopping cart.
    /// Interacts with Redis to retrieve, update, and delete cart data.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IRedisCartServices _redisCartServices;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="redisCartServices">The service used to interact with Redis for cart operations.</param>
        /// <remarks>
        /// The <paramref name="redisCartServices"/> parameter is injected via dependency injection.
        /// </remarks>
        public CartController(IRedisCartServices redisCartServices)
        {
            _redisCartServices = redisCartServices;
        }

        /// <summary>
        /// Retrieves the shopping cart from Redis by its unique identifier.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart to be retrieved.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{Cart}"/> with the cart if found; otherwise, returns a <c>NotFound</c> result.
        /// </returns>
        /// <remarks>
        /// If the cart does not exist in Redis, a 404 Not Found response will be returned.
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<Cart>> GetCartAsync(Guid cartId)
        {
            var cart = await _redisCartServices.GetCartAsync(cartId);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        /// <summary>
        /// Updates or creates a shopping cart in Redis.
        /// </summary>
        /// <param name="cart">The <see cref="Cart"/> object containing the data to be updated or created in Redis.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{Cart}"/> containing the updated or created cart.
        /// </returns>
        /// <remarks>
        /// The cart will be serialized and stored in Redis with a 30-day expiration time.
        /// If the update is successful, the same cart object will be returned.
        /// </remarks>
        [HttpPost]
        public async Task<ActionResult<Cart>> UpdateCartAsync(Cart cart)
        {
            var updatedCart = await _redisCartServices.UpdateCartAsync(cart);
            if (updatedCart == null)
            {
                return BadRequest("Failed to update the cart.");
            }
            return Ok(updatedCart);
        }

        /// <summary>
        /// Deletes a shopping cart from Redis by its unique identifier.
        /// </summary>
        /// <param name="cartId">The unique identifier of the cart to be deleted.</param>
        /// <returns>
        /// Returns an <see cref="ActionResult{bool}"/> indicating whether the cart was successfully deleted.
        /// </returns>
        /// <remarks>
        /// If the cart does not exist or the deletion fails, this method will return <c>false</c>.
        /// </remarks>
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteCartAsync(Guid cartId)
        {
            var result = await _redisCartServices.DeleteCartAsync(cartId);
            if (!result)
            {
                return NotFound("Cart not found.");
            }
            return Ok(result);
        }
    }
}
