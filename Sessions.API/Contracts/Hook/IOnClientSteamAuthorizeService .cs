using SwiftlyS2.Shared.Events;

namespace Sessions.API.Contracts.Hook;

public interface IOnClientSteamAuthorizeService
{
    void OnClientSteamAuthorize(IOnClientSteamAuthorizeEvent @event);
}
