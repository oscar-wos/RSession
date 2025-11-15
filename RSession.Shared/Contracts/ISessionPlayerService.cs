using RSession.Shared.Structs;
using SwiftlyS2.Shared.Players;

namespace RSession.Shared.Contracts;

public interface ISessionPlayerService
{
    SessionPlayer? GetPlayer(IPlayer player);
}
