using E_Commerce.Core.Dtos.BrandDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Queries.BrandQueries
{
    public class GetBrandQuery : IRequest<BrandResponse>
    {
        public Guid BrandID { get; set; }

        public GetBrandQuery(Guid brandID)
        {
            BrandID = brandID;
        }
    }
}
