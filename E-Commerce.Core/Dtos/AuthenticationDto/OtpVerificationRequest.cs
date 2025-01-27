namespace E_Commerce.Core.Dtos.AuthenticationDto
{
    public class OtpVerificationRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
