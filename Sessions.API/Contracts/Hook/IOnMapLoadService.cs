using SwiftlyS2.Shared.Events;

namespace RSession.API.Contracts.Hook;

public interface IOnMapLoadService
{
    void OnMapLoad(IOnMapLoadEvent @event);
}
