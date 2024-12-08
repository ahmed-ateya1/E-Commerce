using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.WishlistDto;
using System.Linq.Expressions;

namespace E_Commerce.Core.ServicesContract
{
    public interface IWishlistService
    {
        Task<WishlistResponse?> CreateAsync(WishlistAddRequest? request);
        Task<WishlistResponse?> UpdateAsync(WishlistUpdateRequest? request);
        Task<bool> DeleteAsync(Guid id);
        Task<WishlistResponse?> GetByAsync(Expression<Func<Wishlist, bool>> expression , bool isTracked = false);
        Task<IEnumerable<WishlistResponse>> GetAllAsync(Expression<Func<Wishlist, bool>>? expression = null);
    }
}
