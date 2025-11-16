using RSession.Shared.Contracts;

namespace RSession.Shared.Delegates;

public delegate void OnDatabaseConfiguredDelegate(
    ISessionDatabaseService databaseService,
    string type
);
