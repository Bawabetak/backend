namespace bawabetak_backend.Strategies.SendVerification.Implementation
{
    public class RegisterSendStrategy : ISendVerificationStrategy
    {
        public VerficationType VerificationType => VerficationType.register;
        private readonly ICacheHelper _cacheHelper;
        private readonly IEmailSenderHelper _emailSenderHelper;
        private readonly IHashHelper _hashHelper;

        public RegisterSendStrategy(ICacheHelper cacheHelper, IEmailSenderHelper emailSenderHelper, IHashHelper hashHelper)
        {
            _cacheHelper = cacheHelper;
            _emailSenderHelper = emailSenderHelper;
            _hashHelper = hashHelper;
        }

        public async Task SendVerficationCode(string email)
        {
            var code = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            var hashedCode = _hashHelper.HashText(code);
            var cacheKey = $"register_{email}";
           await _cacheHelper.SetAsync(cacheKey, hashedCode, TimeSpan.FromMinutes(1));
            await _emailSenderHelper.SendEmailAsync(email, "Registration Verification Code", $"Your verification code is: {code}");
        }
    }
}
