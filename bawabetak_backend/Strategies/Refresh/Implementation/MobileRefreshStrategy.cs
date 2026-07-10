namespace bawabetak_backend.Strategies.Refresh.Implementation
{
    public class MobileRefreshTokenStrategy : IRefreshTokenStrategy
    {
        private readonly IRefreshTokenCoreService _coreService;
        public string ClientType => "Mobile";

        public MobileRefreshTokenStrategy(IRefreshTokenCoreService coreService)
        {
            _coreService = coreService;
        }

        public async Task<AuthTokenResponseDto> RefreshAsync(string? refreshTokenFromBody)
        {
            if (string.IsNullOrEmpty(refreshTokenFromBody))
            {
                throw new BadRequestCustomException(ResponseKeys.InvalidRefreshToken);
            }

           return await _coreService.RotateAsync(refreshTokenFromBody);

           
        }

     
    }
}