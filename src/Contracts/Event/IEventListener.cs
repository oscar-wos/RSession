namespace RSession.Contracts.Event;

public interface IEventListener
{
    void Subscribe();
    void Unsubscribe();
}
