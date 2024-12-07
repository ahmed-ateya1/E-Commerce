using E_Commerce.Core.Dtos.BrandDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Queries.BrandQueries
{
    public class GetAllBrandwithNameQuery : IRequest<IEnumerable<BrandResponse>>
    {
        public string Name { get;}
        public GetAllBrandwithNameQuery(string name)
        {
            Name = name;
        }
    }
}
