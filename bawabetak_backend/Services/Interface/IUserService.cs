
namespace bawabetak_backend.Services.Interface
{
    public interface IUserService
    {
        Task SendVerificationCodeAsync(SendVerificationDto dto);
        Task VerifyCodeAsync(VerifyCodeDto dto);

        Task<IdentityResult> RegisterAsync(RegisterDto dto);
        Task<AuthTokenResponseDto> LoginAsync(LoginDto dto);
        Task ForgetPasswordAsync(ForgetPasswordDto dto);
        Task<IdentityResult> ResetPasswordAsync(ChangePasswordDto dto);
        Task<AuthTokenResponseDto> RefreshTokenAsync(RefreshTokenDto dto);
    }
}
