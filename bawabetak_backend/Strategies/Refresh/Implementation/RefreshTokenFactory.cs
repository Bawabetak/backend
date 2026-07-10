
namespace bawabetak_backend.Strategies.Refresh.Implementation
{
    public class RefreshTokenFactory : IRefreshTokenFactory
    {
        private readonly IEnumerable<IRefreshTokenStrategy> _strategies;
        public RefreshTokenFactory(IEnumerable<IRefreshTokenStrategy> strategies)
        {
            _strategies = strategies;
        }
        public IRefreshTokenStrategy GetStrategy(string clientType)
        {
            var strategy = _strategies.FirstOrDefault(s => s.ClientType.Equals(clientType, StringComparison.OrdinalIgnoreCase));
            if (strategy == null)
            {
                throw new NotFoundCustomException(ResponseKeys.StrategyNotFound);
            }
            return strategy;
        }
    }
}