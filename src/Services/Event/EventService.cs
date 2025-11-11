using RSession.API.Contracts.Event;
using RSession.API.Delegates;
using RSession.API.Structs;
using SwiftlyS2.Shared.Players;

namespace RSession.Services.Event;

public class EventService : IEventService
{
    public event PlayerAuthorizedDelegate? PlayerAuthorized;

    public void InvokePlayerAuthorized(IPlayer player, in SessionsPlayer sessionsPlayer) =>
        PlayerAuthorized?.Invoke(player, sessionsPlayer);
}
