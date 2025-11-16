using RSession.Shared.Contracts;
using SwiftlyS2.Shared.Players;

namespace RSession.Messages.Contracts.Core;

internal interface IPlayerService
{
    void Initialize(ISessionPlayerService playerService, ISessionServerService serverService);
    void HandlePlayerMessage(IPlayer player, short teamNum, bool teamChat, string message);
}
