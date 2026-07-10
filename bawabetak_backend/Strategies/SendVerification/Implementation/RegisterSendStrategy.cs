namespace bawabetak_backend.Strategies.SendVerification.Implementation
{
    public class RegisterSendStrategy : ISendVerificationStrategy
    {
        public VerficationType VerificationType => VerficationType.register;
        private readonly ICacheHelper _cacheHelper;
        private readonly IEmailSenderHelper _emailSenderHelper;

        public RegisterSendStrategy(ICacheHelper cacheHelper, IEmailSenderHelper emailSenderHelper)
        {
            _cacheHelper = cacheHelper;
            _emailSenderHelper = emailSenderHelper;
        }

        public Task SendVerficationCode(string email)
        {
            var code = new Random().Next(100000, 999999).ToString();
            var cacheKey = $"register_{email}";
            _cacheHelper.SetAsync(cacheKey, code, TimeSpan.FromMinutes(1));
            return _emailSenderHelper.SendEmailAsync(email, "Registration Verification Code", $"Your verification code is: {code}");
        }
    }
}
