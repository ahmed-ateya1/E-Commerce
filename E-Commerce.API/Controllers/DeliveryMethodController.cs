using E_Commerce.Core.Commands.DeliveryMethodCommand;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.DeliveryMethodDto;
using E_Commerce.Core.Queries.DeliveryMethodQueries;
using E_Commerce.Core.ServicesContract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryMethodController : ControllerBase
    {
        private readonly IDeliveryMethodServices _deliveryMethodServices;
        private readonly IMediator _mediator;
        private readonly ILogger<DeliveryMethodController> _logger;

        public DeliveryMethodController(IDeliveryMethodServices deliveryMethodServices, ILogger<DeliveryMethodController> logger, IMediator mediator)
        {
            _deliveryMethodServices = deliveryMethodServices;
            _logger = logger;
            _mediator = mediator;
        }
        /// <summary>
        /// Adds a new delivery method.
        /// </summary>
        /// <param name="request">An object of type <see cref="DeliveryMethodAddRequest"/> containing the details for the new delivery method to be added.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> with status code 200 (OK) if the delivery method is successfully added.
        /// If the creation fails, returns a 400 (Bad Request) with an error message.
        /// </returns>
        /// <remarks>
        /// This method is used to add a new delivery method to the system. It uses the <see cref="IDeliveryMethodServices"/> to handle the creation logic. 
        /// A success message is returned if the delivery method is added, otherwise, an error message is returned.
        /// </remarks>
        [HttpPost("addDeliveryMethod")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> AddDeliveryMethodAsync(DeliveryMethodAddRequest request)
        {
            _logger.LogInformation("AddDeliveryMethodAsync called.");
            var response = await _mediator.Send(new CreateDeliveryMethodCommand(request));
            if (response == null)
            {
                _logger.LogError("Response is null");
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Failed to add delivery method.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = true,
                Message = "Delivery method added successfully.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Updates an existing delivery method.
        /// </summary>
        /// <param name="request">An object of type <see cref="DeliveryMethodUpdateRequest"/> containing the details for the delivery method to be updated.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> with status code 200 (OK) if the delivery method is successfully updated.
        /// If the update fails, returns a 400 (Bad Request) with an error message.
        /// </returns>
        /// <remarks>
        /// This method is used to update the details of an existing delivery method. It uses the <see cref="IDeliveryMethodServices"/> to handle the update logic.
        /// A success message is returned if the update is successful, otherwise, an error message is returned.
        /// </remarks>
        [HttpPut("updateDeliveryMethod")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> UpdateDeliveryMethodAsync(DeliveryMethodUpdateRequest request)
        {
            _logger.LogInformation("UpdateDeliveryMethodAsync called.");
            var response = await _mediator.Send(new UpdateDeliveryMethodCommand(request));
            if (response == null)
            {
                _logger.LogError("Response is null");
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Failed to update delivery method.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = true,
                Message = "Delivery method updated successfully.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Deletes a specific delivery method by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the delivery method to delete.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> with status code 200 (OK) if the delivery method is successfully deleted.
        /// If the deletion fails, returns a 400 (Bad Request) with an error message.
        /// </returns>
        /// <remarks>
        /// This method deletes a delivery method by its ID from the system.
        /// A success message is returned if the delivery method is deleted, otherwise, an error message is returned.
        /// </remarks>
        [HttpDelete("deleteDeliveryMethod/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> DeleteDeliveryMethodAsync(Guid id)
        {
            _logger.LogInformation("DeleteDeliveryMethodAsync called.");
            var response = await _mediator.Send(new DeleteDeliveryMethodCommand() { ID = id });
            if (!response)
            {
                _logger.LogError("Failed to delete delivery method.");
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Failed to delete delivery method.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = true,
                Message = "Delivery method deleted successfully.",
                StatusCode = HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Retrieves the details of a specific delivery method by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the delivery method to retrieve.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> with status code 200 (OK) and the details of the delivery method if found.
        /// If the delivery method is not found, returns a 404 (Not Found) with an error message.
        /// </returns>
        /// <remarks>
        /// This method retrieves the details of the delivery method with the specified ID.
        /// If the delivery method is found, its details are returned, otherwise a 404 (Not Found) response is returned.
        /// </remarks>
        [HttpGet("getDeliveryMethod/{id}")]
        public async Task<ActionResult<ApiResponse>> GetDeliveryMethodAsync(Guid id)
        {
            _logger.LogInformation("GetDeliveryMethodAsync called.");
            var response = await _mediator.Send(new GetDeliveryMethodByIdQuery() { ID = id});
            if (response == null)
            {
                _logger.LogError("Delivery method not found.");
                return NotFound(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Delivery method not found.",
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = true,
                Message = "Delivery method found.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all delivery methods in the system.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> with status code 200 (OK) and a list of all delivery methods if found.
        /// If no delivery methods are found, returns a 404 (Not Found) with an error message.
        /// </returns>
        /// <remarks>
        /// This method fetches all delivery methods from the system.
        /// If no delivery methods are found, a 404 (Not Found) response is returned.
        /// Otherwise, the list of delivery methods is returned within an <see cref="ApiResponse"/> with a success message.
        /// </remarks>
        [HttpGet("getAllDeliveryMethods")]
        public async Task<ActionResult<ApiResponse>> GetAllDeliveryMethodsAsync()
        {
            _logger.LogInformation("GetAllDeliveryMethodsAsync called.");
            var response = await _mediator.Send(new GetAllDeliveryMethodQuery());
            if (response == null)
            {
                _logger.LogError("No delivery methods found.");
                return NotFound(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "No delivery methods found.",
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = true,
                Message = "Delivery methods found.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }


    }
}
