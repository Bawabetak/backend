namespace bawabetak_backend.Strategies.Refresh.Interface
{
    public interface IRefreshTokenFactory
    {
        IRefreshTokenStrategy GetStrategy(string clientType);
    }
}