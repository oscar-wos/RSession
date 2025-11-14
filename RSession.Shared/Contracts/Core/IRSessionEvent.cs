using RSession.Shared.Delegates;
using SwiftlyS2.Shared.Players;

namespace RSession.Shared.Contracts.Core;

public interface IRSessionEvent
{
    event OnPlayerRegisteredDelegate OnPlayerRegistered;
    event OnServerRegisteredDelegate OnServerRegistered;
    void InvokePlayerRegistered(IPlayer player, int playerId, long sessionId);
    void InvokeServerRegistered(short serverId);
}
