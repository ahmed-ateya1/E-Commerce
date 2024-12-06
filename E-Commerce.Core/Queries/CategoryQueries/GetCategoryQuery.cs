using E_Commerce.Core.Dtos.CategoryDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Queries.CategoryQueries
{
    public class GetCategoryQuery : IRequest<CategoryResponse>
    {
        public Guid CategoryID { get; set; }
        public GetCategoryQuery(Guid categoryID)
        {
            CategoryID = categoryID;
        }
    }
}
