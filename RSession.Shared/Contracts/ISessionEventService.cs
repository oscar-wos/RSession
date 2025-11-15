using RSession.Shared.Delegates;

namespace RSession.Shared.Contracts;

public interface ISessionEventService
{
    event OnPlayerRegisteredDelegate OnPlayerRegistered;
    event OnServerRegisteredDelegate OnServerRegistered;
}
