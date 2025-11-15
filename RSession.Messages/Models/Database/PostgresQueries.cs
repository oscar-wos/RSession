namespace RSession.Messages.Models.Database;

internal sealed class PostgresQueries : LoadQueries
{
    protected override string CreateMessages =>
        """
            CREATE TABLE IF NOT EXISTS messages (
                message_id BIGSERIAL PRIMARY KEY,
                player_id INTEGER NOT NULL,
                session_id BIGINT NOT NULL,
                message_text TEXT NOT NULL,
                created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
            )
            """;

    public string InsertMessage =>
        """
            INSERT INTO messages (player_id, session_id, message_text)
            VALUES (@playerId, @sessionId, @messageText)
            RETURNING message_id
            """;
}
