using E_Commerce.Core.Commands.CategoryCommand;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.CategoryDto;
using E_Commerce.Core.Queries.CategoryQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce.API.Controllers
{
    /// <summary>
    /// API Controller for managing categories.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ILogger<CategoryController> _logger;
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="mediator">The mediator instance for handling requests.</param>
        /// <param name="logger">The logger instance for logging information.</param>

        public CategoryController(IMediator mediator, ILogger<CategoryController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="command">The details of the category to be added.</param>
        /// <returns>An API response with the created category or an error message.</returns>
        /// <response code="200">Category added successfully.</response>
        /// <response code="400">Failed to add the category.</response>
        [HttpPost("addCategory")]
        public async Task<ActionResult<ApiResponse>> AddCategory([FromForm] CategoryAddRequest command)
        {
            _logger.LogInformation("Attempting to add a new category: {CategoryName}", command.CategoryName);
            var response = await _mediator.Send(new CreateCategoryCommand(command));
            if (response != null)
            {
                return Ok(new ApiResponse
                {
                    Result = response,
                    Message = "Category added successfully.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }
            return BadRequest(new ApiResponse
            {
                Message = "Failed to add category.",
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }
        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="command">The updated category details.</param>
        /// <returns>An API response with the updated category or an error message.</returns>
        /// <response code="200">Category updated successfully.</response>
        /// <response code="400">Failed to update the category.</response>
        [HttpPut("updateCategory")]
        public async Task<ActionResult<ApiResponse>> UpdateCategory([FromForm] CategoryUpdateRequest command)
        {
            _logger.LogInformation("Attempting to update category: {CategoryName}", command.CategoryName);
            var response = await _mediator.Send(new UpdateCategoryCommand(command));
            if (response != null)
            {
                return Ok(new ApiResponse
                {
                    Result = response,
                    Message = "Category updated successfully.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }
            return BadRequest(new ApiResponse
            {
                Message = "Failed to update category.",
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }
        /// <summary>
        /// Deletes a category by ID.
        /// </summary>
        /// <param name="id">The ID of the category to be deleted.</param>
        /// <returns>An API response indicating success or failure.</returns>
        /// <response code="200">Category deleted successfully.</response>
        /// <response code="400">Failed to delete the category.</response>
        [HttpDelete("deleteCategory/{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteCategory(Guid id)
        {
            _logger.LogInformation("Attempting to delete category {id}", id);

            var response = await _mediator.Send(new DeleteCategoryCommand(id));
            if (response)
            {
                return Ok(new ApiResponse
                {
                    Result = response,
                    Message = "Category deleted successfully.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }
            return BadRequest(new ApiResponse
            {
                Message = "Failed to delete category.",
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }
        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>An API response with the list of categories or an error message.</returns>
        /// <response code="200">Categories retrieved successfully.</response>
        /// <response code="400">Failed to retrieve the categories.</response>
        [HttpGet("getCategories")]
        public async Task<ActionResult<ApiResponse>> GetCategories()
        {
            _logger.LogInformation("Attempting to get all categories");
            var response = await _mediator.Send(new GetAllCategoryQuery());
            if (response != null)
            {
                return Ok(new ApiResponse
                {
                    Result = response,
                    Message = "Categories retrieved successfully.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }
            return BadRequest(new ApiResponse
            {
                Message = "Failed to retrieve categories.",
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }
        /// <summary>
        /// Retrieves a category by ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>An API response with the category details or an error message.</returns>
        /// <response code="200">Category retrieved successfully.</response>
        /// <response code="400">Failed to retrieve the category.</response>
        [HttpGet("getCategory/{id}")]
        public async Task<ActionResult<ApiResponse>> GetCategory(Guid id)
        {
            _logger.LogInformation("Attempting to get category {id}", id);
            var response = await _mediator.Send(new GetCategoryQuery(id));
            if (response != null)
            {
                return Ok(new ApiResponse
                {
                    Result = response,
                    Message = "Category retrieved successfully.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }
            return BadRequest(new ApiResponse
            {
                Message = "Failed to retrieve category.",
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }

        /// <summary>
        /// Retrieves categories by parent ID.
        /// </summary>
        /// <param name="parentId">The ID of the parent category.</param>
        /// <returns>An API response with the list of categories or an error message.</returns>
        /// <response code="200">Categories retrieved successfully by parent.</response>
        /// <response code="400">Failed to retrieve categories by parent.</response>
        [HttpGet("getCategoriesforParent/{parentId}")]
        public async Task<ActionResult<ApiResponse>> GetCategoriesforParent(Guid parentId)
        {
            _logger.LogInformation("Attempting to get categories for parent with id = {parentId}", parentId);
            var response = await _mediator.Send(new GetAllCategoryForParentQuery(parentId));
            if (response != null)
            {
                return Ok(new ApiResponse
                {
                    Result = response,
                    Message = "Categories retrieved successfully by parent.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }
            return BadRequest(new ApiResponse
            {
                Message = "Failed to retrieve categories by parent.",
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }
        

        /// <summary>
        /// Retrieves all parent categories.
        /// </summary>
        /// <remarks>
        /// This endpoint fetches a list of all parent categories available in the system.
        /// </remarks>
        /// <returns>
        /// An <see cref="ActionResult"/> containing an <see cref="ApiResponse"/>:
        /// - If successful, the response will include the list of parent categories, a success message, and a status code of 200 (OK).
        /// - If unsuccessful, the response will include a failure message and a status code of 400 (Bad Request).
        /// </returns>
        /// <response code="200">Parent categories retrieved successfully.</response>
        /// <response code="400">Failed to retrieve parent categories.</response>
        [HttpGet("getCategoriesParent")]
        public async Task<ActionResult<ApiResponse>> GetCategoriesParent()
        {
            _logger.LogInformation("Attempting to get all parent categories");
            var response = await _mediator.Send(new GetAllParentCategoryQuery());
            if (response != null)
            {
                return Ok(new ApiResponse
                {
                    Result = response,
                    Message = "Parent Categories retrieved successfully.",
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK
                });
            }
            return BadRequest(new ApiResponse
            {
                Message = "Failed to retrieve parent categories.",
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }

    }
}
