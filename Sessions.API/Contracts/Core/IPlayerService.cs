using Sessions.API.Structs;
using SwiftlyS2.Shared.Players;

namespace Sessions.API.Contracts.Core;

public interface IPlayerService
{
    void HandlePlayerAuthorize(IPlayer player);
    void HandlePlayerDisconnected(IPlayer player);
    void HandlePlayerMessage(IPlayer player, short teamNum, bool teamChat, string message);
    SessionsPlayer? GetPlayer(IPlayer player);
}
