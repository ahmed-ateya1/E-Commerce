using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos.AddressDto;
using E_Commerce.Core.ServicesContract;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace E_Commerce.Core.Services
{
    public class AddressServices : IAddressServices
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AddressServices> _logger;
        private readonly IUserContext _userContext;

        public AddressServices
            (IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<AddressServices> logger,
            IUserContext userContext)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userContext = userContext;
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
        public async Task<AddressResponse?> CreateAsync(AddressAddRequest? addressAddRequest)
        {
            if (addressAddRequest == null)
            {
                _logger.LogError("AddressAddRequest is null");
                return null;
            }
            var user = await _userContext.GetCurrentUserAsync();
            var address = _mapper.Map<Address>(addressAddRequest);
            address.UserID = user.Id;
            address.User = user;
            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Address>().CreateAsync(address);
            });
            return _mapper.Map<AddressResponse>(address);
        }

        public async Task<bool> DeleteAsync(Guid addressID)
        {
            var address = await _unitOfWork.Repository<Address>()
                .GetByAsync(a => a.AddressID == addressID);

            if (address == null)
            {
                _logger.LogError("Address not found");
                return false;
            }
            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Address>().DeleteAsync(address);
            });
            return true;
        }

        public async Task<IEnumerable<AddressResponse>> GetAllAsync(Expression<Func<Address, bool>>? predicate = null)
        {
            var addresses = await _unitOfWork.Repository<Address>()
                .GetAllAsync(predicate , includeProperties: "User");
            if(!addresses.Any())
            {
                return Enumerable.Empty<AddressResponse>();
            }
            return _mapper.Map<IEnumerable<AddressResponse>>(addresses);
        }

        public async Task<AddressResponse?> GetByAsync(Expression<Func<Address, bool>> predicate, bool isTracked = false)
        {
            var address = await _unitOfWork.Repository<Address>()
                .GetByAsync(predicate,isTracked,includeProperties:"User");
            if (address == null)
            {
                _logger.LogError("Address not found");
                return null;
            }
            return _mapper.Map<AddressResponse>(address);
        }
    }
}
