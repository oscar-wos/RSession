namespace Sessions.API.Contracts.Database;

public interface IDatabaseQueries
{
    string SelectServer { get; }
    string SelectMap { get; }
    string SelectPlayer { get; }
    string SelectAlias { get; }
    string InsertServer { get; }
    string InsertMap { get; }
    string InsertPlayer { get; }
    string InsertSession { get; }
    string InsertAlias { get; }
    string InsertMessage { get; }
    string UpdateSession { get; }
    string UpdateSeen { get; }
    string InsertRotation { get; }
}
