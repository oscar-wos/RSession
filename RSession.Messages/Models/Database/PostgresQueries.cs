namespace RSession.Messages.Models.Database;

internal sealed class PostgresQueries : LoadQueries
{
    protected override string CreateMessages =>
        """
            CREATE TABLE IF NOT EXISTS messages (
                id BIGSERIAL PRIMARY KEY,
                player_id INT NOT NULL,
                server_id SMALLINT NOT NULL,
                session_id BIGINT NOT NULL,    
                timestamp TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
                team_num SMALLINT NOT NULL,
                team_chat BOOLEAN NOT NULL,
                message VARCHAR(512)
            )
            """;
}
