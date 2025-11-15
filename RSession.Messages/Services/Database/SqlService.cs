/*using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using RSession.Messages.Contracts.Log;
using RSession.Messages.Models.Database;
using RSession.Shared.Contracts;

namespace RSession.Messages.Services.Database;

internal sealed class SqlService
{
    private readonly ILogService _logService;
    private readonly ILogger<SqlService> _logger;
    private readonly IRSessionDatabaseService _database;
    private readonly SqlQueries _queries;

    public SqlService(
        ILogService logService,
        ILogger<SqlService> logger,
        IRSessionDatabaseService database
    )
    {
        _logService = logService;
        _logger = logger;
        _database = database;
        _queries = new SqlQueries();
    }

    public async Task InitializeAsync()
    {
        try
        {
            await using DbConnection connection = await _database
                .GetConnectionAsync()
                .ConfigureAwait(false);

            await using DbTransaction transaction = await connection
                .BeginTransactionAsync()
                .ConfigureAwait(false);

            foreach (string query in _queries.GetLoadQueries())
            {
                await using DbCommand command = connection.CreateCommand();
                command.CommandText = query;
                command.Transaction = transaction;
                _ = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
            }

            await transaction.CommitAsync().ConfigureAwait(false);

            _logService.LogInformation(
                $"Messages tables initialized for {_database.Type}",
                logger: _logger
            );
        }
        catch (Exception ex)
        {
            _logService.LogError(
                "Failed to initialize messages tables",
                exception: ex,
                logger: _logger
            );
        }
    }

    public async Task<long?> InsertMessageAsync(int playerId, long sessionId, string messageText)
    {
        try
        {
            await using DbConnection connection = await _database
                .GetConnectionAsync()
                .ConfigureAwait(false);

            await using DbCommand command = connection.CreateCommand();
            command.CommandText = _queries.InsertMessage;

            DbParameter playerIdParam = command.CreateParameter();
            playerIdParam.ParameterName = "@playerId";
            playerIdParam.Value = playerId;
            playerIdParam.DbType = DbType.Int32;
            command.Parameters.Add(playerIdParam);

            DbParameter sessionIdParam = command.CreateParameter();
            sessionIdParam.ParameterName = "@sessionId";
            sessionIdParam.Value = sessionId;
            sessionIdParam.DbType = DbType.Int64;
            command.Parameters.Add(sessionIdParam);

            DbParameter messageTextParam = command.CreateParameter();
            messageTextParam.ParameterName = "@messageText";
            messageTextParam.Value = messageText;
            messageTextParam.DbType = DbType.String;
            command.Parameters.Add(messageTextParam);

            if (await command.ExecuteScalarAsync().ConfigureAwait(false) is long messageId)
            {
                return messageId;
            }

            return null;
        }
        catch (Exception ex)
        {
            _logService.LogError("Failed to insert message", exception: ex, logger: _logger);
            return null;
        }
    }
}
*/
