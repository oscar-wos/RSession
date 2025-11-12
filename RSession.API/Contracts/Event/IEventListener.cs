namespace RSession.API.Contracts.Event;

public interface IEventListener
{
    void Subscribe();
    void Unsubscribe();
}
