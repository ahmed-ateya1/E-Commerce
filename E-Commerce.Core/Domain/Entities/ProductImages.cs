using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Domain.Entities
{
    public class ProductImages
    {
        public Guid ProductImageID { get; set; } = Guid.NewGuid();
        public string ImageURL { get; set; }
        public Guid ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}
