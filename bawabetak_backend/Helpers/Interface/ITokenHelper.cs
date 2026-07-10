namespace bawabetak_backend.Helpers.Interface
{
    public interface ITokenHelper
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
        string GenerateRefreshToken();
    }
}
