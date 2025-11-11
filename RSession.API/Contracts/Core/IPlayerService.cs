using RSession.API.Structs;
using SwiftlyS2.Shared.Players;

namespace RSession.API.Contracts.Core;

public interface IPlayerService
{
    void HandlePlayerAuthorize(IPlayer player);
    void HandlePlayerDisconnected(IPlayer player);
    void HandlePlayerMessage(IPlayer player, short teamNum, bool teamChat, string message);
    SessionsPlayer? GetPlayer(IPlayer player);
}
