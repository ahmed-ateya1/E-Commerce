using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Domain.Entities
{
    public class Brand 
    {
        public Guid BrandID { get; set; } = Guid.NewGuid();
        public string BrandName { get; set; }
        public ICollection<Product> Products { get; set; } = [];
    }
}
