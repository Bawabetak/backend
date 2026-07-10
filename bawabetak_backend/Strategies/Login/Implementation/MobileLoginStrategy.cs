
namespace bawabetak_backend.Strategies.Login.Implementation
{
    public class MobileLoginStrategy : ILoginStrategy
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IHashHelper _hashHelper;

        public MobileLoginStrategy(ITokenHelper tokenHelper, IRefreshTokenRepository refreshTokenRepository, IHashHelper hashHelper)
        {
            _tokenHelper = tokenHelper;
            _refreshTokenRepository = refreshTokenRepository;
            _hashHelper = hashHelper;
        }

        public string ClientType => "Mobile";

        public async Task<AuthTokenResponseDto> Login(ApplicationUser user, IList<string> roles)
        {

            var accessToken = _tokenHelper.GenerateToken(user, roles);
            var refreshToken = _tokenHelper.GenerateRefreshToken();
            var hashedRefreshToken = _hashHelper.HashText(refreshToken);
            var refreshTokenEntity = new RefreshToken
            {
                Token = hashedRefreshToken,
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(7) 
            };
            await _refreshTokenRepository.AddAsync(refreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync();
            return new AuthTokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
