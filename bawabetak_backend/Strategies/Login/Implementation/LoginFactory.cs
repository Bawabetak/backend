namespace bawabetak_backend.Strategies.Login.Implementation
{
    public class LoginFactory : ILoginFactory
    {
        private readonly IEnumerable<ILoginStrategy> _strategies;
        public LoginFactory(IEnumerable<ILoginStrategy> strategies)
        {
            _strategies = strategies;
        }
        public ILoginStrategy GetStrategy(string clientType)
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
