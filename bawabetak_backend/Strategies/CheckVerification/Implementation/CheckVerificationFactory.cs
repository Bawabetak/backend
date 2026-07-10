namespace bawabetak_backend.Strategies.CheckVerification.Implementation
{
    public class CheckVerificationFactory : ICheckVerificationFactory
    {
        private readonly IEnumerable<ICheckVerificationStrategy> _strategies;

        public CheckVerificationFactory(IEnumerable<ICheckVerificationStrategy> strategies)
        {
            _strategies = strategies;
        }

        public ICheckVerificationStrategy GetStrategy(VerficationType type)
        {
            var strategy = _strategies.FirstOrDefault(s => s.VerificationType == type);
            if (strategy == null)
            {
                throw new NotFoundCustomException(ResponseKeys.StrategyNotFound);
            }
            return strategy;
        }
    }
}