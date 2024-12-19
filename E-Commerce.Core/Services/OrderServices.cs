using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos.OrderDto;
using E_Commerce.Core.Helper;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Security.Claims;

namespace E_Commerce.Core.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderServices> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderServices
            (
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<OrderServices> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<OrderResponse> CreateAsync(OrderAddRequest? orderRequest)
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
                throw new KeyNotFoundException("Address not found.");
            }

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                .GetByAsync(x => x.DeliveryMethodID == orderRequest.DeliveryMethodID);
            if (deliveryMethod == null)
            {
                _logger.LogError("Delivery method not found.");
                throw new KeyNotFoundException("Delivery method not found.");
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
                    .GetByAsync(x => x.ProductID == orderItemRequest.ProductID);

                if (product == null)
                {
                    _logger.LogError($"Product with ID {orderItemRequest.ProductID} not found.");
                    throw new KeyNotFoundException($"Product with ID {orderItemRequest.ProductID} not found.");
                }

                if (product.StockQuantity < orderItemRequest.Quantity)
                {
                    _logger.LogError($"Insufficient stock for product {product.ProductName}. Requested: {orderItemRequest.Quantity}, Available: {product.StockQuantity}");
                    throw new InvalidOperationException($"Insufficient stock for product {product.ProductName}.");
                }

                var orderItem = _mapper.Map<OrderItem>(orderItemRequest);
                orderItem.Price = product.ProductPrice; 
                orderItems.Add(orderItem);
            }


            order.OrderItems = orderItems;
            order.SubTotal = orderItems.Sum(x => x.Price * x.Quantity);

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Order>().CreateAsync(order);
            });

            return _mapper.Map<OrderResponse>(order);
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var order = await _unitOfWork.Repository<Order>()
                .GetByAsync(x => x.OrderID == id ,includeProperties: "OrderItems");

            if (order == null)
            {
                _logger.LogError($"Order with ID {id} not found.");
                return false;
            }
            await ExecuteWithTransactionAsync(async () =>
            {
                if (order.OrderItems.Any())
                {
                   await _unitOfWork.Repository<OrderItem>().RemoveRangeAsync(order.OrderItems);
                }
                await _unitOfWork.Repository<Order>().DeleteAsync(order);
            });
            return true;
        }

        public async Task<IEnumerable<OrderResponse>> GetAllAsync(Expression<Func<Order, bool>>? filter = null)
        {
            _logger.LogInformation("Fetching all orders with the specified filter.");

            var orders = await _unitOfWork.Repository<Order>()
                .GetAllAsync(filter, includeProperties: "OrderItems,Address,DeliveryMethod,User");

            if (!orders.Any())
            {
                _logger.LogWarning("No orders found.");
                return Enumerable.Empty<OrderResponse>();
            }

            return _mapper.Map<IEnumerable<OrderResponse>>(orders);
        }

        public async Task<OrderResponse> GetByAsync(Expression<Func<Order, bool>> filter, bool isTracked = false)
        {
            _logger.LogInformation("Fetching an order by the specified filter.");

            var order = await _unitOfWork.Repository<Order>()
                .GetByAsync(filter, includeProperties: "OrderItems,Address,DeliveryMethod,User", isTracked: isTracked);

            if (order == null)
            {
                _logger.LogError("Order not found.");
                throw new KeyNotFoundException("Order not found.");
            }

            return _mapper.Map<OrderResponse>(order);
        }

        public async Task<bool> UpdateAsync(Guid orderID, OrderStatus orderStatus)
        {
            var order = await _unitOfWork.Repository<Order>()
                .GetByAsync(x => x.OrderID == orderID);
            if (order == null)
            {
                _logger.LogError($"Order with ID {orderID} not found.");
                return false;
            }
            order.OrderStatus = orderStatus;
            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Order>().UpdateAsync(order);
            });
            return true;
        }
    }
}
