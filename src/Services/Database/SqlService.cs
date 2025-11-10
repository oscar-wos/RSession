using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using Sessions.API.Contracts.Database;
using Sessions.API.Contracts.Log;
using Sessions.API.Models.Config;
using Sessions.API.Structs;

namespace Sessions.Services.Database;

public sealed class SqlService : ISqlService, IDatabaseService, IDisposable
{
    private readonly IOptionsMonitor<DatabaseConfig> _config;

    private readonly ILogService _logService;
    private readonly ILogger<SqlService> _logger;

    private readonly MySqlDataSource _dataSource;

    public SqlService(
        IOptionsMonitor<DatabaseConfig> config,
        ILogService logService,
        ILogger<SqlService> logger
    )
    {
        _config = config;

        _logService = logService;
        _logger = logger;

        string connectionString = BuildConnectionString(_config.CurrentValue.Connection);
        _dataSource = new MySqlDataSourceBuilder(connectionString).Build();
    }

    private string BuildConnectionString(ConnectionConfig config)
    {
        MySqlConnectionStringBuilder builder = new()
        {
            Server = config.Host,
            Port = (uint)config.Port,
            UserID = config.Username,
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
        try { }
        catch (MySqlException ex)
        {
            _logService.LogError("SqlService.InitAsync()", exception: ex, logger: _logger);
        }
    }

    public Task<SessionsAlias?> GetAliasAsync(int playerId) => throw new NotImplementedException();

    public Task<SessionsMap> GetMapAsync(string mapName) => throw new NotImplementedException();

    public Task<SessionsPlayer> GetPlayerAsync(ulong steamId) =>
        throw new NotImplementedException();

    public Task<SessionsServer> GetServerAsync(string serverIp, ushort serverPort) =>
        throw new NotImplementedException();

    public Task<SessionsSession> GetSessionAsync(int playerId, int serverId, string ip) =>
        throw new NotImplementedException();

    public Task InsertAliasAsync(int playerId, string name) => throw new NotImplementedException();

    public Task InsertMessageAsync(
        int playerId,
        long sessionId,
        short teamNum,
        bool teamChat,
        string message
    ) => throw new NotImplementedException();

    public Task UpdateSeenAsync(int playerId) => throw new NotImplementedException();

    public Task UpdateSessionsAsync(IEnumerable<int> playerIds, IEnumerable<long> sessionIds) =>
        throw new NotImplementedException();

    public void Dispose()
    {
        _logService.LogInformation("Disposing SqlService", logger: _logger);
    }
}
