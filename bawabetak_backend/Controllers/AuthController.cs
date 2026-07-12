using Microsoft.AspNetCore.Mvc;

namespace bawabetak_backend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("send-verification")]
        public async Task<IActionResult> SendVerificationCode([FromBody] SendVerificationDto dto)
        {
            await _userService.SendVerificationCodeAsync(dto);
            return Ok(ResponseHelper.Success(ResponseKeys.SendVerificationSuccess));
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto dto)
        {
            await _userService.VerifyCodeAsync(dto);
            return Ok(ResponseHelper.Success(ResponseKeys.CodeVerifiedSuccessfully));
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromForm] RegisterDto dto)
        {
          var result=  await _userService.RegisterAsync(dto);
            if(!result.Succeeded)
            {
                return BadRequest(ResponseHelper.Error(ResponseKeys.RegistrationFailed, result.Errors.Select(e => e.Description).ToList()));
            }
            return Ok(ResponseHelper.Success(ResponseKeys.UserRegisteredSuccessfully));
        }
        [HttpPost("register-complete")]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> CompleteRegister([FromForm] CompleteRegisterDto dto)
        {
            await _userService.CompleteRegister(dto);
            return Ok(ResponseHelper.Success(ResponseKeys.UserCompleteRegisterSuccessfully));
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _userService.LoginAsync(dto);
            return Ok(ResponseHelper.Success(ResponseKeys.UserLoggedInSuccessfully, result));
        }

        [HttpPost("Forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto dto)
        {
            await _userService.ForgetPasswordAsync(dto);
            return Ok(ResponseHelper.Success(ResponseKeys.passwordResetSuccess));
        }

        [HttpPost("Change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
           var result= await _userService.ResetPasswordAsync(dto);
            if(!result.Succeeded)
            {
                return BadRequest(ResponseHelper.Error(ResponseKeys.PasswordChangeFailed, result.Errors.Select(e => e.Description).ToList()));
            }
            return Ok(ResponseHelper.Success(ResponseKeys.passwordChangeSuccess));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto? dto)
        {
            var result = await _userService.RefreshTokenAsync(dto ?? new RefreshTokenDto());
            return Ok(ResponseHelper.Success(ResponseKeys.TokenRefreshedSuccessfully, result));
        }
    }
}