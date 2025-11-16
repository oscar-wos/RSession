using RSession.Shared.Delegates;

namespace RSession.Shared.Contracts;

public interface ISessionEventService
{
    event OnDatabaseConfiguredDelegate OnDatabaseConfigured;
    event OnPlayerRegisteredDelegate OnPlayerRegistered;
    event OnServerRegisteredDelegate OnServerRegistered;
}
