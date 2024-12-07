using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Dtos
{
    public class RangeDto
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }

        public PaginationDto Pagination { get; set; }
    }
}
