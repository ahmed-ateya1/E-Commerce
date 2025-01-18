using E_Commerce.Core.Domain.IdentityEntities;

namespace E_Commerce.Core.Domain.RepositoriesContract
{
    public interface IUserContext
    {
        Task<ApplicationUser?> GetCurrentUserAsync();
    }
}
