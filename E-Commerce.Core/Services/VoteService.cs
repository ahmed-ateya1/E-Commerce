﻿using AutoMapper;
using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos.VoteDto;
using E_Commerce.Core.Helper;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

public class VoteService : IVoteService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<VoteService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    public VoteService(
        IUnitOfWork unitOfWork,
        ILogger<VoteService> logger,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
        _userContext = userContext;
    }

    private async Task<Review> CheckIfReviewExistsAsync(Guid reviewID)
    {
        var review = await _unitOfWork.Repository<Review>().GetByAsync(x => x.ReviewID == reviewID);

        if (review == null)
        {
            _logger.LogError("Review not found: {ReviewID}", reviewID);
            throw new ArgumentNullException(nameof(review));
        }

        return review;
    }
    public async Task<VoteResponse?> UpVoteAsync(VoteAddRequest? request)
    {
        if (request == null)
        {
            _logger.LogError("VoteAddRequest is null");
            throw new ArgumentNullException(nameof(request));
        }

        ValidationHelper.ValidateModel(request);

        var user = await _userContext.GetCurrentUserAsync();
        if (user == null)
        {
            _logger.LogError("User not found");
            throw new ArgumentNullException(nameof(user));
        }

        var review = await CheckIfReviewExistsAsync(request.ReviewID);

        var existingVote = await _unitOfWork.Repository<Vote>().GetByAsync(
            x => x.ReviewID == request.ReviewID && x.UserID == user.Id);

        if (existingVote != null)
        {
            if (existingVote.VoteType == VoteType.UPVOTE)
            {
                await ExecuteWithTransactionAsync(async () =>
                {
                    await _unitOfWork.Repository<Vote>().DeleteAsync(existingVote);
                    review.TotalVotes--;
                    review.TotalUpVotes--; 
                    await _unitOfWork.CompleteAsync();
                });

                _logger.LogInformation("Upvote removed for ReviewID: {ReviewID} by UserID: {UserID}", request.ReviewID, user.Id);
                return null;
            }

            existingVote.VoteType = VoteType.UPVOTE;

            await ExecuteWithTransactionAsync(async () =>
            {
                review.TotalVotes++; 
                review.TotalUpVotes++; 
                review.TotalDownVotes--; 
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Vote updated to UPVOTE for ReviewID: {ReviewID} by UserID: {UserID}", request.ReviewID, user.Id);
            return _mapper.Map<VoteResponse>(existingVote);
        }

        var vote = _mapper.Map<Vote>(request);
        vote.UserID = user.Id;
        vote.ReviewID = review.ReviewID;
        vote.User = user;
        vote.VoteType = VoteType.UPVOTE;

        await ExecuteWithTransactionAsync(async () =>
        {
            await _unitOfWork.Repository<Vote>().CreateAsync(vote);
            review.TotalVotes++;
            review.TotalUpVotes++;
            await _unitOfWork.CompleteAsync();
        });

        _logger.LogInformation("Vote successfully added for ReviewID: {ReviewID} by UserID: {UserID}", request.ReviewID, user.Id);
        return _mapper.Map<VoteResponse>(vote);
    }


    public async Task<VoteResponse?> DownVoteAsync(VoteAddRequest? request)
    {
        if (request == null)
        {
            _logger.LogError("VoteAddRequest is null");
            throw new ArgumentNullException(nameof(request));
        }

        ValidationHelper.ValidateModel(request);

        var user = await _userContext.GetCurrentUserAsync();
        if (user == null)
        {
            _logger.LogError("User not found");
            throw new ArgumentNullException(nameof(user));
        }

        var vote = await _unitOfWork.Repository<Vote>().GetByAsync(
            x => x.ReviewID == request.ReviewID && x.UserID == user.Id, includeProperties: "Review");

        if (vote == null)
        {
            var review = await CheckIfReviewExistsAsync(request.ReviewID);
            var newVote = new Vote
            {
                UserID = user.Id,
                ReviewID = review.ReviewID,
                User = user,
                VoteType = VoteType.DOWNVOTE
            };

            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Vote>().CreateAsync(newVote);
                review.TotalVotes--;
                review.TotalDownVotes++; 
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Downvote added for ReviewID: {ReviewID} by UserID: {UserID}", request.ReviewID, user.Id);
            return _mapper.Map<VoteResponse>(newVote);
        }

        if (vote.VoteType == VoteType.DOWNVOTE)
        {
            await ExecuteWithTransactionAsync(async () =>
            {
                await _unitOfWork.Repository<Vote>().DeleteAsync(vote);
                vote.Review.TotalVotes++;
                vote.Review.TotalDownVotes--; 
                await _unitOfWork.CompleteAsync();
            });

            _logger.LogInformation("Downvote removed for ReviewID: {ReviewID} by UserID: {UserID}", request.ReviewID, user.Id);
            return null;
        }

        vote.VoteType = VoteType.DOWNVOTE;

        await ExecuteWithTransactionAsync(async () =>
        {
            vote.Review.TotalVotes--; 
            vote.Review.TotalDownVotes++; 
            vote.Review.TotalUpVotes--; 
            await _unitOfWork.CompleteAsync();
        });

        _logger.LogInformation("Vote updated to DOWNVOTE for ReviewID: {ReviewID} by UserID: {UserID}", request.ReviewID, user.Id);
        return _mapper.Map<VoteResponse>(vote);
    }


    public async Task<IEnumerable<VoteResponse>> GetAllAsync(Expression<Func<Vote, bool>>? predicate = null)
    {
        var votes = await _unitOfWork.Repository<Vote>()
            .GetAllAsync(predicate, includeProperties: "Review,User");

        if (votes == null || !votes.Any())
        {
            _logger.LogWarning("No votes found");
            return Enumerable.Empty<VoteResponse>();
        }

        return _mapper.Map<IEnumerable<VoteResponse>>(votes);
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
}
