

namespace bawabetak_backend.Strategies.CheckVerification.Implementation
{
    public class ResetPasswordCheckStrategy : ICheckVerificationStrategy
    {
        public VerficationType VerificationType => VerficationType.resetPassword;
        private readonly ICacheHelper _cacheHelper;

        public ResetPasswordCheckStrategy(ICacheHelper cacheHelper)
        {
            _cacheHelper = cacheHelper;
        }

        public async Task VerifyCodeAsync(string email, string code)
        {
            var cacheKey = $"resetPassword_{email}";
            var savedCode = await _cacheHelper.GetAsync<string>(cacheKey);

            if (string.IsNullOrEmpty(savedCode) || savedCode != code)
            {
               throw new BadRequestCustomException(ResponseKeys.InvalidVerificationCode);
            }


            await _cacheHelper.RemoveAsync(cacheKey);

           
        }

    }
}