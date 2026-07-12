
namespace bawabetak_backend.Jobs.Implementaion
{
    public class RegisterVerificationJob : IRegisterVerificationJob
    {
        private readonly ISendVerificationFactory _factory;

        public RegisterVerificationJob(ISendVerificationFactory factory)
        {
            _factory = factory;
        }

        public async Task SendAsync(string email)
        {
            var strategy = _factory.GetStrategy(VerficationType.register);

            await strategy.SendVerficationCode(email);
        }
    }
}
