
namespace bawabetak_backend.Repositories.Implementation
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        private readonly Context _context;

        public RefreshTokenRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken> GetByTokenHashAsync(string Hashedtoken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == Hashedtoken);
        }

        public async Task RevokeAllUserTokensAsync(string userId)
        {
            await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ExecuteUpdateAsync(rt => rt.SetProperty(r => r.IsRevoked, true));
        }
    }
}
