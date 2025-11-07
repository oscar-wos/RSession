using SwiftlyS2.Shared.Events;

namespace Sessions.API.Contracts.Hook;

public interface IPlayerAuthorizeService
{
    void OnClientSteamAuthorize(IOnClientSteamAuthorizeEvent @event);
}
