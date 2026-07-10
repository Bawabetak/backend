using bawabetak_backend.Strategies.CheckVerification.Interface;

namespace bawabetak_backend.Strategies.CheckVerification.Implementation
{
    public class RegisterCheckStrategy : ICheckVerificationStrategy
    {
        public VerficationType VerificationType => VerficationType.register;
        private readonly ICacheHelper _cacheHelper;
        private readonly IUserRepository _userRepository;

        public RegisterCheckStrategy(ICacheHelper cacheHelper, IUserRepository userRepository)
        {
            _cacheHelper = cacheHelper;
            _userRepository = userRepository;
        }

        public async Task VerifyCodeAsync(string email, string code)
        {
            var cacheKey = $"register_{email}";
            var savedCode = await _cacheHelper.GetAsync<string>(cacheKey);

            if (string.IsNullOrEmpty(savedCode) || savedCode != code)
            {
                throw new BadRequestCustomException(ResponseKeys.InvalidVerificationCode);
            }



            await _cacheHelper.RemoveAsync(cacheKey);

            await _cacheHelper.SetAsync($"RegisterApproved_{email}", "true", TimeSpan.FromMinutes(10));

        }
    }
}