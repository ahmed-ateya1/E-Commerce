using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Services;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    /// <summary>
    /// Provides endpoints for managing the shopping cart.
    /// Interacts with Redis to retrieve, update, and delete cart data.
    /// </summary>
    [ApiController]
    [Route("api/cart")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShoppingCartController"/> class.
        /// </summary>
        /// <param name="shoppingCartService">Service for shopping cart operations.</param>
        /// <param name="httpContextAccessor">Accessor for HTTP context to retrieve user information.</param>
        /// <param name="unitOfWork">Unit of work to manage repositories and database transactions.</param>
        public ShoppingCartController(
            IShoppingCartService shoppingCartService,
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork)
        {
            _shoppingCartService = shoppingCartService;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Retrieves the current user's shopping cart.
        /// </summary>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> containing the user's shopping cart.</returns>
        /// <remarks>
        /// This endpoint requires the user to be authenticated.
        /// </remarks>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCart(CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var user = await _unitOfWork.Repository<ApplicationUser>()
                .GetByAsync(x => x.Email == email);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var cart = await _shoppingCartService.GetCartAsync(user.Id.ToString(), cancellationToken);
            return Ok(cart);
        }

        /// <summary>
        /// Adds an item to the current user's shopping cart.
        /// </summary>
        /// <param name="item">The item to be added to the cart.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the operation result.</returns>
        /// <remarks>
        /// This endpoint requires the user to be authenticated.
        /// </remarks>
        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] CartItems item, CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var user = await _unitOfWork.Repository<ApplicationUser>()
                .GetByAsync(x => x.Email == email);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            await _shoppingCartService.AddToCartAsync(user.Id.ToString(), item, cancellationToken);
            return Ok(new { Message = "Item added to cart" });
        }

        /// <summary>
        /// Removes an item from the current user's shopping cart.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart item is to be removed.</param>
        /// <param name="productId">The ID of the product to be removed from the cart.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the operation result.</returns>
        /// <remarks>
        /// This endpoint requires the user to be authenticated.
        /// </remarks>
        [HttpDelete("remove/{productId}")]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(string userId, Guid productId, CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var user = await _unitOfWork.Repository<ApplicationUser>()
                .GetByAsync(x => x.Email == email);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            await _shoppingCartService.RemoveFromCartAsync(user.Id.ToString(), productId, cancellationToken);
            return Ok(new { Message = "Item removed from cart" });
        }

        /// <summary>
        /// Clears all items in the current user's shopping cart.
        /// </summary>
        /// <param name="userId">The ID of the user whose cart is to be cleared.</param>
        /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the operation result.</returns>
        /// <remarks>
        /// This endpoint requires the user to be authenticated.
        /// </remarks>
        [HttpDelete("clear")]
        [Authorize]
        public async Task<IActionResult> ClearCart(string userId, CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var user = await _unitOfWork.Repository<ApplicationUser>()
                .GetByAsync(x => x.Email == email);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            await _shoppingCartService.ClearCartAsync(user.Id.ToString(), cancellationToken);
            return Ok(new { Message = "Cart cleared" });
        }
    }
}

