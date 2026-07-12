public class UserRegisteredEventHandler
    : IEventHandler<UserRegisteredEvent>
{
    private readonly IBackgroundJobClient _backgroundJob;

    public UserRegisteredEventHandler(IBackgroundJobClient backgroundJob)
    {
        _backgroundJob = backgroundJob;
    }

    public Task HandleAsync(UserRegisteredEvent @event)
    {
        _backgroundJob.Enqueue<IRegisterVerificationJob>(job =>
            job.SendAsync(@event.Email));

        return Task.CompletedTask;
    }
}