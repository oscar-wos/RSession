namespace RSession.API.Contracts.Database;

public interface IDatabaseQueries
{
    string SelectPlayer { get; }
    string InsertPlayer { get; }

    string SelectServer { get; }
    string InsertServer { get; }

    string InsertSession { get; }

    string UpdateSeen { get; }
    string UpdateSession { get; }
}
