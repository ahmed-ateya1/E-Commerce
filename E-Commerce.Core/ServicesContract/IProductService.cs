using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.ServicesContract
{
    public interface IProductService
    {
        Task<ProductResponse?> CreateAsync(ProductAddRequest? request);
        Task<ProductResponse?> UpdateAsync(ProductUpdateRequest? request);
        Task<bool> DeleteAsync(Guid id);
        Task<PaginatedResponse<ProductResponse>> GetAllAsync(
             Expression<Func<Product, bool>>? predicate = null, PaginationDto? pagination = null);

        Task<ProductResponse?> GetByAsync(Expression<Func<Product , bool>>predicate , bool isTracked = false);
    }
}
