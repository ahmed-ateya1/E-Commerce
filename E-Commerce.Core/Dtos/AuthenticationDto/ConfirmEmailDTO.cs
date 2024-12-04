using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Dtos.AuthenticationDto
{
    public class ConfirmEmailDTO
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
