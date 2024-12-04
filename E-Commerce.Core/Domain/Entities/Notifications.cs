using E_Commerce.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Domain.Entities
{
    public class Notifications
    {
        public Guid NotificationID { get; set; } = Guid.NewGuid();
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string NotificationType { get; set; }
        public string ReferenceURL { get; set; }
        public Guid UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
