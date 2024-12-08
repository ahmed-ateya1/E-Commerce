using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos.WishlistDto;
using E_Commerce.Core.Helper;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Security.Claims;

namespace E_Commerce.Core.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<WishlistService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WishlistService
            (
            IUnitOfWork unitOfWork,
            ILogger<WishlistService> logger,
            IMapper mapper
,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        private async Task<Product?> GetProductAsync(Guid productID)
        {
            var exists = await _unitOfWork.Repository<Product>()
                .GetByAsync(x => x.ProductID == productID , includeProperties: "Category,ProductImages");
            if (exists == null)
            {
                _logger.LogWarning("Product with ID {ProductID} not found.", productID);
                throw new ArgumentNullException(nameof(exists));
            }
            _logger.LogInformation("Checked if product with ID {ProductID} exists: {Exists}", productID, exists);
            return exists;
        }

        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await action();
                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("Transaction committed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the transaction. Rolling back.");
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("No user is authenticated.");
                throw new InvalidOperationException("User is not authenticated.");
            }

            var user = await _unitOfWork.Repository<ApplicationUser>()
                .GetByAsync(x => x.Email == email);

            if (user == null)
            {
                _logger.LogWarning("User not found: {email}", email);
                throw new ArgumentNullException(nameof(user));
            }

            return user;
        }
        public async Task<WishlistResponse?> CreateAsync(WishlistAddRequest? request)
        {
            if (request == null)
            {
                _logger.LogWarning("Request is null.");
                throw new ArgumentNullException(nameof(request));
            }

            ValidationHelper.ValidateModel(request);    

            _logger.LogInformation("Creating a new wishlist item for product with ID {ProductID}.", request.ProductID);

            var user = await GetCurrentUserAsync();
            var product = await GetProductAsync(request.ProductID);

            var wishlist = _mapper.Map<Wishlist>(request);
            wishlist.UserID = user.Id;
            wishlist.ProductID = product.ProductID;
            wishlist.Product = product;
            wishlist.User = user;

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Wishlist>().CreateAsync(wishlist);
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Wishlist item for product with ID {ProductID} created successfully.", request.ProductID);
            return _mapper.Map<WishlistResponse>(wishlist);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var wishlist = await _unitOfWork.Repository<Wishlist>()
                .GetByAsync(x => x.ProductID == id);

            if (wishlist == null)
            {
                _logger.LogWarning("Product item with ID {ProductID} not found.", id);
                return false;
            }
           await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Wishlist>().DeleteAsync(wishlist);
                await _unitOfWork.CompleteAsync();
            });
            _logger.LogInformation("Product item with ID {ProductID} deleted successfully.", id);
            return true;
        }

        public async Task<IEnumerable<WishlistResponse>> GetAllAsync
            (Expression<Func<Wishlist, bool>>? expression = null)
        {
            var wishlists = await _unitOfWork.Repository<Wishlist>()
               .GetAllAsync(
               expression ,
               includeProperties: "Product,Product.ProductImages,Product.Category"
               );

            if (wishlists == null || !wishlists.Any())
            {
                _logger.LogWarning("No wishlist items found.");
                return [];
            }

            return _mapper.Map<IEnumerable<WishlistResponse>>(wishlists);
        }

        public async Task<WishlistResponse?> GetByAsync(Expression<Func<Wishlist, bool>> expression, bool isTracked = false)
        {
            var wishlist = await _unitOfWork.Repository<Wishlist>()
                .GetByAsync(expression, isTracked: isTracked , includeProperties: "Product,Product.ProductImages,Product.Category");

            if (wishlist == null)
            {
                _logger.LogWarning("No wishlist item found.");
                return null;
            }
            _logger.LogInformation("Wishlist item found: {Wishlist}", wishlist);
            return _mapper.Map<WishlistResponse>(wishlist);

        }

        public async Task<WishlistResponse?> UpdateAsync(WishlistUpdateRequest? request)
        {
            if (request == null)
            {
                _logger.LogWarning("Request is null.");
                throw new ArgumentNullException(nameof(request));
            }

            ValidationHelper.ValidateModel(request);

            _logger.LogInformation("Updating wishlist item with ID {WishlistID}.", request.WishlistID);

            var product = await GetProductAsync(request.ProductID);
            var user = await GetCurrentUserAsync();

            var oldWishlist = await _unitOfWork.Repository<Wishlist>()
                .GetByAsync(x => x.WishlistID == request.WishlistID , includeProperties: "Product,Product.ProductImages,Product.Category");

            if (oldWishlist == null)
            {
                _logger.LogWarning("Wishlist item with ID {WishlistID} not found.", request.WishlistID);
                throw new ArgumentNullException(nameof(oldWishlist));
            }

            var wishlist = _mapper.Map(request, oldWishlist);

            await ExecuteWithTransactionAsync(async () =>
            {
                _logger.LogInformation("Updating wishlist item with ID {WishlistID}.", request.WishlistID);
                await _unitOfWork.Repository<Wishlist>().UpdateAsync(wishlist);
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Wishlist item with ID {WishlistID} updated successfully.", request.WishlistID);

            return _mapper.Map<WishlistResponse>(wishlist);

        }
    }
}
