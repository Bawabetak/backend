
using bawabetak_backend.Helpers.Implementation;

namespace bawabetak_backend.Strategies.Login.Implementation
{
    public class WebLoginStrategy : ILoginStrategy
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHashHelper _hashHelper;

        public string ClientType => "Web";
        public WebLoginStrategy(ITokenHelper tokenHelper, IRefreshTokenRepository refreshTokenRepository,
            IHashHelper hashHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _tokenHelper = tokenHelper;
            _refreshTokenRepository = refreshTokenRepository;
            _httpContextAccessor = httpContextAccessor;
            _hashHelper = hashHelper;
        }

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
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                };

                httpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            }
            return  new AuthTokenResponseDto
            {
                AccessToken = accessToken
               
            };

        }
    }
}
