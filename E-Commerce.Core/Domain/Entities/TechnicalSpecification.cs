using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Domain.Entities
{
    public class TechnicalSpecification
    {
        public Guid TechnicalSpecificationID { get; set; } = Guid.NewGuid();
        public string SpecificationKey { get; set; }
        public string SpecificationValue { get; set; }

        public Guid ProductID { get; set; }
        public virtual Product Product { get; set; }
    }
}
