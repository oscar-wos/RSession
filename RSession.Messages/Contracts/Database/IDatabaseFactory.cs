using RSession.Shared.Contracts;

namespace RSession.Messages.Contracts.Database;

internal interface IDatabaseFactory
{
    IDatabaseService? GetDatabaseService();
    void RegisterDatabaseService(ISessionDatabaseService sessionDatabaseService, string type);
}
