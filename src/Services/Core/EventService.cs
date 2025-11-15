using RSession.Contracts.Core;
using RSession.Shared.Delegates;
using RSession.Shared.Structs;
using SwiftlyS2.Shared.Players;

namespace RSession.Services.Core;

internal sealed class EventService : IEventService
{
    public event OnPlayerRegisteredDelegate? OnPlayerRegistered;
    public event OnServerRegisteredDelegate? OnServerRegistered;

    public void InvokePlayerRegistered(IPlayer player, in SessionPlayer sessionPlayer) =>
        OnPlayerRegistered?.Invoke(player, in sessionPlayer);

    public void InvokeServerRegistered(short serverId) => OnServerRegistered?.Invoke(serverId);
}
