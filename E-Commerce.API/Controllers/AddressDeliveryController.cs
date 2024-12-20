using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.AddressDto;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressDeliveryController : ControllerBase
    {
        private readonly IAddressServices _addressServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public AddressDeliveryController(IAddressServices addressServices, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _addressServices = addressServices;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Adds a new address delivery.
        /// </summary>
        /// <param name="request">The address delivery details to be added.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> indicating success or failure of the operation.
        /// </returns>
        /// <remarks>
        /// **HTTP Status Codes**:
        /// - 200 OK: If the address delivery was added successfully.
        /// - 400 BadRequest: If the address delivery could not be added.
        /// </remarks>
        [HttpPost("addAddressDelivery")]
        public async Task<ActionResult<ApiResponse>> AddAddressDeliveryAsync(AddressAddRequest request)
        {
            var response = await _addressServices.CreateAsync(request);
            if (response == null)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Failed to add address delivery.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = true,
                Message = "Address delivery added successfully.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves a specific address delivery by its ID.
        /// </summary>
        /// <param name="addressId">The unique identifier of the address delivery.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> containing the address delivery details.
        /// </returns>
        /// <remarks>
        /// **HTTP Status Codes**:
        /// - 200 OK: If the address delivery was retrieved successfully.
        /// - 400 BadRequest: If the address delivery could not be retrieved.
        /// </remarks>
        [HttpGet("getAddressDelivery/{addressId}")]
        public async Task<ActionResult<ApiResponse>> GetAddressDeliveryAsync(Guid addressId)
        {
            var response = await _addressServices.GetByAsync(x => x.AddressID == addressId);
            if (response == null)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Failed to get address delivery.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = true,
                Message = "Address delivery retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all address deliveries.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> containing a list of all address deliveries.
        /// </returns>
        /// <remarks>
        /// **HTTP Status Codes**:
        /// - 200 OK: If all address deliveries were retrieved successfully.
        /// - 400 BadRequest: If the retrieval failed.
        /// </remarks>
        [HttpGet("getAllAddressDelivery")]
        public async Task<ActionResult<ApiResponse>> GetAllAddressDeliveryAsync()
        {
            var response = await _addressServices.GetAllAsync();
            if (response == null)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Failed to get all address delivery.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = true,
                Message = "All address delivery retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Retrieves all address deliveries for the authenticated user.
        /// </summary>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> containing the address deliveries for the user.
        /// </returns>
        /// <remarks>
        /// **HTTP Status Codes**:
        /// - 200 OK: If the address deliveries were retrieved successfully.
        /// - 400 BadRequest: If the retrieval failed or the user is not authenticated.
        /// </remarks>
        [HttpGet("getAddressDeliveryByUser")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> GetAddressDeliveryByUserAsync()
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Failed to get address delivery by user.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            var user = await _unitOfWork.Repository<ApplicationUser>().GetByAsync(x => x.Email == email);
            if (user == null)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Failed to get address delivery by user.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            var response = await _addressServices.GetAllAsync(x => x.UserID == user.Id);
            if (response == null)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Failed to get address delivery by user.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = true,
                Message = "Address delivery by user retrieved successfully.",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Deletes a specific address delivery by its ID.
        /// </summary>
        /// <param name="addressId">The unique identifier of the address delivery.</param>
        /// <returns>
        /// Returns an <see cref="ApiResponse"/> indicating success or failure of the operation.
        /// </returns>
        /// <remarks>
        /// **HTTP Status Codes**:
        /// - 200 OK: If the address delivery was deleted successfully.
        /// - 400 BadRequest: If the address delivery could not be deleted.
        /// </remarks>
        [HttpDelete("deleteAddressDelivery/{addressId}")]
        public async Task<ActionResult<ApiResponse>> DeleteAddressDeliveryAsync(Guid addressId)
        {
            var response = await _addressServices.DeleteAsync(addressId);
            if (!response)
            {
                return BadRequest(new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Failed to delete address delivery.",
                    StatusCode = HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse()
            {
                IsSuccess = true,
                Message = "Address delivery deleted successfully.",
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
