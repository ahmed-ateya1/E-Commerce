using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.OrderDto;
using E_Commerce.Core.Helper;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IOrderServices orderServices, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _orderServices = orderServices;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Adds a new order based on the provided request data.
        /// </summary>
        /// <param name="request">An object of type <see cref="OrderAddRequest"/> containing the details for the new order to be added. 
        /// This includes the user's address ID, delivery method ID, and a list of items in the order.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> indicating success or failure. 
        /// If the order creation fails, a BadRequest response is returned, otherwise the created order details are returned.
        /// </returns>
        /// <remarks>
        /// This method validates the order request and uses the <see cref="OrderServices"/> to create the order. 
        /// The order details are then saved in the system and returned in the response.
        /// </remarks>
        /// <response code="200">Order added successfully.</response>
        /// <response code="400">Failed to add the Order or invalid data.</response>
        [HttpPost("addOrder")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddOrder(OrderAddRequest request)
        {
            var response = await _orderServices.CreateAsync(request);
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Failed to add order",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(response);
        }
        /// <summary>
        /// Updates the status of an existing order.
        /// </summary>
        /// <param name="orderID">The unique identifier of the order to update.</param>
        /// <param name="orderStatus">An object containing the new status to be applied to the order.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> indicating success or failure of the operation.
        /// A BadRequest response is returned if the update fails, otherwise a success message is returned.
        /// </returns>
        /// <remarks>
        /// This method updates the order status based on the provided order ID. 
        /// If the order status update is successful, a success message is returned.
        /// </remarks>

        [HttpPut("updateOrderStatus")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> UpdateOrderStatus(Guid orderID, [FromBody] OrderStatus orderStatus)
        {
            var response = await _orderServices.UpdateAsync(orderID, orderStatus);
            if (!response)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Failed to update order status",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Order status updated successfully",
                StatusCode = HttpStatusCode.OK
            });
        }
        /// <summary>
        /// Deletes a specific order by its ID.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to delete.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> indicating the result of the operation.
        /// A BadRequest response is returned if the deletion fails, otherwise a success message is returned.
        /// </returns>
        /// <remarks>
        /// This method deletes the order with the given ID from the system.
        /// If the deletion is successful, a success message is returned; otherwise, an error message is returned.
        /// </remarks>
        /// <response code="200">Order deleted successfully.</response>
        /// <response code="400">Failed to delete the order.</response>
        [HttpDelete("deleteOrder/{orderId}")]
        public async Task<ActionResult<ApiResponse>> DeleteOrder(Guid orderId)
        {
            var response = await _orderServices.DeleteAsync(orderId);
            if (!response)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Failed to delete order",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Order deleted successfully",
                StatusCode = HttpStatusCode.OK
            });
        }
        /// <summary>
        /// Retrieves the details of a specific order by its ID.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to retrieve.</param>
        /// <returns>
        /// Returns the details of the order if found, or a NotFound response if the order does not exist.
        /// </returns>
        /// <remarks>
        /// This method retrieves the details of the order with the specified ID. 
        /// If the order exists, its details are returned in the response, otherwise a NotFound response is returned.
        /// </remarks>
        /// <response code="200">Fetched order successfully.</response>
        /// <response code="404">Order Not found.</response>
        [HttpGet("getOrder/{orderId:guid}")]
        [Authorize]
        public async Task<ActionResult<OrderResponse>> GetOrder(Guid orderId)
        {
            var response = await _orderServices.GetByAsync(x => x.OrderID == orderId);
            if (response == null)
            {
                return NotFound(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Order not found",
                    StatusCode = HttpStatusCode.NotFound
                });
            }
            return Ok(response);
        }

        /// <summary>
        /// Retrieves all orders in the system.
        /// </summary>
        /// <returns>
        /// Returns a list of all orders if found, or a NotFound response if no orders exist.
        /// </returns>
        /// <remarks>
        /// This method fetches all orders from the system and returns them in a list. 
        /// If no orders are found, a NotFound response is returned. 
        /// Otherwise, the orders are returned within an <see cref="ApiResponse"/> with a success message.
        /// </remarks>
        /// <response code="200">Fetched orders successfully.</response>
        /// <response code="404">Orders Not found.</response>
        [HttpGet("getOrders")]
        [Authorize]
        public async Task<ActionResult<List<OrderResponse>>> GetOrders()
        {
            var response = await _orderServices.GetAllAsync();
            if (response == null)
            {
                return NotFound(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "No orders found",
                    StatusCode = HttpStatusCode.NotFound,
                    Result = Enumerable.Empty<OrderResponse>()
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = true,
                Message = "Orders retrieved successfully",
                Result = response,
                StatusCode = HttpStatusCode.OK
            });
        }
        /// <summary>
        /// Retrieves a list of orders associated with the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// This method retrieves the user's email from the HTTP context and finds the corresponding user in the database. 
        /// It then attempts to fetch all orders related to the user's ID. If the user or orders are not found, appropriate responses are returned.
        /// </remarks>
        /// <param name="email">The email address of the currently authenticated user, retrieved from the HTTP context.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> with a list of orders if found. 
        /// If no user or orders are found, a <see cref="ApiResponse"/> with an error message and the corresponding HTTP status code is returned.
        /// </returns>
        /// <response code="200">Order added successfully.</response>
        /// <response code="400">Failed to add the Order or invalid data.</response>
        /// <response code="401">User is not authenticated.</response>
        [HttpGet("getOrdersByUser")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetOrdersByUser()
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var user = await _unitOfWork.Repository<ApplicationUser>()
                .GetByAsync(x => x.Email == email);

            if (user == null)
            {
                return NotFound(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "User not found",
                    StatusCode = HttpStatusCode.NotFound
                });
            }

            var response = await _orderServices
                .GetAllAsync(x => x.UserID == user.Id);

            if (response == null)
            {
                return NotFound(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "No orders found",
                    StatusCode = HttpStatusCode.NotFound
                });
            }

            return Ok(new ApiResponse()
            {
                IsSuccess = true,
                Message = "Orders retrieved successfully",
                Result = response,
                StatusCode = HttpStatusCode.OK
            });
        }



    }
}
