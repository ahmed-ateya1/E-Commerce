using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.DealDto;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealController : ControllerBase
    {
        private readonly IDealService _dealService;
        private readonly ILogger<DealController> _logger;

        public DealController(IDealService dealService, ILogger<DealController> logger)
        {
            _dealService = dealService;
            _logger = logger;
        }
        /// <summary>
        /// Adds a new deal based on the provided request data.
        /// </summary>
        /// <param name="request">An object of type <see cref="DealAddRequest"/> containing the details of the deal to be added.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> indicating success or failure. 
        /// If the deal creation fails, a BadRequest response is returned, otherwise, the created deal details are returned.
        /// </returns>
        /// <remarks>
        /// This method validates the deal request and uses the <see cref="IDealService"/> to create the deal. 
        /// The deal details are then saved in the system and returned in the response.
        /// </remarks>
        /// <response code="200">Deal added successfully.</response>
        /// <response code="400">Failed to add the deal or invalid data.</response>
        [HttpPost("addDeal")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> AddDeal([FromBody] DealAddRequest request)
        {
            _logger.LogInformation("enter to add deal action");

            var response = await _dealService.CreateAsync(request);
            if (response == null)
            {
                _logger.LogError("Failed to add Deals.");
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = response.Message,
                    StatusCode = HttpStatusCode.BadRequest,
                    Result = null
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                StatusCode = HttpStatusCode.OK,
                Result = response.Result
            });
        }
        /// <summary>
        /// Updates an existing deal based on the provided request data.
        /// </summary>
        /// <param name="request">An object of type <see cref="DealUpdateRequest"/> containing the updated details for the deal.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> indicating success or failure. 
        /// If the deal update fails, a BadRequest response is returned, otherwise, the updated deal details are returned.
        /// </returns>
        /// <remarks>
        /// This method validates the deal update request and uses the <see cref="IDealService"/> to update the deal. 
        /// The updated deal details are then saved in the system and returned in the response.
        /// </remarks>
        /// <response code="200">Deal updated successfully.</response>
        /// <response code="400">Failed to update the deal or invalid data.</response>
        [HttpPut("updateDeal")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> UpdateDeal([FromBody] DealUpdateRequest request)
        {
            _logger.LogInformation("enter to update deal action");
            var response = await _dealService.UpdateAsync(request);
            if (response == null)
            {
                _logger.LogError("Failed to update Deals.");
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = response.Message,
                    StatusCode = HttpStatusCode.BadRequest,
                    Result = null
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                StatusCode = HttpStatusCode.OK,
                Result = response.Result
            });
        }

        /// <summary>
        /// Deletes a deal based on the provided deal ID.
        /// </summary>
        /// <param name="id">The unique identifier of the deal to be deleted.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> indicating success or failure. 
        /// If the deal deletion fails, a BadRequest response is returned, otherwise, a success message is returned.
        /// </returns>
        /// <remarks>
        /// This method uses the <see cref="IDealService"/> to delete the deal from the system. 
        /// The deal is removed, and the appropriate response is returned.
        /// </remarks>
        /// <response code="200">Deal deleted successfully.</response>
        /// <response code="400">Failed to delete the deal or invalid ID.</response>
        [HttpDelete("deleteDeal/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> DeleteDeal(Guid id)
        {
            _logger.LogInformation("enter to delete deal action");
            var response = await _dealService.DeleteAsync(id);
            if (response == null)
            {
                _logger.LogError("Failed to delete Deals.");
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = response.Message,
                    StatusCode = HttpStatusCode.BadRequest,
                    Result = null
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                StatusCode = HttpStatusCode.OK,
                Result = response.Result
            });
        }
        /// <summary>
        /// Fetches a specific deal by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the deal to be fetched.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> containing the deal details if found, or an error message if not.
        /// </returns>
        /// <remarks>
        /// This method retrieves the deal using the <see cref="IDealService"/>. If the deal is found, the details are returned; 
        /// otherwise, an error message is returned.
        /// </remarks>
        /// <response code="200">Deal retrieved successfully.</response>
        /// <response code="400">Failed to retrieve the deal or invalid ID.</response>
        [HttpGet("getDeal/{id}")]
        public async Task<ActionResult<ApiResponse>> GetDeal(Guid id)
        {
            _logger.LogInformation("enter to get deal action");
            var response = await _dealService.GetByAsync(x => x.DealID == id);
            if (response == null)
            {
                _logger.LogError("Failed to get Deals.");
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = response.Message,
                    StatusCode = HttpStatusCode.BadRequest,
                    Result = null
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message,
                StatusCode = HttpStatusCode.OK,
                Result = response.Result
            });
        }
        /// <summary>
        /// Fetches all deals available in the system.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> containing a list of all deals.
        /// </returns>
        /// <remarks>
        /// This method retrieves all deals using the <see cref="IDealService"/>. 
        /// A collection of all deals is returned in the response.
        /// </remarks>
        /// <response code="200">All deals retrieved successfully.</response>
        /// <response code="400">Failed to retrieve the deals.</response>
        [HttpGet("getAllDeals")]
        public async Task<ActionResult<ApiResponse>> GetAllDeals([FromQuery] PaginationDto paginationDto)
        {
            _logger.LogInformation("enter to get all deals action");
            var response = await _dealService.GetAllAsync(pagination:paginationDto);
            if (response == null)
            {
                _logger.LogError("Failed to get Deals.");
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "occour problem when try to fetch Deals",
                    StatusCode = HttpStatusCode.BadRequest,
                    Result = null
                });
            }
            return Ok(new ApiResponse()
            {
                Result = response,
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }
        /// <summary>
        /// Fetches all active deals available in the system.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> containing a list of all active deals.
        /// </returns>
        /// <remarks>
        /// This method retrieves all deals using the <see cref="IDealService"/> and filters them to include only active deals.
        /// A collection of active deals is returned in the response.
        /// </remarks>
        /// <response code="200">All active deals retrieved successfully.</response>
        /// <response code="400">Failed to retrieve the active deals.</response>
        [HttpGet("getAllDealsActive")]
        public async Task<ActionResult<ApiResponse>> GetAllDealsActive([FromQuery]PaginationDto paginationDto)
        {
            _logger.LogInformation("enter to get all deals active action");
            var response = await _dealService.GetAllAsync(pagination:paginationDto);

            response.Items = response.Items.Where(x => x.IsActive);
            if (response == null)
            {
                _logger.LogError("Failed to get Deals.");
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "occour problem when try to fetch Deals",
                    StatusCode = HttpStatusCode.BadRequest,
                    Result = null
                });
            }
            return Ok(new ApiResponse()
            {
                Result = response,
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true
            });
        }
    }
}
