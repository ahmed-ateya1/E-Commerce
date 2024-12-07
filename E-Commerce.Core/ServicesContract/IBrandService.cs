using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.BrandDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.ServicesContract
{
    public interface IBrandService
    {
        Task<BrandResponse?> CreateAsync(BrandAddRequest? brandAddRequest);
        Task<BrandResponse?> UpdateAsync(BrandUpdateRequest? brandUpdateRequest);
        Task<bool> DeleteAsync(Guid? id);
        Task<BrandResponse?> GetByAsync(Expression<Func<Brand, bool>> filter);
        Task<IEnumerable<BrandResponse>> GetAllAsync(Expression<Func<Brand, bool>>? filter = null);
    }
}
