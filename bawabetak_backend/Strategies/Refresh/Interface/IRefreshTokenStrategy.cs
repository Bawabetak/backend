namespace bawabetak_backend.Strategies.Refresh.Interface
{
    public interface IRefreshTokenStrategy
    {
        string ClientType { get; }

        Task<AuthTokenResponseDto> RefreshAsync(string? refreshTokenFromBody);
    }
}
