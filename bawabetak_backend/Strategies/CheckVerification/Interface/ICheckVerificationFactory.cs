namespace bawabetak_backend.Strategies.CheckVerification.Interface
{
    public interface ICheckVerificationFactory
    {
        ICheckVerificationStrategy GetStrategy(VerficationType type);
    }
}