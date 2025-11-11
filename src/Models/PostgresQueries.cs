using RSession.API.Contracts.Database;
using RSession.API.Models.Database;

namespace RSession.Models;

public class PostgresQueries : LoadQueries, IDatabaseQueries
{
    protected override string CreateServers =>
        """
            CREATE TABLE IF NOT EXISTS servers (
                id SMALLSERIAL PRIMARY KEY,
                ip INET NOT NULL,
                port SMALLINT NOT NULL
            )
            """;

    protected override string CreateMaps =>
        """
            CREATE TABLE IF NOT EXISTS maps (
                id SMALLSERIAL PRIMARY KEY,
                name VARCHAR(128) NOT NULL
            )
            """;

    protected override string CreatePlayers =>
        """
            CREATE TABLE IF NOT EXISTS players (
                id SERIAL,
                steam_id BIGINT NOT NULL PRIMARY KEY,
                first_seen TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
                last_seen TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
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

    protected override string CreateRotations =>
        """
            CREATE TABLE IF NOT EXISTS rotations (
                id BIGSERIAL PRIMARY KEY,
                server_id SMALLINT NOT NULL,
                map_id SMALLINT NOT NULL,
                timestamp TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
            )
            """;

    protected override string CreateAliases =>
        """
            CREATE TABLE IF NOT EXISTS aliases (
                id BIGSERIAL PRIMARY KEY,
                player_id INT NOT NULL,
                timestamp TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
                name VARCHAR(64)
            )
            """;

    protected override string CreateMessages =>
        """
            CREATE TABLE IF NOT EXISTS messages (
                id BIGSERIAL PRIMARY KEY,
                player_id INT NOT NULL,
                session_id BIGINT NOT NULL,    
                timestamp TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
                team_num SMALLINT NOT NULL,
                team_chat BOOLEAN NOT NULL,
                message VARCHAR(512)
            )
            """;

    public string SelectServer =>
        "SELECT id FROM servers WHERE ip = CAST(@ip as INET) AND port = @port";

    public string SelectMap => "SELECT id FROM maps WHERE name = @name";

    public string SelectPlayer => "SELECT id FROM players WHERE steam_id = @steamId";

    public string InsertServer =>
        "INSERT INTO servers (ip, port) VALUES (CAST(@ip as INET), @port) RETURNING id";

    public string InsertMap => "INSERT INTO maps (name) VALUES (@name) RETURNING id";

    public string InsertPlayer => "INSERT INTO players (steam_id) VALUES (@steamId) RETURNING id";

    public string InsertSession =>
        "INSERT INTO sessions (player_id, server_id, ip) VALUES (@playerId, @serverId, CAST(@ip as INET)) RETURNING id";

    public string InsertRotation =>
        "INSERT INTO rotations (server_id, map_id) VALUES (@serverId, @mapId)";

    public string UpdateSeen => "UPDATE players SET last_seen = NOW() WHERE id = ANY(@playerIds)";

    public string UpdateSession =>
        "UPDATE sessions SET end_time = NOW() WHERE id = ANY(@sessionIds)";

    public string SelectAlias =>
        "SELECT id, name FROM aliases WHERE player_id = @playerId ORDER BY id DESC LIMIT 1";

    public string InsertAlias => "INSERT INTO aliases (player_id, name) VALUES (@playerId, @name)";

    public string InsertMessage =>
        "INSERT INTO messages (player_id, session_id, team_num, team_chat, message) VALUES (@playerId, @sessionId, @teamNum, @teamChat, @message)";
}
