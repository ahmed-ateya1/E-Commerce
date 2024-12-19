using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos.DeliveryMethodDto;
using E_Commerce.Core.ServicesContract;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace E_Commerce.Core.Services
{
    public class DeliveryMethodServices : IDeliveryMethodServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DeliveryMethodServices> _logger;

        public DeliveryMethodServices
            (IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger<DeliveryMethodServices> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
        public async Task<DeliveryMethodResponse> CreateAsync(DeliveryMethodAddRequest? request)
        {
            if (request == null)
            {
                _logger.LogError("Request is null");
                return null;
            }
            var deliveryMethod = _mapper.Map<DeliveryMethod>(request);

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<DeliveryMethod>().CreateAsync(deliveryMethod);
            });
            return _mapper.Map<DeliveryMethodResponse>(deliveryMethod);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                .GetByAsync(x => x.DeliveryMethodID == id);
            if (deliveryMethod == null)
            {
                _logger.LogError("Delivery method not found");
                return false;
            }
            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<DeliveryMethod>().DeleteAsync(deliveryMethod);
            });
            return true;
        }

        public async Task<IEnumerable<DeliveryMethodResponse>> GetAllAsync(Expression<Func<DeliveryMethod, bool>>? expression = null)
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>()
                .GetAllAsync(expression);
            if (!deliveryMethods.Any())
            {
                return Enumerable.Empty<DeliveryMethodResponse>();
            }
            return _mapper.Map<IEnumerable<DeliveryMethodResponse>>(deliveryMethods);
        }

        public async Task<DeliveryMethodResponse?> GetByAsync(Expression<Func<DeliveryMethod, bool>> expression)
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                .GetByAsync(expression);
            if (deliveryMethod == null)
            {
                return null;
            }
            return _mapper.Map<DeliveryMethodResponse>(deliveryMethod);
        }

        public async Task<DeliveryMethodResponse?> UpdateAsync(DeliveryMethodUpdateRequest? request)
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                .GetByAsync(x => x.DeliveryMethodID == request.DeliveryMethodID);
            if (deliveryMethod == null)
            {
                _logger.LogError("Delivery method not found");
                return null;
            }
            await ExecuteWithTransactionAsync(async () =>
            {
                deliveryMethod = _mapper.Map(request, deliveryMethod);
                await _unitOfWork.Repository<DeliveryMethod>().UpdateAsync(deliveryMethod);
            });
            return _mapper.Map<DeliveryMethodResponse>(deliveryMethod);
        }
    }
}
