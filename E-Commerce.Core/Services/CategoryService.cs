using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos.CategoryDto;
using E_Commerce.Core.Dtos;
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
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IFileServices _fileServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IMapper mapper,
            IFileServices fileServices,
            IUnitOfWork unitOfWork,
            ILogger<CategoryService> logger)
        {
            _mapper = mapper;
            _fileServices = fileServices;
            _unitOfWork = unitOfWork;
            _logger = logger;
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
        public async Task<CategoryResponse?> CreateAsync(CategoryAddRequest? request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            ValidationHelper.ValidateModel(request);

            Category parent = null;
            if (request.ParentCategoryID is not null)
            {
                parent = await _unitOfWork.Repository<Category>()
                    .GetByAsync(x => x.CategoryID == request.ParentCategoryID);
                if (parent is null)
                    throw new ArgumentNullException(nameof(parent));
            }
            var category = _mapper.Map<Category>(request);
            if (parent is not null)
                category.ParentCategory = parent;

            if (request.CategoryImage is not null)
            {
                try
                {
                    var categoryImageUrl = await _fileServices.CreateFile(request.CategoryImage);
                    category.CategoryImageURL = categoryImageUrl;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error uploading category image");
                    throw new InvalidOperationException("Failed to upload category image.", ex);
                }
            }

            await ExecuteWithTransaction(async () =>
            {
                await _unitOfWork.Repository<Category>().CreateAsync(category);
            });

            return _mapper.Map<CategoryResponse>(category);
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await _unitOfWork.Repository<Category>()
                .GetByAsync(x => x.CategoryID == id);

            if (category is null)
                return false;

            await ExecuteWithTransaction(async () =>
            {
                if(!string.IsNullOrEmpty(category.CategoryImageURL))
                    await _fileServices.DeleteFile(new Uri(category.CategoryImageURL).Segments.Last());
                await _unitOfWork.Repository<Category>().DeleteAsync(category);
            });
            return true;
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync(Expression<Func<Category, bool>>? predicate = null, PaginationDto? pagination = null)
        {
            pagination ??= new PaginationDto();

            var categories = await _unitOfWork.Repository<Category>()
                .GetAllAsync(predicate,includeProperties: "ParentCategory",pageIndex:pagination.PageIndex,pageSize:pagination.PageSize);
            if (!categories.Any())
                return [];

            return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<CategoryResponse?> GetByAsync(Expression<Func<Category, bool>> predict, bool isTracked = false)
        {
            var category = await _unitOfWork.Repository<Category>()
                .GetByAsync(predict, isTracked , includeProperties: "ParentCategory");
            if (category is null)
                return null;
            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse> UpdateAsync(CategoryUpdateRequest? request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            ValidationHelper.ValidateModel(request);

            var categoryOld = await _unitOfWork.Repository<Category>()
                .GetByAsync(x => x.CategoryID == request.CategoryID, includeProperties: "ParentCategory");

            if (categoryOld is null)
                throw new ArgumentNullException(nameof(categoryOld));

            if (request.CategoryImage is not null)
            {
                try
                {
                    var fileName = !string.IsNullOrEmpty(categoryOld.CategoryImageURL)
                        ? new Uri(categoryOld.CategoryImageURL).Segments.Last()
                        : Guid.NewGuid().ToString();

                    var categoryImageUrl = await _fileServices.UpdateFile(request.CategoryImage, fileName);

                    categoryOld.CategoryImageURL = categoryImageUrl;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating category image");
                    throw new InvalidOperationException("Failed to update category image.", ex);
                }
            }

            var category = _mapper.Map(request, categoryOld);

            await ExecuteWithTransaction(async () =>
            {
                await _unitOfWork.Repository<Category>().UpdateAsync(category);
            });

            return _mapper.Map<CategoryResponse>(category);
        }

    }
}
