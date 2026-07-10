namespace bawabetak_backend.Strategies.CheckVerification.Interface
{
    public interface ICheckVerificationStrategy
        {
            VerficationType VerificationType { get; }
            Task VerifyCodeAsync(string email, string code);
        }
    
}
