using E_Commerce.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Domain.Entities
{
    public class Vote
    {
        public Guid VoteID { get; set; } = Guid.NewGuid();
        public Guid ReviewID { get; set; }
        public virtual Review Review { get; set; }
        public Guid UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
