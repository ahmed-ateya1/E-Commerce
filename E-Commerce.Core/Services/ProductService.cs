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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<ProductService> logger,
            IProductImagesService productImagesService,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _productImagesService = productImagesService;
            _httpContextAccessor = httpContextAccessor;
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

        private async Task<Brand> CheckBrandAsync(Guid brandID)
        {
            var brand = await _unitOfWork.Repository<Brand>()
                .GetByAsync(x => x.BrandID == brandID);
            if (brand == null)
            {
                _logger.LogWarning("Brand not found with ID: {BrandID}", brandID);
                throw new ArgumentNullException(nameof(brand));
            }
            return brand;
        }
        private async Task<Category> CheckCategoryAsync(Guid categoryID)
        {
            var category = await _unitOfWork.Repository<Category>()
                .GetByAsync(x => x.CategoryID == categoryID);
            if (category == null)
            {
                _logger.LogWarning("Category not found with ID: {CategoryID}", category);
             throw new ArgumentNullException(nameof(category));
            }
            return category;
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

            var user = await GetCurrentUserAsync();
            var brand = await CheckBrandAsync(request.BrandID);
            var category = await CheckCategoryAsync(request.CategoryID);

            var product = _mapper.Map<Product>(request);
            product.UserID = user.Id;
            product.Brand = brand;
            product.User = user;
            product.Category = category;

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
                    includeProperties: "Brand,Category,User,ProductImages",
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

            var productResponses = _mapper.Map<List<ProductResponse>>(products);

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
                .GetByAsync(predicate,includeProperties: "Brand,Category,User,ProductImages", isTracked: isTracked);

            if (product == null)
            {
                _logger.LogWarning("Product not found based on the predicate.");
                throw new ArgumentNullException(nameof(product));
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

            _logger.LogInformation("Updating product with ID: {ProductID}", request.ProductID);

            ValidationHelper.ValidateModel(request);

            var oldProduct = await _unitOfWork.Repository<Product>()
                .GetByAsync(x => x.ProductID == request.ProductID, includeProperties: "Brand,Category,User,ProductImages");

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
