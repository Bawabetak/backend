namespace bawabetak_backend.Jobs.Interface
{
    public interface IRegisterVerificationJob
    {
        Task SendAsync(string email);
    }
}
