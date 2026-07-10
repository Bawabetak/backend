namespace bawabetak_backend.Strategies.SendVerification.Interface
{
        public interface ISendVerificationFactory
        {
            ISendVerificationStrategy GetStrategy(VerficationType type);
        }
    
}
