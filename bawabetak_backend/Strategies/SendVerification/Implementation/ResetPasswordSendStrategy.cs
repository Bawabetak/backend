

namespace bawabetak_backend.Strategies.SendVerification.Implementation
{

    public class ResetPasswordSendStrategy : ISendVerificationStrategy
    {
        public VerficationType VerificationType => VerficationType.resetPassword;
        private readonly ICacheHelper _cacheHelper;
        private readonly IEmailSenderHelper _emailSenderHelper;
        private readonly IUserRepository _userRepository;

        public ResetPasswordSendStrategy(ICacheHelper cacheHelper, IEmailSenderHelper emailSenderHelper, IUserRepository userRepository)
        {
            _cacheHelper = cacheHelper;
            _emailSenderHelper = emailSenderHelper;
            _userRepository = userRepository;
        }

        public async Task SendVerficationCode(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundCustomException(ResponseKeys.UserNotFound);
            }

            var code = new Random().Next(100000, 999999).ToString();
            var cacheKey = $"resetPassword_{email}";
            _cacheHelper.SetAsync(cacheKey, code, TimeSpan.FromMinutes(1));
            await _emailSenderHelper.SendEmailAsync(email, "Reset Password Verification Code", $"Your verification code is: {code}");
        }
    }
}
