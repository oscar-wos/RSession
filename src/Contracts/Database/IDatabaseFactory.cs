namespace RSession.Contracts.Database;

internal interface IDatabaseFactory
{
    IDatabaseService GetDatabaseService();
    void InvokeDatabaseConfigured();
}
