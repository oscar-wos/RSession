using SwiftlyS2.Shared.Events;

namespace Sessions.API.Contracts.Hook;

public interface IOnMapLoadService
{
    void OnMapLoad(IOnMapLoadEvent @event);
}
