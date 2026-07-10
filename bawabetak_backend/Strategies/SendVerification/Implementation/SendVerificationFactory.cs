
namespace bawabetak_backend.Strategies.SendVerification.Implementation
{
    public class SendVerificationFactory : ISendVerificationFactory
    {
        private readonly IEnumerable<ISendVerificationStrategy> _strategies;
        public SendVerificationFactory(IEnumerable<ISendVerificationStrategy> strategies)
        {
            _strategies = strategies;
        }
        public ISendVerificationStrategy GetStrategy(VerficationType type)
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
