using RSession.Shared.Contracts;
using SwiftlyS2.Shared.Players;

namespace RSession.Contracts.Core;

internal interface IRSessionEventServiceInternal : IRSessionEventService
{
    void InvokePlayerRegistered(IPlayer player, int playerId, long sessionId);
    void InvokeServerRegistered(short serverId);
}
