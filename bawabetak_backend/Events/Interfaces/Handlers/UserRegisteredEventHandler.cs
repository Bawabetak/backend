namespace bawabetak_backend.Events.Handlers
{
    public class UserRegisteredEventHandler
    : INotificationHandler<UserRegisteredEvent>
    {
        private readonly IBackgroundJobClient _backgroundJob;

        public UserRegisteredEventHandler(IBackgroundJobClient backgroundJob)
        {
            _backgroundJob = backgroundJob;
        }

        public Task Handle(
            UserRegisteredEvent notification,
            CancellationToken cancellationToken)
        {
            _backgroundJob.Enqueue<IRegisterVerificationJob>(job =>
                job.SendAsync(notification.Email));

            return Task.CompletedTask;
        }
    }
}