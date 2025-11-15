using RSession.Shared.Structs;
using SwiftlyS2.Shared.Players;

namespace RSession.Shared.Delegates;

public delegate void OnPlayerRegisteredDelegate(IPlayer player, in SessionPlayer sessionPlayer);
