using E_Commerce.Core.Commands.ProductCommand;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ProductDto;
using E_Commerce.Core.Helper;
using E_Commerce.Core.Queries.ProductQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductController> _logger;
        private readonly IMediator _mediator;

        public ProductController(
            IUnitOfWork unitOfWork,
            ILogger<ProductController> logger,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="productAddRequest">The product details to add.</param>
        /// <returns>An API response indicating the result of the operation.</returns>
        /// <response code="200">Product added successfully.</response>
        /// <response code="400">Failed to add the product or invalid data.</response>
        [HttpPost("addProduct")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> AddProduct([FromForm] ProductAddRequest productAddRequest)
        {
            _logger.LogInformation("Starting AddProduct operation.");

            var category = await _unitOfWork.Repository<Category>().GetByAsync(x => x.CategoryID == productAddRequest.CategoryID);
            var brand = await _unitOfWork.Repository<Brand>().GetByAsync(x => x.BrandID == productAddRequest.BrandID);

            if (category == null || brand == null)
            {
                var missingEntity = category == null ? "Category" : "Brand";
                _logger.LogWarning($"{missingEntity} not found for AddProduct.");
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"{missingEntity} not found.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }

            var response = await _mediator.Send(new CreateProductCommand(productAddRequest));

            if (response == null)
            {
                _logger.LogError("Failed to add product.");
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to add product.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }

            _logger.LogInformation("Product added successfully.");
            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Product added successfully.",
                Result = response
            });
            
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="productUpdateRequest">The product details to update.</param>
        /// <returns>An API response indicating the result of the operation.</returns>
        /// <response code="200">Product updated successfully.</response>
        /// <response code="400">Failed to update the product or invalid data.</response>
        [HttpPut("updateProduct")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> UpdateProduct([FromForm] ProductUpdateRequest productUpdateRequest)
        {

            _logger.LogInformation("Starting UpdateProduct operation.");

            var category = await _unitOfWork.Repository<Category>().GetByAsync(x => x.CategoryID == productUpdateRequest.CategoryID);
            var brand = await _unitOfWork.Repository<Brand>().GetByAsync(x => x.BrandID == productUpdateRequest.BrandID);

            if (category == null || brand == null)
            {
                var missingEntity = category == null ? "Category" : "Brand";
                _logger.LogWarning($"{missingEntity} not found for UpdateProduct.");
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"{missingEntity} not found.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }

            var response = await _mediator.Send(new UpdateProductCommand(productUpdateRequest));

            if (response == null)
            {
                _logger.LogError("Failed to update product.");
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to update product.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }

            _logger.LogInformation("Product updated successfully.");
            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Product updated successfully.",
                Result = response
            });
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>An API response indicating the result of the operation.</returns>
        /// <response code="200">Product deleted successfully.</response>
        /// <response code="400">Failed to delete the product or invalid ID.</response>
        [HttpDelete("deleteProduct/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> DeleteProduct(Guid id)
        {

            _logger.LogInformation($"Starting DeleteProduct for ID {id}.");

            var response = await _mediator.Send(new DeleteProductCommand(id));

            if (!response)
            {
                _logger.LogError("Failed to delete product.");
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to delete product.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }

            _logger.LogInformation("Product deleted successfully.");
            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Product deleted successfully.",
                Result = response
            });
           
        }

        /// <summary>
        /// Retrieves a paginated list of all products.
        /// </summary>
        /// <param name="pagination">The pagination details including page number and page size.</param>
        /// <returns>
        /// An <see cref="ActionResult{ApiResponse}"/> containing the result of the operation:
        /// <list type="bullet">
        /// <item><description><see cref="HttpStatusCode.OK"/>: Products fetched successfully.</description></item>
        /// <item><description><see cref="HttpStatusCode.BadRequest"/>: Failed to fetch products.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Use this endpoint to get a paginated list of all products.
        /// </remarks>
        [HttpGet("getAllProducts")]
        public async Task<ActionResult<ApiResponse>> GetAllProducts([FromQuery] PaginationDto pagination)
        {
            var response = await _mediator.Send(new GetAllProductsQuery(pagination));
            if (response == null)
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to get products.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            return Ok(new ApiResponse()
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Products fetched successfully.",
                Result = response
            });
        }

        /// <summary>
        /// Retrieves the details of a specific product by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>
        /// An <see cref="ActionResult{ApiResponse}"/> containing the result of the operation:
        /// <list type="bullet">
        /// <item><description><see cref="HttpStatusCode.OK"/>: Product fetched successfully.</description></item>
        /// <item><description><see cref="HttpStatusCode.BadRequest"/>: Failed to fetch product.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Use this endpoint to get the details of a specific product by its ID.
        /// </remarks>
        [HttpGet("getProduct/{id}")]
        public async Task<ActionResult<ApiResponse>> GetProduct(Guid id)
        {
            var response = await _mediator.Send(new GetProductQuery() { Id = id });
            if (response == null)
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to get product.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            return Ok(new ApiResponse()
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Product fetched successfully.",
                Result = response
            });
        }

        /// <summary>
        /// Searches for products by name with optional pagination.
        /// </summary>
        /// <param name="name">The name or part of the name of the product to search for.</param>
        /// <param name="pagination">The pagination details including page number and page size.</param>
        /// <returns>
        /// An <see cref="ActionResult{ApiResponse}"/> containing the result of the operation:
        /// <list type="bullet">
        /// <item><description><see cref="HttpStatusCode.OK"/>: Products searched successfully.</description></item>
        /// <item><description><see cref="HttpStatusCode.BadRequest"/>: Failed to search products.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Use this endpoint to search for products by their name.
        /// </remarks>
        [HttpGet("searchProduct/{name}")]
        public async Task<ActionResult<ApiResponse>> SearchProduct(string name, [FromQuery] PaginationDto pagination)
        {
            var response = await _mediator.Send(new SearchProductQuery() { Name = name, Pagination = pagination });
            if (response == null)
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to search product.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            return Ok(new ApiResponse()
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Product searched successfully.",
                Result = response
            });
        }

        /// <summary>
        /// Retrieves a paginated list of products by category.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <param name="pagination">The pagination details including page number and page size.</param>
        /// <returns>
        /// An <see cref="ActionResult{ApiResponse}"/> containing the result of the operation:
        /// <list type="bullet">
        /// <item><description><see cref="HttpStatusCode.OK"/>: Products fetched by category successfully.</description></item>
        /// <item><description><see cref="HttpStatusCode.BadRequest"/>: Failed to fetch products by category.</description></item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Use this endpoint to get a paginated list of products within a specific category.
        /// </remarks>
        [HttpGet("getProductsByCategory/{categoryId}")]
        public async Task<ActionResult<ApiResponse>> GetProductByCategory(Guid categoryId, [FromQuery] PaginationDto pagination)
        {
            var response = await _mediator.Send(new GetProductByCategoryQuery(categoryId, pagination));
            if (response == null)
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to get product by category.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            return Ok(new ApiResponse()
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Product fetched by category successfully.",
                Result = response
            });
        }


        /// <summary>
        /// Fetches products by brand ID with pagination.
        /// </summary>
        /// <param name="brandId">Brand ID to filter products.</param>
        /// <param name="pagination">Pagination details.</param>
        /// <returns>Paginated list of products for the specified brand.</returns>
        [HttpGet("getProductsByBrand/{brandId}")]
        public async Task<ActionResult<ApiResponse>> GetProductByBrand(Guid brandId, [FromQuery] PaginationDto pagination)
        {

            var response = await _mediator.Send(new GetProductsByBrandQuery(brandId, pagination));
            if (response == null)
            {
                _logger.LogWarning("No products found for brand ID: {BrandId}", brandId);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "No products found for the specified brand.",
                    StatusCode = HttpStatusCode.NotFound
                });
            }

            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Products fetched successfully.",
                Result = response
            });
        }

        /// <summary>
        /// Fetches products within a specified price range.
        /// </summary>
        /// <param name="range">Price range filter.</param>
        /// <returns>List of products within the specified price range.</returns>
        [HttpGet("getProductsByPriceRange")]
        public async Task<ActionResult<ApiResponse>> GetProductByPriceRange([FromQuery] RangeDto range)
        {

            var response = await _mediator.Send(new GetProductsByPriceRangeQuery(range));
            if (response == null)
            {
                _logger.LogWarning("No products found within the price range: {Min} - {Max}", range.Min, range.Max);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "No products found within the specified price range.",
                    StatusCode = HttpStatusCode.NotFound
                });
            }

            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Products fetched successfully.",
                Result = response
            });

        }

        /// <summary>
        /// Fetches products by rating with pagination.
        /// </summary>
        /// <param name="rating">Rating filter (0-10).</param>
        /// <param name="pagination">Pagination details.</param>
        /// <returns>Paginated list of products with the specified rating.</returns>
        /// 
        [HttpGet("getProductsByRating/{rating:int:range(0,10)}")]
        public async Task<ActionResult<ApiResponse>> GetProductByRating(int rating, [FromQuery] PaginationDto pagination)
        {

            var response = await _mediator.Send(new GetProductsByRatingQuery(rating, pagination));
            if (response == null)
            {
                _logger.LogWarning("No products found with rating: {Rating}", rating);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "No products found with the specified rating.",
                    StatusCode = HttpStatusCode.NotFound
                });
            }

            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Products fetched successfully.",
                Result = response
            });
           
        }
        /// <summary>
        /// Fetches products by Category Name with pagination.
        /// </summary>
        /// <param name="name">represent category name.</param>
        /// <param name="pagination">Pagination details.</param>
        /// <returns>Paginated list of products with the specified category.</returns>
        [HttpGet("getProductsByCategoryName/{name}")]
        public async Task<ActionResult<ApiResponse>> GetProductByCategoryName(string name, [FromQuery] PaginationDto pagination)
        {
            var response = await _mediator.Send(new GetProductsByCategoryNameQuery(name, pagination));
            if (response == null)
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to get product by category name.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            return Ok(new ApiResponse()
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Product fetched by category name successfully.",
                Result = response
            });
        }
        /// <summary>
        /// Fetches products by Brand Name with pagination.
        /// </summary>
        /// <param name="name">represent Brand name.</param>
        /// <param name="pagination">Pagination details.</param>
        /// <returns>Paginated list of products with the specified Brand.</returns>
        [HttpGet("getProductsByBrandName/{name}")]
        public async Task<ActionResult<ApiResponse>> GetProductByBrandName(string name, [FromQuery] PaginationDto pagination)
        {
            var response = await _mediator.Send(new GetProductsByBrandNameQuery(name, pagination));
            if (response == null)
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to get product by brand name.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            return Ok(new ApiResponse()
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Product fetched by brand name successfully.",
                Result = response
            });
        }


    }
}
