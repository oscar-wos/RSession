using SwiftlyS2.Shared.Events;

namespace Sessions.API.Contracts.Hook;

public interface IOnClientDisconnectedService
{
    void OnClientDisconnected(IOnClientDisconnectedEvent @event);
}
