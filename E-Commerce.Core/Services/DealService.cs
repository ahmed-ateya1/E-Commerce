using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.DealDto;
using E_Commerce.Core.Dtos.ProductDto;
using E_Commerce.Core.ServicesContract;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace E_Commerce.Core.Services
{
    public class DealService : IDealService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DealService> _logger;

        public DealService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<DealService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
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
        public async Task<ServiceResponse> CreateAsync(DealAddRequest? request)
        {
            _logger.LogInformation("DealService.CreateAsync called");
            if (request == null)
            {
                return new ServiceResponse {
                    Message = "Deal can't be null!"
                };
            }

            var product = await _unitOfWork.Repository<Product>()
                .GetByAsync(x => x.ProductID == request.ProductID, includeProperties: "ProductImages,Reviews,OrderItems");

            if (product == null)
            {
                _logger.LogWarning("Product not found!");
                return new ServiceResponse
                {
                    Message = "Product not found!"
                };
            }
            var deal = _mapper.Map<Deal>(request);
            deal.Product = product;
            await ExecuteWithTransactionAsync(async () =>
            {
                _logger.LogInformation("Creating deal...");
                await _unitOfWork.Repository<Deal>().CreateAsync(deal);
                await _unitOfWork.CompleteAsync();
            });

            return new ServiceResponse
            {
                IsSuccess = true,
                Message = "Deal created successfully!",
                Result = _mapper.Map<DealResponse>(deal)
            };
        }

        public async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            _logger.LogInformation("DealService.DeleteAsync called");
            var deal = await _unitOfWork.Repository<Deal>()
                .GetByAsync(x => x.DealID == id);

            if (deal == null)
            {
                _logger.LogWarning("Deal not found!");
                return new ServiceResponse
                {
                    Message = "Deal not found!"
                };
            }
            await ExecuteWithTransactionAsync(async () =>
            {
                _logger.LogInformation("Deleting deal...");
                await _unitOfWork.Repository<Deal>().DeleteAsync(deal);
                await _unitOfWork.CompleteAsync();
            });
            _logger.LogInformation("Deal deleted successfully!");
            return new ServiceResponse
            {
                IsSuccess = true,
                Message = "Deal deleted successfully!"
            };
        }

        public async Task<PaginatedResponse<ProductResponse>> GetAllAsync(Expression<Func<Deal, bool>>? expression = null, PaginationDto? pagination = null)
        {
            _logger.LogInformation("DealService.GetAllAsync called");
            pagination ??= new PaginationDto();
            _logger.LogInformation("Fetching all products with pagination: PageIndex={PageIndex}, PageSize={PageSize}",
                pagination.PageIndex, pagination.PageSize);


            _logger.LogInformation("Fetching all deals...");
            var deals = await _unitOfWork.Repository<Deal>()
                .GetAllAsync(expression,
                includeProperties: "Product,Product.ProductImages,Product.Reviews,Product.OrderItems",
                sortBy: pagination.SortBy,
                sortDirection: pagination.SortDirection,
                pageSize: pagination.PageSize,
                pageIndex: pagination.PageIndex);

            long dealCount = await _unitOfWork.Repository<Deal>().CountAsync(expression);
            if (deals == null || !deals.Any())
            {
                _logger.LogWarning("No deals found!");
                return new PaginatedResponse<ProductResponse>
                {
                    PageIndex = pagination.PageIndex,
                    PageSize = pagination.PageSize,
                    Items = new List<ProductResponse>(),
                    TotalCount = 0
                };
            }
            var result = _mapper.Map<List<ProductResponse>>(deals);

            _logger.LogInformation("Fetched {Count} deals", result.Count);
            return new PaginatedResponse<ProductResponse>
            {
                Items = result,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize,
                TotalCount = dealCount
            };
        }

        public async Task<ServiceResponse> GetByAsync(Expression<Func<Deal, bool>> expression, bool isTracking = false)
        {
            _logger.LogInformation("DealService.GetByAsync called");
            var deal = await _unitOfWork.Repository<Deal>()
                .GetByAsync(expression,isTracking,includeProperties: "Product,Product.ProductImages,Product.Reviews,Product.OrderItems");

            if (deal == null)
            {
                _logger.LogWarning("Deal not found!");
                return new ServiceResponse
                {
                    Message = "Deal not found!"
                };
            }
            _logger.LogInformation("Deal fetched successfully!");
            return new ServiceResponse
            {
                IsSuccess = true,
                Message = "Deal fetched successfully!",
                Result = _mapper.Map<DealResponse>(deal)
            };
        }

        public async Task<ServiceResponse> UpdateAsync(DealUpdateRequest? request)
        {
            if (request == null)
            {
                return new ServiceResponse
                {
                    Message = "Deal can't be null!"
                };
            }

            _logger.LogInformation("DealService.UpdateAsync called");

            var productExist = await _unitOfWork.Repository<Product>()
                .AnyAsync(x => x.ProductID == request.ProductID);

            if (!productExist)
            {
                _logger.LogWarning("Product not found!");
                return new ServiceResponse
                {
                    Message = "Product not found!"
                };
            }

            var deal = await _unitOfWork.Repository<Deal>()
                .GetByAsync(x => x.DealID == request.DealID,includeProperties: "Product,Product.ProductImages,Product.Reviews,Product.OrderItems");

            if (deal == null)
            {
                _logger.LogWarning("Deal not found!");
                return new ServiceResponse
                {
                    Message = "Deal not found!"
                };
            }
            
            _mapper.Map(request, deal);

            await ExecuteWithTransactionAsync(async () =>
            {
                _logger.LogInformation("Updating deal...");
                await _unitOfWork.Repository<Deal>().UpdateAsync(deal);
                await _unitOfWork.CompleteAsync();
            });

            return new ServiceResponse
            {
                IsSuccess = true,
                Message = "Deal updated successfully!",
                Result = _mapper.Map<DealResponse>(deal)
            };
        }
    }
}
