using RSession.API.Delegates;
using RSession.API.Structs;
using SwiftlyS2.Shared.Players;

namespace RSession.API.Contracts.Event;

public interface IEventService
{
    event PlayerAuthorizedDelegate PlayerAuthorized;

    void InvokePlayerAuthorized(IPlayer player, in SessionsPlayer sessionsPlayer);
}
