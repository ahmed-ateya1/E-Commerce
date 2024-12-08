using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.WishlistDto;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    /// <summary>
    /// Controller for managing wishlist operations, such as adding, removing, and retrieving wishlist items.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        private readonly ILogger<WishlistController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// Initializes a new instance of the <see cref="WishlistController"/> class.
        /// </summary>
        /// <param name="wishlistService">Service for managing wishlist operations.</param>
        /// <param name="logger">Logger for logging operations and errors.</param>
        /// <param name="unitOfWork">Unit of work for database operations.</param>
        /// <param name="httpContextAccessor">Accessor for HTTP context.</param>
        public WishlistController(
            IWishlistService wishlistService,
            ILogger<WishlistController> logger,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _wishlistService = wishlistService;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Adds a product to the user's wishlist.
        /// </summary>
        /// <param name="request">The wishlist add request containing the product details.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        /// <response code="200">Product added to wishlist successfully.</response>
        /// <response code="400">Failed to add product to wishlist.</response>
        [HttpPost("addProductToWishlist")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddProductToWishList([FromBody] WishlistAddRequest request)
        {
            
            _logger.LogInformation("Adding product to wishlist");
            var response = await _wishlistService.CreateAsync(request);
            if (response == null)
            {
                _logger.LogError("Failed to add product to wishlist");
                return BadRequest(new ApiResponse
                {
                    Message = "Failed to add product to wishlist",
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false
                });
            }
            _logger.LogInformation("Product added to wishlist successfully");
            return Ok(new ApiResponse
            {
                Message = "Product added to wishlist successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Removes a product from the user's wishlist.
        /// </summary>
        /// <param name="productID">The ID of the product to remove from the wishlist.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        /// <response code="200">Product removed from wishlist successfully.</response>
        /// <response code="400">Failed to remove product from wishlist.</response>
        [HttpDelete("removeProductFromWishlist/{productID}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> RemoveProductFromWishlist(Guid productID)
        {
            _logger.LogInformation("Removing product from wishlist");
            var isDeleted = await _wishlistService.DeleteAsync(productID);
            if (!isDeleted)
            {
                _logger.LogError("Failed to remove product from wishlist");
                return BadRequest(new ApiResponse
                {
                    Message = "Failed to remove product from wishlist",
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false
                });
            }
            _logger.LogInformation("Product removed from wishlist successfully");
            return Ok(new ApiResponse
            {
                Message = "Product removed from wishlist successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = isDeleted
            });
        }

        /// <summary>
        /// Retrieves the wishlist of the authenticated user.
        /// </summary>
        /// <returns>An <see cref="ApiResponse"/> containing the user's wishlist.</returns>
        /// <response code="200">Wishlist retrieved successfully.</response>
        /// <response code="401">User is not authenticated.</response>
        /// <response code="404">User not found.</response>
        /// <response code="400">Failed to retrieve wishlist.</response>
        [HttpGet("getWishlist")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetWishlist()
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("No user is authenticated.");
                return Unauthorized(new ApiResponse
                {
                    Message = "User is not authenticated",
                    StatusCode = HttpStatusCode.Unauthorized,
                    IsSuccess = false
                });
            }
            var user = await _unitOfWork.Repository<ApplicationUser>()
                .GetByAsync(x => x.Email == email);
            if (user == null)
            {
                _logger.LogWarning("User not found: {email}", email);
                return NotFound(new ApiResponse
                {
                    Message = "User not found",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }
            _logger.LogInformation("Getting wishlist");
            var response = await _wishlistService.GetAllAsync(x => x.UserID == user.Id);
            if (response == null)
            {
                _logger.LogError("Failed to get wishlist");
                return BadRequest(new ApiResponse
                {
                    Message = "Failed to get wishlist",
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false
                });
            }
            _logger.LogInformation("Wishlist retrieved successfully");
            return Ok(new ApiResponse
            {
                Message = "Wishlist retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }
    }

}
