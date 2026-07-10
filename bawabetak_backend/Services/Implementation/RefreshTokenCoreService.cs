namespace bawabetak_backend.Services.Implementation
{
    public class RefreshTokenCoreService : IRefreshTokenCoreService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ITokenHelper _tokenHelper;
        private readonly IHashHelper _hashHelper;

        public RefreshTokenCoreService(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ITokenHelper tokenHelper,
            IHashHelper hashHelper)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tokenHelper = tokenHelper;
            _hashHelper = hashHelper;
        }

        public async Task<AuthTokenResponseDto> RotateAsync(string oldRefreshTokenPlain)
        {
            if (string.IsNullOrEmpty(oldRefreshTokenPlain))
            {
                throw new BadRequestCustomException(ResponseKeys.InvalidRefreshToken);
            }

            var incomingHash = _hashHelper.HashText(oldRefreshTokenPlain);
            var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(incomingHash);

            if (storedToken == null)
            {
                throw new BadRequestCustomException(ResponseKeys.InvalidRefreshToken);
            }


            if (storedToken.IsRevoked)
            {
                await _refreshTokenRepository.RevokeAllUserTokensAsync(storedToken.UserId);
                throw new BadRequestCustomException(ResponseKeys.RefreshTokenReused);
            }

            if (storedToken.Expires < DateTime.UtcNow)
            {
                throw new BadRequestCustomException(ResponseKeys.RefreshTokenExpired);
            }

            var user = await _userRepository.GetUserByIdAsync(storedToken.UserId);
            if (user == null)
            {
                throw new NotFoundCustomException(ResponseKeys.UserNotFound);
            }

              storedToken.IsRevoked = true;
            _refreshTokenRepository.Update(storedToken);

            var newRefreshTokenPlain = _tokenHelper.GenerateRefreshToken();
            var newRefreshTokenEntity = new RefreshToken
            {
                Token = _hashHelper.HashText(newRefreshTokenPlain),
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(7),
                 IsRevoked = false
            };
            await _refreshTokenRepository.AddAsync(newRefreshTokenEntity);
            await _refreshTokenRepository.SaveChangesAsync(); 

            var roles = await _roleRepository.GetRolesByUserAsync(user);
            var accessToken = _tokenHelper.GenerateToken(user, roles);

            return new AuthTokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshTokenPlain
            };
        }

   
    }
}