using RSession.Contracts.Database;

namespace RSession.Models.Database;

public sealed class SqlQueries : LoadQueries, IDatabaseQueries
{
    protected override string CreatePlayers =>
        """
            CREATE TABLE IF NOT EXISTS players (
                id INT AUTO_INCREMENT PRIMARY KEY,
                steam_id BIGINT NOT NULL,
                first_seen DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                last_seen DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            )
            """;

    protected override string CreateServers =>
        """
            CREATE TABLE IF NOT EXISTS servers (
                id SMALLINT AUTO_INCREMENT PRIMARY KEY,
                ip VARCHAR(15) NOT NULL,
                port SMALLINT UNSIGNED NOT NULL
            )
            """;

    protected override string CreateSessions =>
        """
            CREATE TABLE IF NOT EXISTS sessions (
                id BIGINT AUTO_INCREMENT PRIMARY KEY,
                player_id INT NOT NULL,
                server_id SMALLINT NOT NULL,
                ip VARCHAR(15) NOT NULL,
                start_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
                end_time DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            )
            """;

    public string SelectPlayer => "SELECT id FROM players WHERE steam_id = @steamId";

    public string InsertPlayer =>
        "INSERT INTO players (steam_id) VALUES (@steamId); SELECT LAST_INSERT_ID()";

    public string SelectServer => "SELECT id FROM servers WHERE ip = @ip AND port = @port";

    public string InsertServer =>
        "INSERT INTO servers (ip, port) VALUES (@ip, @port); SELECT LAST_INSERT_ID()";

    public string InsertSession =>
        "INSERT INTO sessions (player_id, server_id, ip) VALUES (@playerId, @serverId, @ip); SELECT LAST_INSERT_ID()";

    public string UpdateSeen => "UPDATE players SET last_seen = NOW() WHERE id IN (@playerIds)";

    public string UpdateSession => "UPDATE sessions SET end_time = NOW() WHERE id IN (@sessionIds)";
}
