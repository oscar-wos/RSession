namespace RSession.Messages.Models.Database;

internal sealed class SqlQueries : LoadQueries
{
    protected override string CreateMessages =>
        """
            CREATE TABLE IF NOT EXISTS messages (
                message_id BIGINT AUTO_INCREMENT PRIMARY KEY,
                player_id INT NOT NULL,
                session_id BIGINT NOT NULL,
                message_text TEXT NOT NULL,
                created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
            )
            """;

    public string InsertMessage =>
        """
            INSERT INTO messages (player_id, session_id, message_text)
            VALUES (@playerId, @sessionId, @messageText);
            SELECT LAST_INSERT_ID()
            """;
}
