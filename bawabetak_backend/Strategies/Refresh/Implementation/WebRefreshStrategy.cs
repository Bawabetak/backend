namespace bawabetak_backend.Strategies.Refresh.Implementation
{
    public class WebRefreshTokenStrategy : IRefreshTokenStrategy
    {
        private readonly IRefreshTokenCoreService _coreService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string ClientType => "Web";

        public WebRefreshTokenStrategy(IRefreshTokenCoreService coreService, IHttpContextAccessor httpContextAccessor)
        {
            _coreService = coreService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthTokenResponseDto> RefreshAsync(string? refreshTokenFromBody)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var oldRefreshToken = httpContext?.Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(oldRefreshToken))
            {
                throw new BadRequestCustomException(ResponseKeys.InvalidRefreshToken);
            }

            var auth= await _coreService.RotateAsync(oldRefreshToken);

            if (httpContext != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(7)
                };
                httpContext.Response.Cookies.Append("refreshToken", auth.RefreshToken, cookieOptions);
            }

            return new AuthTokenResponseDto
            {
                AccessToken = auth.AccessToken
            };
        }
    }
}