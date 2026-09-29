

namespace bawabetak_backend.Strategies.SendVerification.Implementation
{

    public class ForgetPasswordSendStrategy : ISendVerificationStrategy
    {
        public VerficationType VerificationType => VerficationType.forgetpassword;
        private readonly ICacheHelper _cacheHelper;
        private readonly IEmailSenderHelper _emailSenderHelper;
        private readonly IUserRepository _userRepository;
        private readonly IHashHelper _hashHelper;

        public ForgetPasswordSendStrategy(ICacheHelper cacheHelper, IEmailSenderHelper emailSenderHelper,
            IUserRepository userRepository,IHashHelper hashHelper)
        {
            _cacheHelper = cacheHelper;
            _emailSenderHelper = emailSenderHelper;
            _userRepository = userRepository;
            _hashHelper=hashHelper;
        }

        public async Task SendVerficationCode(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                throw new NotFoundCustomException(ResponseKeys.UserNotFound);
            }

            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            var hashedCode = _hashHelper.HashText(code);
            var cacheKey = $"forgetPassword_{email}";
           await _cacheHelper.SetAsync(cacheKey, hashedCode, TimeSpan.FromMinutes(1));
            await _emailSenderHelper.SendEmailAsync(email, "Forget Password Verification Code", $"Your verification code is: {code}");
        }
    }
}
