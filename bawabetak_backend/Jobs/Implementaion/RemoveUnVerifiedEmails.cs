

namespace bawabetak_backend.Jobs.Implementaion
{
    public class RemoveUnVerifiedEmailsJob : IRemoveUnVerifiedEmailsJob
    {
        private readonly IUserRepository _userRepository;
        public RemoveUnVerifiedEmailsJob(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RemoveUnVerifiedEmails()
        {
            await _userRepository.RemoveUnVerifiedEmails();
        }
    }
}
