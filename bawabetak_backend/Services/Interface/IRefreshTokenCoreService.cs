namespace bawabetak_backend.Services.Interface
{
    public interface IRefreshTokenCoreService
    {
        Task<AuthTokenResponseDto> RotateAsync(string oldRefreshTokenPlain);
    }
}
