using E_Commerce.Core.Domain.Entities;
using E_Commerce.Core.Dtos;
using E_Commerce.Core.Dtos.CategoryDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Queries.CategoryQueries
{
    public class GetAllCategoryQuery : IRequest<IEnumerable<CategoryResponse>>
    {
        public Expression<Func<Category, bool>>? Filter { get; set; }

        public GetAllCategoryQuery(Expression<Func<Category, bool>>? filter = null)
        {
            Filter = filter;
        }
    }
}
