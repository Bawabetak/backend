namespace bawabetak_backend.Repositories.Interface
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        public Task<RefreshToken> GetByTokenHashAsync(string Hashedtoken);
        public Task RevokeAllUserTokensAsync(string userId);
    }
}
