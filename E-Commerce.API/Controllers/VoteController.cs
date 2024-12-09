using AutoMapper;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.VoteDto;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;
        private readonly ILogger<VoteController> _logger;
        private readonly IMapper _mapper;

        public VoteController(IVoteService voteService, ILogger<VoteController> logger, IMapper mapper)
        {
            _voteService = voteService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Upvotes a review.
        /// </summary>
        /// <param name="request">The vote request containing the review ID and vote details.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the upvote operation.</returns>
        /// <response code="200">Upvoted successfully.</response>
        /// <response code="400">Failed to upvote.</response>
        [HttpPost("upVote")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpVote([FromBody] VoteAddRequest request)
        {
            _logger.LogInformation("UpVote method called");
            var response = await _voteService.UpVoteAsync(request);

            if (response == null)
            {
                return Ok(new ApiResponse
                {
                    IsSuccess = true,
                    Message = "Upvoting removed successfully",
                    StatusCode = HttpStatusCode.OK
                });
            }
            _logger.LogInformation("Upvoted successfully");
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Upvoted successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Downvotes a review.
        /// </summary>
        /// <param name="request">The vote request containing the review ID and vote details.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the downvote operation.</returns>
        /// <response code="200">Downvoted successfully.</response>
        /// <response code="400">Failed to downvote.</response>
        [HttpPost("downVote")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> DownVote([FromBody] VoteAddRequest request)
        {
            _logger.LogInformation("DownVote method called");
            var response = await _voteService.DownVoteAsync(request);
            if (response == null)
            {
                return Ok(new ApiResponse
                {
                    IsSuccess = true,
                    Message = "Downvote removed successfully",
                    StatusCode = HttpStatusCode.OK
                });
            }
            _logger.LogInformation("Downvoted successfully");
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Downvoted successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all votes for a specific review.
        /// </summary>
        /// <param name="reviewId">The ID of the review for which votes are to be fetched.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the list of votes for the review.</returns>
        /// <response code="200">Votes retrieved successfully.</response>
        /// <response code="500">An error occurred while retrieving votes.</response>
        [HttpGet("getAll/{reviewId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetAll(Guid reviewId)
        {
            _logger.LogInformation("GetAll method called");
            var response = await _voteService.GetAllAsync(x => x.ReviewID == reviewId);
            if (response == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    IsSuccess = false,
                    Message = "An error occurred while getting votes",
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }
            _logger.LogInformation("Votes retrieved successfully");
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Votes retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }
    }
}
