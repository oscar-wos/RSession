using SwiftlyS2.Shared.Events;

namespace RSession.API.Contracts.Hook;

public interface IOnClientDisconnectedService
{
    void OnClientDisconnected(IOnClientDisconnectedEvent @event);
}
