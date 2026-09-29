using bawabetak_backend.Strategies.CheckVerification.Interface;

namespace bawabetak_backend.Strategies.CheckVerification.Implementation
{
    public class RegisterCheckStrategy : ICheckVerificationStrategy
    {
        public VerficationType VerificationType => VerficationType.register;
        private readonly ICacheHelper _cacheHelper;
        private readonly IUserRepository _userRepository;
        private readonly IHashHelper _hashHelper;

        public RegisterCheckStrategy(ICacheHelper cacheHelper, IUserRepository userRepository, IHashHelper hashHelper)
        {
            _cacheHelper = cacheHelper;
            _userRepository = userRepository;
            _hashHelper = hashHelper;
        }

        public async Task VerifyCodeAsync(string email, string code)
        {
            var cacheKey = $"register_{email}";
            var savedCode = await _cacheHelper.GetAsync<string>(cacheKey);

            if (string.IsNullOrEmpty(savedCode) )
            {
                throw new BadRequestCustomException(ResponseKeys.InvalidVerificationCode);
            }

            var isSameCode = _hashHelper.VerifyHash(code, savedCode);
            if (!isSameCode)
            {
                throw new BadRequestCustomException(ResponseKeys.InvalidVerificationCode);

            }

            await _cacheHelper.RemoveAsync(cacheKey);

            await _cacheHelper.SetAsync($"RegisterApproved_{email}", true, TimeSpan.FromMinutes(10));
            await _userRepository.MarkAsVerifiedAsync(email);

        }
    }
}