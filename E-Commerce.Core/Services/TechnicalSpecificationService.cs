using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos.TechnicalSpecificationDto;
using E_Commerce.Core.Helper;
using E_Commerce.Core.ServicesContract;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace E_Commerce.Core.Services
{
    public class TechnicalSpecificationService : ITechnicalSpecificationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TechnicalSpecificationService> _logger;

        public TechnicalSpecificationService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<TechnicalSpecificationService> logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

     

        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await action();
                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("Transaction committed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the transaction. Rolling back.");
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<TechnicalSpecificationResponse> CreateAsync(TechnicalSpecificationAddRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            ValidationHelper.ValidateModel(request);

            if (await _unitOfWork.Repository<Product>().CheckEntityAsync(x=>x.ProductID == request.ProductID,"Profduct") == null)
                throw new ArgumentException("Product not found.");

            _logger.LogInformation("Creating a new technical specification for ProductID {ProductID}.", request.ProductID);

            var technicalSpecification = _mapper.Map<TechnicalSpecification>(request);

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<TechnicalSpecification>().CreateAsync(technicalSpecification);
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Technical specification created successfully for ProductID {ProductID}.", request.ProductID);
            return _mapper.Map<TechnicalSpecificationResponse>(technicalSpecification);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting technical specification with ID {ID}.", id);

            var technicalSpecification = await _unitOfWork.Repository<TechnicalSpecification>().GetByAsync(x => x.TechnicalSpecificationID == id);
            if (technicalSpecification == null)
            {
                _logger.LogWarning("Technical specification with ID {ID} not found.", id);
                return false;
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<TechnicalSpecification>().DeleteAsync(technicalSpecification);
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Technical specification with ID {ID} deleted successfully.", id);
            return true;
        }

        public async Task<IEnumerable<TechnicalSpecificationResponse>> GetAllAsync(Expression<Func<TechnicalSpecification, bool>>? expression = null)
        {
            _logger.LogInformation("Fetching all technical specifications with filter: {Filter}.", expression);

            var technicalSpecifications = await _unitOfWork.Repository<TechnicalSpecification>().GetAllAsync(expression);
            if (technicalSpecifications == null || !technicalSpecifications.Any())
            {
                _logger.LogWarning("No technical specifications found with the given filter: {Filter}.", expression);
                return Enumerable.Empty<TechnicalSpecificationResponse>();
            }

            var response = _mapper.Map<IEnumerable<TechnicalSpecificationResponse>>(technicalSpecifications);

            _logger.LogInformation("{Count} technical specifications fetched successfully.", response.Count());
            return response;
        }

        public async Task<TechnicalSpecificationResponse?> GetByAsync(Expression<Func<TechnicalSpecification, bool>> expression, bool isTracked = false)
        {
            _logger.LogInformation("Fetching technical specification with filter: {Filter}.", expression);

            var technicalSpecification = await _unitOfWork.Repository<TechnicalSpecification>().GetByAsync(expression, isTracked);
            if (technicalSpecification == null)
            {
                _logger.LogWarning("Technical specification not found with the given filter: {Filter}.", expression);
                return null;
            }

            _logger.LogInformation("Technical specification fetched successfully.");
            return _mapper.Map<TechnicalSpecificationResponse>(technicalSpecification);
        }

        public async Task<TechnicalSpecificationResponse> UpdateAsync(TechnicalSpecificationUpdateRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            ValidationHelper.ValidateModel(request);

            if (await _unitOfWork.Repository<Product>().CheckEntityAsync(x => x.ProductID == request.ProductID, "Profduct") == null)
                throw new ArgumentException("Product not found.");

            _logger.LogInformation("Updating technical specification with ID {ID}.", request.TechnicalSpecificationID);

            var technicalSpecification = await _unitOfWork.Repository<TechnicalSpecification>().GetByAsync(x => x.TechnicalSpecificationID == request.TechnicalSpecificationID);
            if (technicalSpecification == null)
                throw new ArgumentException("Technical specification not found.");

            _mapper.Map(request, technicalSpecification);

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<TechnicalSpecification>().UpdateAsync(technicalSpecification);
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Technical specification with ID {ID} updated successfully.", request.TechnicalSpecificationID);
            return _mapper.Map<TechnicalSpecificationResponse>(technicalSpecification);
        }
    }
}
