namespace bawabetak_backend.Strategies.Login.Interface
{
    public interface ILoginStrategy
    {
        string ClientType { get; }
        public Task<AuthTokenResponseDto> Login(ApplicationUser user, IList<string> roles);
    }
}
