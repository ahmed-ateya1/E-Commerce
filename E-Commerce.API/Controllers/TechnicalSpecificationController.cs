using E_Commerce.Core.Commands.SpecificationCommand;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.TechnicalSpecificationDto;
using E_Commerce.Core.Queries.SpecificationQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce.API.Controllers
{
    /// <summary>
    /// API Controller for managing Technical Specification for product.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicalSpecificationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public TechnicalSpecificationController(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        /// <summary>
        /// Adds a new technical specification for a product.
        /// </summary>
        /// <param name="request">The request containing the details of the technical specification to add.</param>
        /// <returns>An API response indicating the result of the operation.</returns>
        /// <response code="200">Technical specification added successfully.</response>
        /// <response code="404">Product not found.</response>
        /// <response code="400">Failed to add the technical specification.</response>
        [HttpPost("addSpecification")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> AddSpecification([FromBody] TechnicalSpecificationAddRequest request)
        {
            var product = await _unitOfWork.Repository<Product>()
                .AnyAsync(x => x.ProductID == request.ProductID);
            if (!product)
            {
                return NotFound(new ApiResponse
                {
                    Message = "Product not found",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }

            var response = await _mediator.Send(new CreateSpecificationCommand(request));

            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Technical Specification not added",
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false
                });
            }

            return Ok(new ApiResponse
            {
                Message = "Technical Specification added successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Updates an existing technical specification.
        /// </summary>
        /// <param name="request">The request containing updated details for the technical specification.</param>
        /// <returns>An API response indicating the result of the operation.</returns>
        /// <response code="200">Technical specification updated successfully.</response>
        /// <response code="404">Product or technical specification not found.</response>
        /// <response code="400">Failed to update the technical specification.</response>
        [HttpPut("updateSpecification")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> UpdateSpecification([FromBody] TechnicalSpecificationUpdateRequest request)
        {
            var product = await _unitOfWork.Repository<Product>()
                .AnyAsync(x => x.ProductID == request.ProductID);
            if (!product)
            {
                return NotFound(new ApiResponse
                {
                    Message = "Product not found",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }

            var response = await _mediator.Send(new UpdateSpecificationCommand(request));
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Technical Specification not updated",
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false
                });
            }

            return Ok(new ApiResponse
            {
                Message = "Technical Specification updated successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Deletes a technical specification by its ID.
        /// </summary>
        /// <param name="id">The ID of the technical specification to delete.</param>
        /// <returns>An API response indicating the result of the operation.</returns>
        /// <response code="200">Technical specification deleted successfully.</response>
        /// <response code="400">Failed to delete the technical specification.</response>
        [HttpDelete("deleteSpecification/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> DeleteAsync(Guid id)
        {
            var response = await _mediator.Send(new DeleteSpecificationCommand(id));
            if (!response)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Technical Specification not deleted",
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccess = false
                });
            }

            return Ok(new ApiResponse
            {
                Message = "Technical Specification deleted successfully",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }

        /// <summary>
        /// Retrieves a technical specification by its ID.
        /// </summary>
        /// <param name="id">The ID of the technical specification to retrieve.</param>
        /// <returns>An API response containing the requested technical specification.</returns>
        /// <response code="200">Technical specification retrieved successfully.</response>
        /// <response code="404">Technical specification not found.</response>
        [HttpGet("getSpecification/{id}")]
        public async Task<ActionResult<ApiResponse>> GetByAsync(Guid id)
        {
            var response = await _mediator.Send(new GetSpecificationQuery(id));
            if (response == null)
            {
                return NotFound(new ApiResponse
                {
                    Message = "Technical Specification not found",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }

            return Ok(new ApiResponse
            {
                Message = "Technical Specification found",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all technical specifications for a specific product.
        /// </summary>
        /// <param name="productID">The ID of the product.</param>
        /// <returns>An API response containing the list of technical specifications.</returns>
        /// <response code="200">Technical specifications retrieved successfully.</response>
        /// <response code="404">Technical specifications not found.</response>
        [HttpGet("getAllSpecificationsForProduct/{productID}")]
        public async Task<ActionResult<ApiResponse>> GetAllSpecificationsForProduct(Guid productID)
        {
            var response = await _mediator.Send(new GetAllSpecificationsForProductQuery(productID));
            if (response == null)
            {
                return NotFound(new ApiResponse
                {
                    Message = "Technical Specifications not found",
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccess = false
                });
            }

            return Ok(new ApiResponse
            {
                Message = "Technical Specifications found",
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = response
            });
        }
    }
}
