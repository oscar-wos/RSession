using SwiftlyS2.Shared.Players;

namespace RSession.API.Delegates;

public delegate void PlayerAuthorizedDelegate(IPlayer player, int playerId, long sessionId);
