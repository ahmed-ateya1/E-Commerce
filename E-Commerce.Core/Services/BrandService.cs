using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos.BrandDto;
using E_Commerce.Core.Helper;
using E_Commerce.Core.ServicesContract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Services
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BrandService> _logger;
        private readonly IMapper _mapper;

        public BrandService(IUnitOfWork unitOfWork, ILogger<BrandService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        private async Task ExecuteWithTransaction(Func<Task> action)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    await action();
                    await _unitOfWork.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    await _unitOfWork.RollbackTransactionAsync();
                    throw;
                }
            }
        }
        public async Task<BrandResponse?> CreateAsync(BrandAddRequest? brandAddRequest)
        {
            if(brandAddRequest == null)
            {
                throw new ArgumentNullException(nameof(brandAddRequest));
            }
            ValidationHelper.ValidateModel(brandAddRequest);

            var brand = _mapper.Map<Brand>(brandAddRequest);
            await ExecuteWithTransaction(async () =>
            {
                await _unitOfWork.Repository<Brand>().CreateAsync(brand);
                await _unitOfWork.CompleteAsync();
            });
            return _mapper.Map<BrandResponse>(brand);
        }

        public async Task<bool> DeleteAsync(Guid? id)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            var brand = await _unitOfWork.Repository<Brand>()
                .GetByAsync(x=>x.BrandID == id);
            if (brand == null)
            {
                throw new ArgumentNullException(nameof(brand));
            }
            await ExecuteWithTransaction(async () =>
            {
                await _unitOfWork.Repository<Brand>().DeleteAsync(brand);
                await _unitOfWork.CompleteAsync();
            });
            return true;
        }

        public async Task<IEnumerable<BrandResponse>> GetAllAsync
            (Expression<Func<Brand, bool>>? filter = null, int? pageIndex = null, int? pageSize = null)
        {
            if(pageIndex == null || pageSize == null)
            {
                pageSize = 10;
                pageIndex = 1;
            }
            var brands = await _unitOfWork.Repository<Brand>()
                .GetAllAsync(filter, "",null,pageIndex, pageSize);

            if (!brands.Any())
                return [];
            return _mapper.Map<IEnumerable<BrandResponse>>(brands);
        }

        public async Task<BrandResponse?> GetByAsync(Expression<Func<Brand, bool>> filter)
        {
            var brand = await _unitOfWork.Repository<Brand>()
                .GetByAsync(filter);
            if (brand == null)
            {
                return null;
            }
            return _mapper.Map<BrandResponse>(brand);
        }

        public async Task<BrandResponse?> UpdateAsync(BrandUpdateRequest? brandUpdateRequest)
        {
            if (brandUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(brandUpdateRequest));
            }
            ValidationHelper.ValidateModel(brandUpdateRequest);

            var Oldbrand = await _unitOfWork.Repository<Brand>()
                .GetByAsync(x => x.BrandID == brandUpdateRequest.BrandID);

           if(Oldbrand == null)
           {
                throw new ArgumentNullException(nameof(Oldbrand));
           }
           var brand = _mapper.Map(brandUpdateRequest, Oldbrand);
           await ExecuteWithTransaction(async () =>
           {
               await _unitOfWork.Repository<Brand>().UpdateAsync(brand);
               await _unitOfWork.CompleteAsync();
           });
           return _mapper.Map<BrandResponse>(brand);
        }
    }
}
