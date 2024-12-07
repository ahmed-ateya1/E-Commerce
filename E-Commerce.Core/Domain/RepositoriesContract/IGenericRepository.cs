using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Domain.RepositoriesContract
{
    public interface IGenericRepository<T> where T : class
    {
        public  Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null,
            string includeProperties = "",
            Expression<Func<T, object>>? orderBy = null,
            int? pageIndex = null,
            int? pageSize = null);
        Task<T> GetByAsync(Expression<Func<T, bool>>? filter = null, bool isTracked = true, string includeProperties = "");
        Task<T> CreateAsync(T model);
        Task<long> CountAsync(Expression<Func<T, bool>>? filter = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        Task AddRangeAsync(IEnumerable<T> model);
        Task RemoveRangeAsync(IEnumerable<T> model);
        Task<bool> DeleteAsync(T model);
        Task<T> UpdateAsync(T model);
        void Detach(T entity);
        Task SaveAsync();
    }
}
