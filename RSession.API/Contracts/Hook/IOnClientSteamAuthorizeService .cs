using SwiftlyS2.Shared.Events;

namespace RSession.API.Contracts.Hook;

public interface IOnClientSteamAuthorizeService
{
    void OnClientSteamAuthorize(IOnClientSteamAuthorizeEvent @event);
}
