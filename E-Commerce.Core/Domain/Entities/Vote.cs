using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Core.Helper;
namespace E_Commerce.Core.Domain.Entities
{
    public class Vote
    {
        public Guid VoteID { get; set; } = Guid.NewGuid();
       public VoteType VoteType { get; set; } = VoteType.NONE;
        public Guid ReviewID { get; set; }
        public virtual Review Review { get; set; }
        public Guid UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
