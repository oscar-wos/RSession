using SwiftlyS2.Shared.Players;

namespace RSession.Shared.Contracts.Core;

public interface IRSessionPlayer
{
    int? GetPlayer(IPlayer player);
    long? GetSession(IPlayer player);
    void HandlePlayerAuthorize(IPlayer player, short serverId);
    void HandlePlayerDisconnected(IPlayer player);
}
