using E_Commerce.Core.Helper;

namespace E_Commerce.Core.Dtos.AuthenticationDto
{
    public class RegisterDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RolesOption Role { get; set; } = RolesOption.USER;
    }

}
