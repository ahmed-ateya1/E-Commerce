using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IRedisCartServices _redisCartServices;

        public CartController(IRedisCartServices redisCartServices)
        {
            _redisCartServices = redisCartServices;
        }
        [HttpGet]
        public async Task<ActionResult<Cart>> GetCartAsync(Guid cartId)
        {
            var cart = await _redisCartServices.GetCartAsync(cartId);
            return Ok(cart);
        }
        [HttpPost]
        public async Task<ActionResult<Cart>> UpdateCartAsync(Cart cart)
        {
            var updatedCart = await _redisCartServices.UpdateCartAsync(cart);
            return Ok(updatedCart);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteCartAsync(Guid cartId)
        {
            var result = await _redisCartServices.DeleteCartAsync(cartId);
            return Ok(result);
        }

    }
}
