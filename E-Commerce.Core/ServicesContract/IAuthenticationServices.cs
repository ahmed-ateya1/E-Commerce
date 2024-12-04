using E_Commerce.Core.Dtos.AuthenticationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.ServicesContract
{
    public interface IAuthenticationServices
    {
        Task<AuthenticationResponse> RegisterClientAsync(RegisterDTO clientRegisterDTO);
        Task<AuthenticationResponse> LoginAsync(LoginDTO loginDTO);
        Task<AuthenticationResponse> RefreshTokenAsync(string token);
        Task<bool> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO);
        Task<bool> RevokeTokenAsync(string token);
        Task<string> AddRoleToUserAsync(AddRoleDTO model);
    }
}
