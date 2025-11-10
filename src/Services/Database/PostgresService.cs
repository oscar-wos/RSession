using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using Sessions.API.Contracts.Database;
using Sessions.API.Contracts.Log;
using Sessions.API.Models.Config;
using Sessions.API.Structs;
using Sessions.Models;

namespace Sessions.Services.Database;

public sealed class PostgresService : IPostgresService, IDatabaseService, IDisposable
{
    private readonly IOptionsMonitor<DatabaseConfig> _config;

    private readonly ILogService _logService;
    private readonly ILogger<PostgresService> _logger;

    private readonly NpgsqlDataSource _dataSource;
    private readonly PostgresQueries _queries;

    public PostgresService(
        IOptionsMonitor<DatabaseConfig> config,
        ILogService logService,
        ILogger<PostgresService> logger
    )
    {
        _config = config;

        _logService = logService;
        _logger = logger;

        string connectionString = BuildConnectionString(_config.CurrentValue.Connection);
        _dataSource = NpgsqlDataSource.Create(connectionString);

        _queries = new PostgresQueries();
    }

    private string BuildConnectionString(ConnectionConfig config)
    {
        NpgsqlConnectionStringBuilder builder = new()
        {
            Host = config.Host,
            Port = config.Port,
            Username = config.Username,
            Password = config.Password,
            Database = config.Database,
            Pooling = true,
        };

        string connectionString = builder.ConnectionString;
        _logService.LogDebug(connectionString, logger: _logger);

        return builder.ConnectionString;
    }

    public async Task InitAsync()
    {
        await using NpgsqlConnection connection = await _dataSource
            .OpenConnectionAsync()
            .ConfigureAwait(false);

        await using NpgsqlTransaction transaction = await connection
            .BeginTransactionAsync()
            .ConfigureAwait(false);

        foreach (string query in _queries.GetLoadQueries())
        {
            await using NpgsqlCommand command = new(query, connection, transaction);
            _ = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        await transaction.CommitAsync().ConfigureAwait(false);
    }

    public async Task<SessionsAlias?> GetAliasAsync(int playerId)
    {
        await using NpgsqlConnection connection = await _dataSource
            .OpenConnectionAsync()
            .ConfigureAwait(false);

        await using NpgsqlCommand command = new(_queries.SelectAlias, connection);
        _ = command.Parameters.AddWithValue("@playerId", playerId);

        await using NpgsqlDataReader reader = await command
            .ExecuteReaderAsync()
            .ConfigureAwait(false);

        if (await reader.ReadAsync().ConfigureAwait(false))
        {
            return new SessionsAlias() { Id = reader.GetInt64(0), Name = reader.GetString(1) };
        }

        return null;
    }

    public async Task<SessionsMap> GetMapAsync(string mapName)
    {
        await using NpgsqlConnection connection = await _dataSource
            .OpenConnectionAsync()
            .ConfigureAwait(false);

        await using (NpgsqlCommand command = new(_queries.SelectMap, connection))
        {
            _ = command.Parameters.AddWithValue("@name", mapName);

            if (await command.ExecuteScalarAsync().ConfigureAwait(false) is short result)
            {
                return new SessionsMap() { Id = result };
            }
        }

        await using (NpgsqlCommand command = new(_queries.InsertMap, connection))
        {
            _ = command.Parameters.AddWithValue("@name", mapName);

            if (await command.ExecuteScalarAsync().ConfigureAwait(false) is not short result)
            {
                throw new Exception("Failed to insert map");
            }

            return new SessionsMap() { Id = result };
        }
    }

    public async Task<SessionsPlayer> GetPlayerAsync(ulong steamId)
    {
        await using NpgsqlConnection connection = await _dataSource
            .OpenConnectionAsync()
            .ConfigureAwait(false);

        await using (NpgsqlCommand command = new(_queries.SelectPlayer, connection))
        {
            _ = command.Parameters.AddWithValue("@steamId", (long)steamId);

            if (await command.ExecuteScalarAsync().ConfigureAwait(false) is int result)
            {
                return new SessionsPlayer() { Id = result };
            }
        }

        await using (NpgsqlCommand command = new(_queries.InsertPlayer, connection))
        {
            _ = command.Parameters.AddWithValue("@steamId", (long)steamId);

            if (await command.ExecuteScalarAsync().ConfigureAwait(false) is not int result)
            {
                throw new Exception("Failed to insert player");
            }

            return new SessionsPlayer() { Id = result };
        }
    }

    public async Task<SessionsServer> GetServerAsync(string ip, ushort port)
    {
        await using NpgsqlConnection connection = await _dataSource
            .OpenConnectionAsync()
            .ConfigureAwait(false);

        await using (NpgsqlCommand command = new(_queries.SelectServer, connection))
        {
            _ = command.Parameters.AddWithValue("@ip", ip);
            _ = command.Parameters.AddWithValue("@port", (short)port);

            if (await command.ExecuteScalarAsync().ConfigureAwait(false) is short result)
            {
                return new SessionsServer()
                {
                    Id = result,
                    Ip = ip,
                    Port = port,
                };
            }
        }

        await using (NpgsqlCommand command = new(_queries.InsertServer, connection))
        {
            _ = command.Parameters.AddWithValue("@ip", ip);
            _ = command.Parameters.AddWithValue("@port", (short)port);

            if (await command.ExecuteScalarAsync().ConfigureAwait(false) is not short result)
            {
                throw new Exception("Failed to insert server");
            }

            return new SessionsServer()
            {
                Id = result,
                Ip = ip,
                Port = port,
            };
        }
    }

    public async Task<SessionsSession> GetSessionAsync(int playerId, int serverId, string ip)
    {
        await using NpgsqlConnection connection = await _dataSource
            .OpenConnectionAsync()
            .ConfigureAwait(false);

        await using NpgsqlCommand command = new(_queries.InsertSession, connection);

        _ = command.Parameters.AddWithValue("@playerId", playerId);
        _ = command.Parameters.AddWithValue("@serverId", serverId);
        _ = command.Parameters.AddWithValue("@ip", ip);

        if (await command.ExecuteScalarAsync().ConfigureAwait(false) is not long result)
        {
            throw new Exception("Failed to insert session");
        }

        return new SessionsSession() { Id = result };
    }

    public async Task InsertAliasAsync(int playerId, string name)
    {
        await using NpgsqlConnection connection = await _dataSource
            .OpenConnectionAsync()
            .ConfigureAwait(false);
    }

    public async Task InsertMessageAsync(
        int playerId,
        long sessionId,
        short teamNum,
        bool teamChat,
        string message
    )
    {
        await using NpgsqlConnection connection = await _dataSource
            .OpenConnectionAsync()
            .ConfigureAwait(false);

        await using NpgsqlCommand command = new(_queries.InsertMessage, connection);

        _ = command.Parameters.AddWithValue("@playerId", playerId);
        _ = command.Parameters.AddWithValue("@sessionId", sessionId);
        _ = command.Parameters.AddWithValue("@teamNum", teamNum);
        _ = command.Parameters.AddWithValue("@teamChat", teamChat);
        _ = command.Parameters.AddWithValue("@message", message);

        _ = await command.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public async Task UpdateSessionsAsync(IEnumerable<int> playerIds, IEnumerable<long> sessionIds)
    {
        await using NpgsqlConnection connection = await _dataSource
            .OpenConnectionAsync()
            .ConfigureAwait(false);

        await using NpgsqlCommand updatePlayerCommand = new(_queries.UpdateSeen, connection);
        _ = updatePlayerCommand.Parameters.AddWithValue("@playerIds", playerIds.ToArray());

        _ = await updatePlayerCommand.ExecuteNonQueryAsync().ConfigureAwait(false);

        await using NpgsqlCommand updateSessionCommand = new(_queries.UpdateSession, connection);
        _ = updateSessionCommand.Parameters.AddWithValue("@sessionIds", sessionIds.ToArray());

        _ = await updateSessionCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
    }

    public void Dispose() =>
        _logService.LogInformation("Disposing PostgresService", logger: _logger);
}
