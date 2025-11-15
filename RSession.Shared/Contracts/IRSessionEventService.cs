using RSession.Shared.Delegates;

namespace RSession.Shared.Contracts;

public interface IRSessionEventService
{
    event OnPlayerRegisteredDelegate OnPlayerRegistered;
    event OnServerRegisteredDelegate OnServerRegistered;
}
