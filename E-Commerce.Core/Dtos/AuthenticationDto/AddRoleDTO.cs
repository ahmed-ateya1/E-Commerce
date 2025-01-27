using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Dtos.AuthenticationDto
{
    public class AddRoleDTO
    {
        public Guid UserID { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
