using SwiftlyS2.Shared.Players;

namespace RSession.Shared.Contracts;

public interface IRSessionPlayer
{
    int? GetPlayer(IPlayer player);
    long? GetSession(IPlayer player);
}
