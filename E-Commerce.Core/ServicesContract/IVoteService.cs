using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos.VoteDto;
using System.Linq.Expressions;

namespace E_Commerce.Core.ServicesContract
{
    public interface IVoteService
    {
        Task<VoteResponse?> UpVoteAsync(VoteAddRequest? request);
        Task<VoteResponse?> DownVoteAsync(VoteAddRequest? request);
        Task<IEnumerable<VoteResponse>> GetAllAsync(Expression<Func<Vote, bool>>? predicate = null);
    }
}
