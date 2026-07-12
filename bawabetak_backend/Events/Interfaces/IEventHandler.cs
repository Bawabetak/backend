namespace bawabetak_backend.Events.Interfaces
{
    public interface IEventHandler<in TEvent>
    {
        Task HandleAsync(TEvent @event);
    }
}
