using RSession.Shared.Contracts;
using SwiftlyS2.Shared.Players;

namespace RSession.Contracts.Core;

internal interface IPlayerService : ISessionPlayerService
{
    void HandlePlayerAuthorize(IPlayer player, short serverId);
    void HandlePlayerDisconnected(IPlayer player);
}
