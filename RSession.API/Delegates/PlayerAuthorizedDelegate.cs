using RSession.API.Structs;
using SwiftlyS2.Shared.Players;

namespace RSession.API.Delegates;

public delegate void PlayerAuthorizedDelegate(IPlayer player, in SessionsPlayer sessionsPlayer);
