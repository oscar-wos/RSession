using RSession.API.Delegates;
using SwiftlyS2.Shared.Players;

namespace RSession.API.Contracts.Core;

public interface IEventService
{
    event PlayerAuthorizedDelegate PlayerAuthorized;

    void InvokePlayerAuthorized(IPlayer player, int playerId, long sessionId);
}
