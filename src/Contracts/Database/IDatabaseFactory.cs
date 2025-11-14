namespace RSession.Contracts.Database;

public interface IDatabaseFactory
{
    IDatabaseService Database { get; }
}
