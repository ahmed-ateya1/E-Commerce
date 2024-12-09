using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ReviewDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.ServicesContract
{
    public interface IReviewService 
    {
        Task<ReviewResponse?> CreateAsync(ReviewAddRequest? request);
        Task<ReviewResponse?> UpdateAsync(ReviewUpdateRequest? request);
        Task<bool> DeleteAsync(Guid reviewID);
        Task<ReviewResponse?> GetByAsync(Expression<Func<Review,bool>> expression , bool isTracked = false);
        Task<PaginatedResponse<ReviewResponse>> GetAllAsync(Expression<Func<Review, bool>>? expression = null , PaginationDto? pagination = null);
    }
}
