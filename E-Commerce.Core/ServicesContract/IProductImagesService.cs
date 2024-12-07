using Microsoft.AspNetCore.Http;

namespace E_Commerce.Core.ServicesContract
{
    public interface IProductImagesService
    {
        Task<IEnumerable<string>> SaveImageAsync(Guid productID , IEnumerable<IFormFile>? images);
        Task<bool> DeleteAllImageAsync(Guid? ProductID);
    }
}
