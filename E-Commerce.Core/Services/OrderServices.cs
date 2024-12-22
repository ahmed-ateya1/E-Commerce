using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.OrderDto;
using E_Commerce.Core.Helper;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;

namespace E_Commerce.Core.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderServices> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPaymentService _paymentService;

        public OrderServices
            (
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<OrderServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _paymentService = paymentService;
        }
        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    _logger.LogInformation("Transaction started.");
                    await action();
                    await _unitOfWork.CommitTransactionAsync();
                    _logger.LogInformation("Transaction committed successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Transaction failed. Rolling back...");
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("No user is authenticated.");
                throw new UnauthorizedAccessException("No user is authenticated.");
            }

            var user = await _unitOfWork.Repository<ApplicationUser>()
                .GetByAsync(x => x.Email == email);
            if (user == null)
            {
                _logger.LogError("User not found.");
                throw new KeyNotFoundException("User not found.");
            }
            return user;
        }

        public async Task<ServiceResponse> CreateAsync(OrderAddRequest? orderRequest)
        {
            _logger.LogInformation("Creating a new order.");

            if (orderRequest == null)
            {
                _logger.LogError("Order request is null.");
                throw new ArgumentNullException(nameof(orderRequest));
            }

            var user = await GetCurrentUserAsync();
            var address = await _unitOfWork.Repository<Address>()
                .GetByAsync(x => x.AddressID == orderRequest.AddressID);
            if (address == null)
            {
                _logger.LogError("Address not found.");
                return new ServiceResponse
                {
                    Message = "Address not found.",
                    IsSuccess = false,
                    Result = null,
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                .GetByAsync(x => x.DeliveryMethodID == orderRequest.DeliveryMethodID);
            if (deliveryMethod == null)
            {
                _logger.LogError("Delivery method not found.");
                return new ServiceResponse
                {
                    Message = "Delivery method not found.",
                    IsSuccess = false,
                    Result = null,
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            var order = _mapper.Map<Order>(orderRequest);
            order.UserID = user.Id;
            order.User = user;
            order.AddressID = address.AddressID;
            order.Address = address;
            order.DeliveryMethodID = deliveryMethod.DeliveryMethodID;
            order.DeliveryMethod = deliveryMethod;
            order.OrderNumber = OrderNumberGeneratorHelper.GenerateOrderNumber(order.UserID);

            var orderItems = new List<OrderItem>();

            foreach (var orderItemRequest in orderRequest.OrderItems)
            {
                var product = await _unitOfWork.Repository<Product>()
                    .GetByAsync(x => x.ProductID == orderItemRequest.ProductID , includeProperties: "ProductImages");

                if (product == null)
                {
                    _logger.LogError($"Product with ID {orderItemRequest.ProductID} not found.");
                    throw new KeyNotFoundException($"Product with ID {orderItemRequest.ProductID} not found.");
                }

                if (product.StockQuantity < orderItemRequest.Quantity)
                {
                    _logger.LogError($"Insufficient stock for product {product.ProductName}. Requested: {orderItemRequest.Quantity}, Available: {product.StockQuantity}");
                    return new ServiceResponse
                    {
                        Message = $"Insufficient stock for product {product.ProductName}. Requested: {orderItemRequest.Quantity}, Available: {product.StockQuantity}",
                        IsSuccess = false,
                        Result = null,
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
                product.StockQuantity -= orderItemRequest.Quantity;
                await _unitOfWork.Repository<Product>().UpdateAsync(product);

                var orderItem = _mapper.Map<OrderItem>(orderItemRequest);
                orderItem.Price = product.ProductPrice;
                orderItem.Product = product;
                orderItems.Add(orderItem);
            }

            order.OrderItems = orderItems;
            order.SubTotal = orderItems.Sum(x => x.Price * x.Quantity);
            decimal totalAmount = order.SubTotal + order.DeliveryMethod.Price;
            var paymentIntent = await _paymentService.CreatePaymentIntent(totalAmount, "usd", "Order Payment");
            order.PaymentIntentID = paymentIntent.Id;
            order.ClientSecret = paymentIntent.ClientSecret;
            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Order>().CreateAsync(order);
                await _unitOfWork.CompleteAsync();
            });
           
            return new ServiceResponse
            {
                Message = "Order created successfully.",
                IsSuccess = true,
                Result = _mapper.Map<OrderResponse>(order),
                StatusCode = HttpStatusCode.Created
            };
        }



        public async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            var order = await _unitOfWork.Repository<Order>()
                .GetByAsync(x => x.OrderID == id, includeProperties: "OrderItems");

            if (order == null)
            {
                _logger.LogError($"Order with ID {id} not found.");
                return new ServiceResponse
                {
                    Message = $"Order with ID {id} not found.",
                    IsSuccess = false,
                    Result = null,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            if (order.OrderStatus == OrderStatus.Completed)
            {
                _logger.LogError("Cannot delete a completed order.");
                return new ServiceResponse
                {
                    Message = "Cannot delete a completed order.",
                    IsSuccess = false,
                    Result = null,
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                order.OrderStatus = OrderStatus.Cancelled;
                await _unitOfWork.Repository<Order>().UpdateAsync(order);

                if (!string.IsNullOrEmpty(order.PaymentIntentID))
                {
                    var refundResult = await _paymentService.ProcessRefund(order.PaymentIntentID);

                    if (!refundResult.IsSuccess)
                    {
                        throw new Exception($"Refund failed: {refundResult.Message}");
                    }
                    _logger.LogInformation($"Refund processed successfully for Order ID: {id}");
                }

                foreach (var orderItem in order.OrderItems)
                {
                    var product = await _unitOfWork.Repository<Product>()
                        .GetByAsync(x => x.ProductID == orderItem.ProductID);

                    if (product != null)
                    {
                        product.StockQuantity += orderItem.Quantity;
                        await _unitOfWork.Repository<Product>().UpdateAsync(product);
                    }
                }
                if (order.OrderItems.Any())
                {
                    await _unitOfWork.Repository<OrderItem>().RemoveRangeAsync(order.OrderItems);
                }

                await _unitOfWork.Repository<Order>().DeleteAsync(order);
            });

            return new ServiceResponse
            {
                Result = true,
                IsSuccess = true,
                Message = "Order cancelled and deleted successfully. Refund processed.",
                StatusCode = HttpStatusCode.OK
            };
        }


        public async Task<ServiceResponse> GetAllAsync(Expression<Func<Order, bool>>? filter = null)
        {
            _logger.LogInformation("Fetching all orders with the specified filter.");

            var orders = await _unitOfWork.Repository<Order>()
                .GetAllAsync(filter, includeProperties: "OrderItems,OrderItems.Product,OrderItems.Product.ProductImages,Address,DeliveryMethod,User");

            if (!orders.Any())
            {
                _logger.LogWarning("No orders found.");
                return new ServiceResponse
                {
                    Message = "No orders found.",
                    IsSuccess = true,
                    Result = Enumerable.Empty<OrderResponse>(),
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            return new ServiceResponse
            {
                Result = _mapper.Map<IEnumerable<OrderResponse>>(orders),
                IsSuccess = true,
                Message = "Orders fetched successfully.",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse> GetByAsync(Expression<Func<Order, bool>> filter, bool isTracked = false)
        {
            _logger.LogInformation("Fetching an order by the specified filter.");

            var order = await _unitOfWork.Repository<Order>()
                .GetByAsync(filter, includeProperties: "OrderItems,OrderItems.Product,Address,DeliveryMethod,User", isTracked: isTracked);

            if (order == null)
            {
                _logger.LogError("Order not found.");
                return new ServiceResponse
                {
                    Message = "Order not found.",
                    IsSuccess = false,
                    Result = null,
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            return new ServiceResponse
            {
                Result = _mapper.Map<OrderResponse>(order),
                IsSuccess = true,
                Message = "Order fetched successfully.",
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<ServiceResponse> UpdateAsync(Order order , OrderStatus orderStatus)
        {
            var orderRes = await _unitOfWork.Repository<Order>()
                .GetByAsync(x => x.OrderID == order.OrderID);
            if (order == null)
            {
                _logger.LogError($"Order with ID {order.OrderID} not found.");
                return new ServiceResponse
                {
                    Message = $"Order with ID {order.OrderID} not found.",
                    IsSuccess = false,
                    Result = null,
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            order.OrderStatus = orderStatus;
            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Order>().UpdateAsync(order);
            });
            return new ServiceResponse
            {
                Result = true,
                IsSuccess = true,
                Message = "Order updated successfully.",
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
