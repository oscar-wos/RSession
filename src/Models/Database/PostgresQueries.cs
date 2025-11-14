using RSession.Contracts.Database;

namespace RSession.Models.Database;

public class PostgresQueries : LoadQueries, IDatabaseQueries
{
    protected override string CreatePlayers =>
        """
            CREATE TABLE IF NOT EXISTS players (
                id SERIAL,
                steam_id BIGINT NOT NULL PRIMARY KEY,
                first_seen TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
                last_seen TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
            )
            """;

    protected override string CreateServers =>
        """
            CREATE TABLE IF NOT EXISTS servers (
                id SMALLSERIAL PRIMARY KEY,
                ip INET NOT NULL,
                port SMALLINT NOT NULL
            )
            """;

    protected override string CreateSessions =>
        """
            CREATE TABLE IF NOT EXISTS sessions (
                id BIGSERIAL PRIMARY KEY,
                player_id INT NOT NULL,
                server_id SMALLINT NOT NULL,
                ip INET NOT NULL,
                start_time TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
                end_time TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
            )
            """;

    public string SelectPlayer => "SELECT id FROM players WHERE steam_id = @steamId";

    public string InsertPlayer => "INSERT INTO players (steam_id) VALUES (@steamId) RETURNING id";

    public string SelectServer =>
        "SELECT id FROM servers WHERE ip = CAST(@ip as INET) AND port = @port";

    public string InsertServer =>
        "INSERT INTO servers (ip, port) VALUES (CAST(@ip as INET), @port) RETURNING id";

    public string InsertSession =>
        "INSERT INTO sessions (player_id, server_id, ip) VALUES (@playerId, @serverId, CAST(@ip as INET)) RETURNING id";

    public string UpdateSeen => "UPDATE players SET last_seen = NOW() WHERE id = ANY(@playerIds)";

    public string UpdateSession =>
        "UPDATE sessions SET end_time = NOW() WHERE id = ANY(@sessionIds)";
}
