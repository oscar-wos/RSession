using RSession.Shared.Delegates;

namespace RSession.Shared.Contracts;

public interface IRSessionEvent
{
    event OnPlayerRegisteredDelegate OnPlayerRegistered;
    event OnServerRegisteredDelegate OnServerRegistered;
}
