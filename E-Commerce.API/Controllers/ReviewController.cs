using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ReviewDto;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Net;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(IReviewService reviewService, IUnitOfWork unitOfWork, ILogger<ReviewController> logger)
        {
            _reviewService = reviewService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Adds a review for a product.
        /// </summary>
        /// <param name="request">The review add request containing review details.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        /// <response code="200">Review added successfully.</response>
        /// <response code="400">Failed to add the review.</response>
        [HttpPost("addReview")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddReview([FromForm] ReviewAddRequest request)
        {
            _logger.LogInformation("Adding review");
            var response = await _reviewService.CreateAsync(request);
            if (response == null)
            {
                _logger.LogError("Failed to add review");
                return BadRequest(new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Failed to add review"
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Review added successfully",
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <param name="request">The review update request containing updated review details.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        /// <response code="200">Review updated successfully.</response>
        /// <response code="400">Failed to update the review.</response>
        [HttpPut("updateReview")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpdateReview([FromForm] ReviewUpdateRequest request)
        {
            _logger.LogInformation("Updating review");
            var response = await _reviewService.UpdateAsync(request);
            if (response == null)
            {
                _logger.LogError("Failed to update review");
                return BadRequest(new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Failed to update review"
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Review updated successfully",
                IsSuccess = true,
                Result = response
            });
        }

        /// <summary>
        /// Deletes a review by its ID.
        /// </summary>
        /// <param name="id">The ID of the review to be deleted.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        /// <response code="200">Review deleted successfully.</response>
        /// <response code="400">Failed to delete the review.</response>
        [HttpDelete("deleteReview/{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> DeleteReview(Guid id)
        {
            _logger.LogInformation("Deleting review");
            var response = await _reviewService.DeleteAsync(id);
            if (!response)
            {
                _logger.LogError("Failed to delete review");
                return BadRequest(new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Failed to delete review"
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Review deleted successfully",
                IsSuccess = true
            });
        }

        /// <summary>
        /// Retrieves a specific review by its ID.
        /// </summary>
        /// <param name="id">The ID of the review to retrieve.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the review details.</returns>
        /// <response code="200">Review retrieved successfully.</response>
        /// <response code="400">Failed to retrieve the review.</response>
        [HttpGet("getReview/{id}")]
        public async Task<ActionResult<ApiResponse>> GetReview(Guid id)
        {
            _logger.LogInformation("Getting review");
            var response = await _reviewService.GetByAsync(x => x.ReviewID == id);
            if (response == null)
            {
                _logger.LogError("Failed to get review");
                return BadRequest(new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Failed to get review"
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Review retrieved successfully",
                Result = response,
                IsSuccess = true
            });
        }

        /// <summary>
        /// Retrieves all reviews for a product.
        /// </summary>
        /// <param name="productID">The ID of the product for which reviews are to be fetched.</param>
        /// <param name="pagination">Pagination parameters for the review list.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the list of reviews for the product.</returns>
        /// <response code="200">Reviews retrieved successfully.</response>
        /// <response code="400">Failed to retrieve reviews.</response>
        [HttpGet("getAllReviews/{productID}")]
        public async Task<ActionResult<ApiResponse>> GetAllReviews(Guid productID, [FromQuery] PaginationDto pagination)
        {
            _logger.LogInformation("Getting all reviews");
            var response = await _reviewService.GetAllAsync(x => x.ProductID == productID,pagination);
            if (response == null)
            {
                _logger.LogError("Failed to get reviews");
                return BadRequest(new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Failed to get reviews"
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Reviews retrieved successfully",
                Result = response,
                IsSuccess = true
            });
        }

        /// <summary>
        /// Retrieves all replies to a specific review.
        /// </summary>
        /// <param name="reviewID">The ID of the review for which replies are to be fetched.</param>
        /// <param name="pagination">Pagination parameters for the reply list.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the list of replies to the review.</returns>
        /// <response code="200">Review replies retrieved successfully.</response>
        /// <response code="400">Failed to retrieve review replies.</response>
        [HttpGet("getReviewReplies/{reviewID}")]
        public async Task<ActionResult<ApiResponse>> GetReviewReplies(Guid reviewID, [FromQuery] PaginationDto pagination)
        {
            _logger.LogInformation("Getting review replies");
            var response = await _reviewService.GetAllAsync(x => x.ParentReviewID == reviewID,pagination);
            if (response == null)
            {
                _logger.LogError("Failed to get review replies");
                return BadRequest(new ApiResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Failed to get review replies"
                });
            }
            return Ok(new ApiResponse
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Review replies retrieved successfully",
                Result = response,
                IsSuccess = true
            });
        }
    }
}
