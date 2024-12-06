using E_Commerce.Core.Commands.BrandCommand;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.BrandDto;
using E_Commerce.Core.Queries.BrandQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace E_Commerce.API.Controllers
{
    /// <summary>
    /// Controller to manage brand-related operations in the E-Commerce system.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<BrandController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrandController"/> class.
        /// </summary>
        /// <param name="unitOfWork">Instance of <see cref="IUnitOfWork"/> for database operations.</param>
        /// <param name="mediator">Instance of <see cref="IMediator"/> for handling commands and queries.</param>
        /// <param name="logger">Logger instance for logging actions.</param>
        public BrandController(IUnitOfWork unitOfWork, IMediator mediator, ILogger<BrandController> logger)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new brand.
        /// </summary>
        /// <param name="brand">The <see cref="BrandAddRequest"/> containing the brand details.</param>
        /// <response code="200">Brand created successfully.</response>
        /// <response code="400">Failed to create the brand.</response>
        /// <returns>An API response indicating the result of the brand creation.</returns>
        [HttpPost("addBrand")]
        public async Task<ActionResult<ApiResponse>> AddBrand([FromBody] BrandAddRequest brand)
        {
            _logger.LogInformation("Attempting to add a new brand: {BrandName}", brand.BrandName);

            var response = await _mediator.Send(new CreateBrandCommand(brand));

            if (response != null)
            {
                _logger.LogInformation("Brand created successfully: {BrandName}", brand.BrandName);
                return Ok(new ApiResponse
                {
                    Result = response,
                    Message = "Brand added successfully.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }

            _logger.LogWarning("Failed to add brand: {BrandName}", brand.BrandName);
            return BadRequest(new ApiResponse
            {
                Message = "Failed to add brand.",
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }

        /// <summary>
        /// Updates an existing brand.
        /// </summary>
        /// <param name="brand">The <see cref="BrandUpdateRequest"/> containing updated brand details.</param>
        /// <response code="200">Brand updated successfully.</response>
        /// <response code="404">Brand not found.</response>
        /// <response code="400">Failed to update the brand.</response>
        /// <returns>An API response indicating the result of the brand update.</returns>
        [HttpPut("updateBrand")]
        public async Task<ActionResult<ApiResponse>> UpdateBrand([FromBody] BrandUpdateRequest brand)
        {
            _logger.LogInformation("Attempting to update brand with ID: {BrandID}", brand.BrandID);

            var brandFound = await _unitOfWork.Repository<Brand>().GetByAsync(x => x.BrandID == brand.BrandID);
            if (brandFound == null)
            {
                _logger.LogWarning("Brand not found for update: {BrandID}", brand.BrandID);
                return NotFound(new ApiResponse
                {
                    Message = "Brand not found.",
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound
                });
            }

            var response = await _mediator.Send(new UpdateBrandCommand(brand));
            if (response != null)
            {
                _logger.LogInformation("Brand updated successfully: {BrandID}", brand.BrandID);
                return Ok(new ApiResponse
                {
                    Result = response,
                    Message = "Brand updated successfully.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }

            _logger.LogWarning("Failed to update brand: {BrandID}", brand.BrandID);
            return BadRequest(new ApiResponse
            {
                Message = "Failed to update brand.",
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }

        /// <summary>
        /// Deletes a brand by its ID.
        /// </summary>
        /// <param name="id">The ID of the brand to delete.</param>
        /// <response code="200">Brand deleted successfully.</response>
        /// <response code="404">Brand not found.</response>
        /// <response code="400">Failed to delete the brand.</response>
        /// <returns>An API response indicating the result of the brand deletion.</returns>
        [HttpDelete("deleteBrand/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteBrand(Guid id)
        {
            _logger.LogInformation("Attempting to delete brand with ID: {BrandID}", id);

            var brand = await _unitOfWork.Repository<Brand>().GetByAsync(x => x.BrandID == id);
            if (brand == null)
            {
                _logger.LogWarning("Brand not found for deletion: {BrandID}", id);
                return NotFound(new ApiResponse
                {
                    Message = "Brand not found.",
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.NotFound
                });
            }

            var response = await _mediator.Send(new DeleteBrandCommand(id));
            if (response)
            {
                _logger.LogInformation("Brand deleted successfully: {BrandID}", id);
                return Ok(new ApiResponse
                {
                    Message = "Brand deleted successfully.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }

            _logger.LogWarning("Failed to delete brand: {BrandID}", id);
            return BadRequest(new ApiResponse
            {
                Message = "Failed to delete brand.",
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }

        /// <summary>
        /// Retrieves a brand by its ID.
        /// </summary>
        /// <param name="id">The ID of the brand to retrieve.</param>
        /// <response code="200">Brand retrieved successfully.</response>
        /// <response code="404">Brand not found.</response>
        /// <returns>An API response containing the retrieved brand details.</returns>
        [HttpGet("getBrand/{id}")]
        public async Task<ActionResult<ApiResponse>> GetBrand(Guid id)
        {
            _logger.LogInformation("Fetching brand with ID: {BrandID}", id);

            var brand = await _mediator.Send(new GetBrandQuery(id));
            if (brand != null)
            {
                _logger.LogInformation("Brand retrieved successfully: {BrandID}", id);
                return Ok(new ApiResponse
                {
                    Result = brand,
                    Message = "Brand found successfully.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }

            _logger.LogWarning("Brand not found: {BrandID}", id);
            return NotFound(new ApiResponse
            {
                Message = "Brand not found.",
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound
            });
        }
        /// <summary>
        /// Retrieves all brands.
        /// </summary>
        /// <param name="paginationDto">Contain pagination index and pagination size</param>
        /// <response code="200">Brands retrieved successfully.</response>
        /// <response code="404">No brands found matching the name.</response>
        /// <returns>An API response containing the list of brands matching the name.</returns>
        [HttpGet("getAllBrands")]
        public async Task<ActionResult<ApiResponse>> GetAllBrands([FromQuery]PaginationDto paginationDto)
        {
            _logger.LogInformation("Fetching all brands");
            var brands = await _mediator.Send(new GetAllBrandQuery(paginationDto));
            if (brands != null)
            {
                _logger.LogInformation("Brands retrieved successfully");
                return Ok(new ApiResponse
                {
                    Result = brands,
                    Message = "Brands found successfully.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }
            _logger.LogWarning("No brands found");
            return NotFound(new ApiResponse
            {
                Message = "No brands found.",
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound
            });
        }
        /// <summary>
        /// Retrieves brands by their name.
        /// </summary>
        /// <param name="name">The name of the brand to search for.</param>
        /// <response code="200">Brands retrieved successfully.</response>
        /// <response code="404">No brands found matching the name.</response>
        /// <returns>An API response containing the list of brands matching the name.</returns>
        [HttpGet("getBrandByName/{name}")]
        public async Task<ActionResult<ApiResponse>> GetBrandByName(string name)
        {
            _logger.LogInformation("Fetching brands by name: {BrandName}", name);

            var brands = await _mediator.Send(new GetAllBrandQuery(new PaginationDto(), x => x.BrandName.ToUpper().Contains(name.ToUpper())));
            if (brands != null)
            {
                _logger.LogInformation("Brands retrieved successfully by name: {BrandName}", name);
                return Ok(new ApiResponse
                {
                    Result = brands,
                    Message = "Brands found successfully.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }

            _logger.LogWarning("No brands found matching name: {BrandName}", name);
            return NotFound(new ApiResponse
            {
                Message = "Brand not found.",
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound
            });
        }
    }
}
