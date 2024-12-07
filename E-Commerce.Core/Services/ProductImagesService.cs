using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;

namespace E_Commerce.Core.Services
{
    public class ProductImagesService : IProductImagesService
    {
        private readonly IFileServices _fileServices;
        private readonly IUnitOfWork _unitOfWork;

        public ProductImagesService(IFileServices fileServices, IUnitOfWork unitOfWork)
        {
            _fileServices = fileServices;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeleteAllImageAsync(Guid? ProductID)
        {
            if (ProductID == null)
                return false;

            var images = await _unitOfWork.Repository<ProductImages>()
                .GetAllAsync(x => x.ProductID == ProductID);

            if (!images.Any())
                return true;

            var fileNames = images
                .Select(image => !string.IsNullOrEmpty(image.ImageURL)
                    ? new Uri(image.ImageURL).Segments.Last()
                    : Guid.NewGuid().ToString())
                .ToList();


            var deleteFileTasks = fileNames.Select(fileName => _fileServices.DeleteFile(fileName));


            await Task.WhenAll(deleteFileTasks);
            await _unitOfWork.Repository<ProductImages>().RemoveRangeAsync(images);

            return true;
        }

        public async Task<IEnumerable<string>> SaveImageAsync(Guid productID, IEnumerable<IFormFile>? images)
        {
            var productExists = await _unitOfWork.Repository<Product>()
                .AnyAsync(x => x.ProductID == productID);

            if (!productExists)
                throw new ArgumentNullException(nameof(productID), "Product not found.");

            if (images is null || !images.Any())
                return Array.Empty<string>();

            var fileUrls = new ConcurrentBag<string>();
            var productImages = new ConcurrentBag<ProductImages>();


            var saveFileTasks = images.Select(async image =>
            {
                var fileUrl = await _fileServices.CreateFile(image);
                fileUrls.Add(fileUrl);

                productImages.Add(new ProductImages
                {
                    ProductID = productID,
                    ImageURL = fileUrl
                });
            });

            await Task.WhenAll(saveFileTasks);

            await _unitOfWork.Repository<ProductImages>().AddRangeAsync(productImages);

            return fileUrls.ToArray();
        }
    }
}
