namespace bawabetak_backend.Strategies.SendVerification.Interface
{
    public interface ISendVerificationStrategy
    {
        VerficationType VerificationType { get; }
        public Task SendVerficationCode(string email);
    }
}
