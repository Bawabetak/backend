
namespace bawabetak_backend.Events
{
    public record UserRegisteredEvent(string Email) : INotification;
}
