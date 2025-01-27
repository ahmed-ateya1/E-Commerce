using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ProductDto;
using E_Commerce.Core.Helper;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Security.Claims;

namespace E_Commerce.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;
        private readonly IProductImagesService _productImagesService;
        private readonly IUserContext _userContext;

        public ProductService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<ProductService> logger,
            IProductImagesService productImagesService,
            IUserContext userContext)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _productImagesService = productImagesService;
            _userContext = userContext;
        }

        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    _logger.LogInformation("Transaction started.");
                    await action();
                    await _unitOfWork.CommitTransactionAsync();
                    _logger.LogInformation("Transaction committed successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Transaction failed. Rolling back...");
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
        }
        private async Task HandleProductWishListAsync(IEnumerable<ProductResponse> products, Guid userId)
        {
            var productIds = products.Select(x => x.ProductID).ToList();

            var wishlists = await _unitOfWork.Repository<Wishlist>()
                .GetAllAsync(x => productIds.Contains(x.ProductID) && x.UserID == userId);

            var wishlistProductIds = new HashSet<Guid>(wishlists.Select(x => x.ProductID));

            foreach (var product in products)
            {
                product.ProductInWishlist = wishlistProductIds.Contains(product.ProductID);
            }
        }

        private string GeneratePromotionLabel(double discount)
        {
            if (discount >= 50)
            {
                return "Best Save";
            }
            else if (discount >= 35)
            {
                return "Save 35%";
            }
            else if (discount >= 15)
            {
                return "Save 15%";
            }
            else if (discount > 0)
            {
                return "Sale";
            }
            else
            {
                return "Regular Price";
            }
        }
        public async Task<ProductResponse?> CreateAsync(ProductAddRequest? request)
        {
            if (request == null)
            {
                _logger.LogWarning("CreateAsync called with a null request.");
                throw new ArgumentNullException(nameof(request));
            }

            _logger.LogInformation("Creating a new product with name: {ProductName}", request.ProductName);

            ValidationHelper.ValidateModel(request);

            var user = await _userContext.GetCurrentUserAsync();
            if (user == null)
            {
                _logger.LogWarning("User not found.");
                throw new ArgumentNullException(nameof(user));
            }
            var brand = await _unitOfWork.Repository<Brand>()
                .CheckEntityAsync(x => x.BrandID == request.BrandID, "Brand");
            var category = await _unitOfWork.Repository<Category>()
                .CheckEntityAsync(x => x.CategoryID == request.CategoryID, "Category");

            var product = _mapper.Map<Product>(request);
            product.UserID = user.Id;
            product.Brand = brand;
            product.User = user;
            product.Category = category;
            product.PromotionLabel = GeneratePromotionLabel(product.Discount);

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Product>().CreateAsync(product);
                await _unitOfWork.CompleteAsync();
            });

            var images = await _productImagesService.SaveImageAsync(product.ProductID, request.ProductFiles);
            var productResponse = _mapper.Map<ProductResponse>(product);
            productResponse.ProductFilesUrl = images.ToList();

            _logger.LogInformation("Product created successfully with ID: {ProductID}", product.ProductID);

            return productResponse;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting product with ID: {ProductID}", id);

            var product = await _unitOfWork.Repository<Product>()
                .GetByAsync(x => x.ProductID == id, includeProperties: "TechnicalSpecifications,ProductImages,Wishlists,Reviews");

            if (product == null)
            {
                _logger.LogWarning("Product not found with ID: {ProductID}", id);
                return false;
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                if (product.ProductImages?.Any() == true)
                {
                    await _productImagesService.DeleteAllImageAsync(product.ProductID);
                }
                if (product.TechnicalSpecifications?.Any() == true)
                {
                    await _unitOfWork.Repository<TechnicalSpecification>().RemoveRangeAsync(product.TechnicalSpecifications);
                }
                if (product.Wishlists?.Any() == true)
                {
                    await _unitOfWork.Repository<Wishlist>().RemoveRangeAsync(product.Wishlists);
                }

                await _unitOfWork.Repository<Product>().DeleteAsync(product);
            });

            _logger.LogInformation("Product deleted successfully with ID: {ProductID}", id);

            return true;
        }

        public async Task<PaginatedResponse<ProductResponse>> GetAllAsync(
            Expression<Func<Product, bool>>? predicate = null, PaginationDto? pagination = null)
        {
            pagination ??= new PaginationDto();
            _logger.LogInformation("Fetching all products with pagination: PageIndex={PageIndex}, PageSize={PageSize}",
                pagination.PageIndex, pagination.PageSize);

            
            var products = await _unitOfWork.Repository<Product>()
                .GetAllAsync(
                    predicate,
                    includeProperties: "Brand,Reviews,Category,User,ProductImages,Deals",
                    sortBy: pagination.SortBy,
                    sortDirection: pagination.SortDirection,
                    pageSize: pagination.PageSize,
                    pageIndex: pagination.PageIndex);

            long productCount = await _unitOfWork.Repository<Product>().CountAsync(predicate);

            if (!products.Any())
            {
                _logger.LogInformation("No products found matching the criteria.");
                return new PaginatedResponse<ProductResponse>
                {
                    PageIndex = pagination.PageIndex,
                    PageSize = pagination.PageSize,
                    Items = new List<ProductResponse>(),
                    TotalCount = 0
                };
            }
            var productsDeals = await _unitOfWork.Repository<Deal>()
                .GetAllAsync(includeProperties: "Product");

            var activeDeals = productsDeals.Where(x => x.IsActiveDeal()).ToList();
            var productResponses = _mapper.Map<List<ProductResponse>>(products);

            foreach (var productResponse in productResponses)
            {
                var deal = productsDeals.FirstOrDefault(x => x.ProductID == productResponse.ProductID);
                if (deal != null)
                {
                    productResponse.Discount = deal.Discount;
                    productResponse.PromotionLabel = GeneratePromotionLabel(deal.Discount);
                    productResponse.ProductPriceAfterDiscount = productResponse.ProductPrice - (productResponse.ProductPrice * (decimal)deal.Discount / 100);
                }
            }
            var user = await _userContext.GetCurrentUserAsync();

            if (user != null)
            {
                _logger.LogInformation("User is authenticated. Fetching wishlist for the user.");
                await HandleProductWishListAsync(productResponses, user.Id);
            }

            _logger.LogInformation("{TotalProducts} products fetched successfully.", productResponses.Count);

            return new PaginatedResponse<ProductResponse>
            {
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize,
                Items = productResponses,
                TotalCount = productCount
            };
        }

        public async Task<ProductResponse?> GetByAsync(
            Expression<Func<Product, bool>> predicate, bool isTracked = false)
        {
            _logger.LogInformation("Fetching a single product based on the provided predicate.");

            var product = await _unitOfWork.Repository<Product>()
                .GetByAsync(predicate, includeProperties: "Brand,Category,Reviews,User,ProductImages,Deals", isTracked: isTracked);

            if (product == null)
            {
                _logger.LogWarning("Product not found based on the predicate.");
                throw new ArgumentNullException(nameof(product));
            }
            if(product.Deals.Any())
            {
                product.Discount = product.Deals.First().Discount;
                product.PromotionLabel = GeneratePromotionLabel(product.Discount);
            }
            _logger.LogInformation("Product fetched successfully with ID: {ProductID}", product.ProductID);
            return _mapper.Map<ProductResponse>(product);
        }

        public async Task<ProductResponse?> UpdateAsync(ProductUpdateRequest? request)
        {
            if (request == null)
            {
                _logger.LogWarning("UpdateAsync called with a null request.");
                throw new ArgumentNullException(nameof(request));
            }
            var user = await _userContext.GetCurrentUserAsync();
            if (user == null)
            {
                _logger.LogWarning("User not found.");
                throw new ArgumentNullException(nameof(user));
            }
            _logger.LogInformation("Updating product with ID: {ProductID}", request.ProductID);

            ValidationHelper.ValidateModel(request);

            var oldProduct = await _unitOfWork.Repository<Product>()
                .GetByAsync(x => x.ProductID == request.ProductID, includeProperties: "Brand,Reviews,Category,User,ProductImages");

            if (oldProduct == null)
            {
                _logger.LogWarning("Product not found for update with ID: {ProductID}", request.ProductID);
                throw new ArgumentNullException(nameof(oldProduct));
            }
            if (request.ProductFiles!=null && oldProduct.ProductImages?.Any() == true)
            {
                await _productImagesService.DeleteAllImageAsync(oldProduct.ProductID);
                await _productImagesService.SaveImageAsync(oldProduct.ProductID, request.ProductFiles);
            }

            _mapper.Map(request, oldProduct);
            oldProduct.PromotionLabel = GeneratePromotionLabel(oldProduct.Discount);

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Product>().UpdateAsync(oldProduct);
                await _unitOfWork.CompleteAsync();
            });

            
            _logger.LogInformation("Product updated successfully with ID: {ProductID}", request.ProductID);

            return _mapper.Map<ProductResponse>(oldProduct);
        }
    }
}
