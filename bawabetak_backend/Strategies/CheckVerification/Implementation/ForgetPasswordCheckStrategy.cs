

namespace bawabetak_backend.Strategies.CheckVerification.Implementation
{
    public class ForgetPasswordCheckStrategy : ICheckVerificationStrategy
    {
        public VerficationType VerificationType => VerficationType.forgetpassword;
        private readonly ICacheHelper _cacheHelper;

        public ForgetPasswordCheckStrategy(ICacheHelper cacheHelper)
        {
            _cacheHelper = cacheHelper;
        }

        public async Task VerifyCodeAsync(string email, string code)
        {
            var cacheKey = $"forgetPassword_{email}";
            var savedCode = await _cacheHelper.GetAsync<string>(cacheKey);

            if (string.IsNullOrEmpty(savedCode) || savedCode != code)
            {
               throw new BadRequestCustomException(ResponseKeys.InvalidVerificationCode);
            }
            var approvalKey = $"ResetApproved_{email}";
            await _cacheHelper.SetAsync(approvalKey, "true", TimeSpan.FromMinutes(2));


            await _cacheHelper.RemoveAsync(cacheKey);

           
        }

    }
}