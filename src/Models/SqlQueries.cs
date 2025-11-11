using RSession.API.Contracts.Database;
using RSession.API.Models.Database;

namespace RSession.Models;

public sealed class SqlQueries : LoadQueries, IDatabaseQueries
{
    protected override string CreateServers =>
        """
            CREATE TABLE IF NOT EXISTS servers (
                id SMALLINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
                ip VARCHAR(15) NOT NULL,
                port SMALLINT UNSIGNED NOT NULL
            )
            """;

    protected override string CreateMaps =>
        """
            CREATE TABLE IF NOT EXISTS maps (
                id SMALLINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
                name VARCHAR(128) NOT NULL
            )
            """;

    protected override string CreatePlayers =>
        """
            CREATE TABLE IF NOT EXISTS players (
                id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
                steam_id BIGINT UNSIGNED NOT NULL,
                first_seen DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                last_seen DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            )
            """;

    protected override string CreateSessions =>
        """
            CREATE TABLE IF NOT EXISTS sessions (
                id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
                player_id INT UNSIGNED NOT NULL,
                server_id SMALLINT UNSIGNED NOT NULL,
                ip VARCHAR(15) NOT NULL,
                start_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                end_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            )
            """;

    protected override string CreateRotations =>
        """
            CREATE TABLE IF NOT EXISTS rotations (
                id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
                server_id SMALLINT UNSIGNED NOT NULL,
                map_id SMALLINT UNSIGNED NOT NULL,
                timestamp DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            )
            """;

    protected override string CreateAliases =>
        """
            CREATE TABLE IF NOT EXISTS aliases (
                id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
                player_id INT UNSIGNED NOT NULL,
                timestamp DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                name VARCHAR(64) COLLATE utf8mb4_unicode_520_ci
            )
            """;

    protected override string CreateMessages =>
        """
            CREATE TABLE IF NOT EXISTS messages (
                id BIGINT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
                player_id INT UNSIGNED NOT NULL,
                session_id BIGINT UNSIGNED NOT NULL,
                timestamp DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                team_num SMALLINT UNSIGNED NOT NULL,
                team_chat BOOLEAN NOT NULL,
                message VARCHAR(512) COLLATE utf8mb4_unicode_520_ci
            )
            """;

    public string SelectServer => "SELECT id FROM servers WHERE ip = @ip AND port = @port";

    public string SelectMap => "SELECT id FROM maps WHERE name = @name";

    public string SelectPlayer => "SELECT id FROM players WHERE steam_id = @steamId";

    public string InsertServer =>
        "INSERT INTO servers (ip, port) VALUES (@ip, @port); SELECT LAST_INSERT_ID()";

    public string InsertMap => "INSERT INTO maps (name) VALUES (@name); SELECT LAST_INSERT_ID()";

    public string InsertPlayer =>
        "INSERT INTO players (steam_id) VALUES (@steamId); SELECT LAST_INSERT_ID()";

    public string InsertSession =>
        "INSERT INTO sessions (player_id, server_id, ip) VALUES (@playerId, @serverId, @ip); SELECT LAST_INSERT_ID()";

    public string InsertRotation =>
        "INSERT INTO rotations (server_id, map_id) VALUES (@serverId, @mapId)";

    public string UpdateSeen => "UPDATE players SET last_seen = NOW() WHERE id IN (@playerIds)";

    public string UpdateSession => "UPDATE sessions SET end_time = NOW() WHERE id IN (@sessionIds)";

    public string SelectAlias =>
        "SELECT id, name FROM aliases WHERE player_id = @playerId ORDER BY id DESC LIMIT 1";

    public string InsertAlias => "INSERT INTO aliases (player_id, name) VALUES (@playerId, @name)";

    public string InsertMessage =>
        "INSERT INTO messages (player_id, session_id, team_num, team_chat, message) VALUES (@playerId, @sessionId, @teamNum, @teamChat, @message)";
}
