using E_Commerce.Core.Domain.IdentityEntities;
using E_Commerce.Core.Domain.RepositoriesContract;
using E_Commerce.Core.Dtos.AuthenticationDto;
using E_Commerce.Core.ServicesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    /// <summary>
    /// Manages user accounts, including registration, login, password reset, roles, and token handling.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="authenticationServices">Authentication service.</param>
        /// <param name="userManager">User manager service.</param>
        /// <param name="emailSender">Email sender service.</param>
        /// <param name="signInManager">Sign-in manager service.</param>
        /// <param name="unitOfWork">Unit of work service.</param>
        /// <param name="passwordHasher">Password hasher service.</param>
        public AccountController(IAuthenticationServices authenticationServices, UserManager<ApplicationUser> userManager, IEmailSender emailSender, SignInManager<ApplicationUser> signInManager, IUnitOfWork unitOfWork, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _authenticationServices = authenticationServices;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Registers a new client account.
        /// </summary>
        /// <param name="registerDTO">Client registration details.</param>
        /// <returns>Authentication response with token and status.</returns>
        /// <response code="200">Client registered successfully.</response>
        /// <response code="400">Invalid input or request.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("register-Client")]
        public async Task<ActionResult<AuthenticationResponse>> RegisterCleintAsync([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("|", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return BadRequest(errors);
            }

            var result = await _authenticationServices.RegisterClientAsync(registerDTO);
            if (!result.IsAuthenticated)
                return Problem(result.Message);

            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                SetRefreshToken(result.RefreshToken, result.RefreshTokenExpiration);
            }

            return Ok(result);
        }


        /// <summary>
        /// Logs in a user or company.
        /// </summary>
        /// <param name="loginDTO">Login credentials (email, password).</param>
        /// <returns>Authentication response with token and status.</returns>
        /// <response code="200">Login successful.</response>
        /// <response code="400">Invalid credentials or input.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> LoginAsync([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("|", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                return BadRequest(errors);
            }

            var result = await _authenticationServices.LoginAsync(loginDTO);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                SetRefreshToken(result.RefreshToken, result.RefreshTokenExpiration);
            }
            return Ok(result);
        }

        /// <summary>
        /// Sends a password reset OTP to the user's email.
        /// </summary>
        /// <param name="forgotPassword">Email to send the password reset link to.</param>
        /// <returns>Status message.</returns>
        /// <response code="200">Password reset OTP sent successfully.</response>
        /// <response code="400">Invalid input.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authenticationServices.ForgotPassword(forgotPassword);
            return Ok("If the email is associated with an account, an OTP will be sent.");
        }


        /// <summary>
        /// Resets the user's password using an OTP.
        /// </summary>
        /// <param name="resetPassword">Reset password request details, including OTP.</param>
        /// <returns>Status message.</returns>
        /// <response code="200">Password reset successfully.</response>
        /// <response code="400">Invalid OTP or request.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(resetPassword.Email!);
            if (user == null)
                return BadRequest("Invalid Request");

            var passwordHasher = new PasswordHasher<ApplicationUser>();
            var hashedPassword = passwordHasher.HashPassword(user, resetPassword.Password!);


            user.PasswordHash = hashedPassword;


            user.OTPCode = null;
            user.OTPExpiration = null;
            await _userManager.UpdateAsync(user);

            return Ok("Password has been reset successfully.");
        }



        /// <summary>
        /// Changes the user's password.
        /// </summary>
        /// <param name="model">Change password request details.</param>
        /// <returns>Status message.</returns>
        /// <response code="200">Password changed successfully.</response>
        /// <response code="400">Invalid current password.</response>
        /// <response code="404">User not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return BadRequest(new { message = "User not found" });
            }
            var user = await _userManager.FindByEmailAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.OldPassword);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest(new { message = "Current password is incorrect" });
            }

            var newHashedPassword = _passwordHasher.HashPassword(user, model.NewPassword);
            user.PasswordHash = newHashedPassword;

            await _userManager.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            return Ok(new { message = "Password changed successfully" });
        }

        /// <summary>
        /// Checks if the email is already in use.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>True if the email is in use; otherwise, false.</returns>
        /// <response code="200">Check successful, result returned.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("check-email")]
        public async Task<IActionResult> IsEmailInUse([FromQuery] string email)
        {
            var isInUse = await _unitOfWork.Repository<ApplicationUser>().AnyAsync(u => u.Email == email);
            return Ok(new { isInUse });
        }
     
        /// <summary>
        /// Verifies the OTP code.
        /// </summary>
        /// <param name="request">OTP verification request.</param>
        /// <returns>Status message.</returns>
        /// <response code="200">OTP verified successfully.</response>
        /// <response code="400">Invalid or expired OTP.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationRequest request)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest("Invalid email.");
            }

            if (user.OTPCode != request.Otp || user.OTPExpiration < DateTime.UtcNow)
            {
                return BadRequest("Invalid or expired OTP.");
            }

            user.EmailConfirmed = true;
            user.OTPCode = null;
            user.OTPExpiration = null;
            await _userManager.UpdateAsync(user);

            return Ok("Verify successfully.");
        }

        /// <summary>
        /// Adds a new role to the user.
        /// </summary>
        /// <param name="model">Role details to assign.</param>
        /// <returns>Status message.</returns>
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authenticationServices.AddRoleToUserAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

        /// <summary>
        /// Refreshes the user's authentication token.
        /// </summary>
        /// <returns>New authentication token.</returns>
        [HttpGet("refreshtoken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Refresh token is missing or invalid.");

            var result = await _authenticationServices.RefreshTokenAsync(refreshToken);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshToken(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }

        /// <summary>
        /// Revokes a refresh token.
        /// </summary>
        /// <param name="revokTokenDTO">Token details to revoke.</param>
        /// <returns>Status message.</returns>
        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokTokenDTO revokTokenDTO)
        {
            var token = revokTokenDTO.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _authenticationServices.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Invalid token");

            return Ok();
        }

        /// <summary>
        /// Sets the refresh token cookie in the response.
        /// </summary>
        /// <param name="refreshToken">The refresh token string.</param>
        /// <param name="expires">The expiration time for the token.</param>
        private void SetRefreshToken(string refreshToken, DateTime expires)
        {
            var cookieOption = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = expires
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOption);
        }
    }
}
