using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.ReviewDto;
using E_Commerce.Core.Helper;
using E_Commerce.Core.ServicesContract;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace E_Commerce.Core.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ReviewService> _logger;
        private readonly IUserContext _userContext;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ReviewService> logger, IUserContext userContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userContext = userContext;
        }

        private void LogEntityNotFound(string entityName, Guid id)
        {
            _logger.LogWarning("{EntityName} with ID {ID} not found.", entityName, id);
        }

        private async Task ValidateParentReviewAsync(Guid? parentReviewID)
        {
            if (parentReviewID.HasValue)
            {
                var parentReview = await _unitOfWork.Repository<Review>().GetByAsync(x => x.ReviewID == parentReviewID);
                if (parentReview == null)
                {
                    LogEntityNotFound("Parent review", parentReviewID.Value);
                    throw new ArgumentNullException(nameof(parentReview));
                }
            }
        }

        private async Task HandlerVotedAsync(IEnumerable<ReviewResponse> reviews, Guid userID)
        {
            var reviewsIds = reviews.Select(x => x.ReviewID).ToList();
            var votes = await _unitOfWork.Repository<Vote>()
                .GetAllAsync(x => reviewsIds.Contains(x.ReviewID) && x.UserID == userID);

            var voteDictionary = votes.ToDictionary(x => x.ReviewID, x => x);

            foreach (var review in reviews)
            {
                if (voteDictionary.TryGetValue(review.ReviewID, out var vote))
                {
                    review.HasUpVoted = vote.VoteType == VoteType.UPVOTE;
                    review.HasDownVoted = vote.VoteType == VoteType.DOWNVOTE;
                }
                else
                {
                    review.HasUpVoted = false;
                    review.HasDownVoted = false;
                }
            }
        }


        private async Task<Product> GetProductAsync(Guid productID)
        {
            var product = await _unitOfWork.Repository<Product>().GetByAsync(x => x.ProductID == productID);
            if (product == null)
            {
                LogEntityNotFound("Product", productID);
                throw new ArgumentNullException(nameof(product));
            }
            return product;
        }
        private async Task ExecuteWithTransactionAsync(Func<Task> action)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await action();
                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("Transaction committed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the transaction. Rolling back.");
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<ReviewResponse?> CreateAsync(ReviewAddRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            ValidationHelper.ValidateModel(request);

            var user = await _userContext.GetCurrentUserAsync();
            if (user == null)
            {
                _logger.LogWarning("No user is authenticated.");
                return null;
            }
            var product = await GetProductAsync(request.ProductID);
            await ValidateParentReviewAsync(request.ParentReviewID);

            var review = _mapper.Map<Review>(request);
            review.UserID = user.Id;

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Review>().CreateAsync(review);
                product.TotalReviews++;
                await _unitOfWork.CompleteAsync();
            });

            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<bool> DeleteAsync(Guid reviewID)
        {
            var review = await _unitOfWork.Repository<Review>().GetByAsync(x => x.ReviewID == reviewID);
            if (review == null)
            {
                LogEntityNotFound("Review", reviewID);
                return false;
            }

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Review>().DeleteAsync(review);
                await _unitOfWork.CompleteAsync();
            });

            return true;
        }

        public async Task<PaginatedResponse<ReviewResponse>> GetAllAsync
            (Expression<Func<Review, bool>>? expression = null,PaginationDto? pagination = null)
        {
            pagination ??= new PaginationDto();

            _logger.LogInformation(
                "Fetching all Reviews with pagination: PageIndex={PageIndex}, PageSize={PageSize}",
                pagination.PageIndex, pagination.PageSize);

            var totalReviews = await _unitOfWork.Repository<Review>().CountAsync(expression);

            if (totalReviews == 0)
            {
                return new PaginatedResponse<ReviewResponse>
                {
                    Items = new List<ReviewResponse>(),
                    TotalCount = 0,
                    PageIndex = pagination.PageIndex,
                    PageSize = pagination.PageSize
                };
            }

            var reviews = await _unitOfWork.Repository<Review>()
                .GetAllAsync(
                    expression,
                    includeProperties: "User,ParentReview",
                    sortBy: pagination.SortBy ?? "ReviewDate",
                    sortDirection: pagination.SortDirection,
                    pageSize: pagination.PageSize,
                    pageIndex: pagination.PageIndex);

            var response = _mapper.Map<IEnumerable<ReviewResponse>>(reviews);

            var user = await _userContext.GetCurrentUserAsync();
            if (user != null)
            {
                await HandlerVotedAsync(response, user.Id);
            }

            return new PaginatedResponse<ReviewResponse>
            {
                Items = response,
                TotalCount = totalReviews,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize
            };
        }


        public async Task<ReviewResponse?> GetByAsync(Expression<Func<Review, bool>> expression, bool isTracked = false)
        {
            var review = await _unitOfWork.Repository<Review>().GetByAsync(expression, isTracked, includeProperties: "User,Product,ChildReviews,ParentReview");

            if (review == null)
            {
                _logger.LogWarning("Review not found.");
                return null;
            }

            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<ReviewResponse?> UpdateAsync(ReviewUpdateRequest? request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            ValidationHelper.ValidateModel(request);

            var oldReview = await _unitOfWork.Repository<Review>().GetByAsync(x => x.ReviewID == request.ReviewID, includeProperties: "User,ParentReview,ChildReviews");
            if (oldReview == null)
            {
                LogEntityNotFound("Review", request.ReviewID);
                return null;
            }

            _mapper.Map(request, oldReview);

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Review>().UpdateAsync(oldReview);
                await _unitOfWork.CompleteAsync();
            });

            return _mapper.Map<ReviewResponse>(oldReview);
        }
    }
}
