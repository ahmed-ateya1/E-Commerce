using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Dtos
{
    public class PaginationDto
    {
        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }
}
