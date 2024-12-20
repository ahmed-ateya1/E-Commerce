using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.AddressDto;
using System.Linq.Expressions;

namespace E_Commerce.Core.ServicesContract
{
    public interface IAddressServices
    {
        Task<AddressResponse?> CreateAsync(AddressAddRequest? addressAddRequest);
        Task<bool> DeleteAsync(Guid addressID);
        Task<AddressResponse?>GetByAsync(Expression<Func<Address, bool>> predicate , bool isTracked = false);
        Task<IEnumerable<AddressResponse>>GetAllAsync(Expression<Func<Address, bool>>? predicate = null);
    }
}
