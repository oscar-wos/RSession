/*using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RSession.Contracts.Database;
using RSession.Messages.Contracts.Log;

namespace RSession.Messages.Services.Database;

internal sealed class DatabaseFactory
{
    private readonly ILogService _logService;
    private readonly ILogger<DatabaseFactory> _logger;
    private readonly IDatabaseService _database;

    private readonly IServiceProvider _serviceProvider;

    public object DatabaseService { get; private set; }

    public DatabaseFactory(
        ILogService logService,
        ILogger<DatabaseFactory> logger,
        IRSessionDatabaseService database,
        IServiceProvider serviceProvider
    )
    {
        _logService = logService;
        _logger = logger;
        _database = database;
        _serviceProvider = serviceProvider;

        string type = _database.Type;

        DatabaseService = type.ToLowerInvariant() switch
        {
            "postgres" => _serviceProvider.GetRequiredService<PostgresService>(),
            "mysql" => _serviceProvider.GetRequiredService<SqlService>(),
            _ => throw _logService.LogCritical(
                $"Database is not supported - '{type}' | Supported types: postgres, mysql",
                logger: _logger
            ),
        };

        _logService.LogInformation($"DatabaseFactory initialized - '{type}'", logger: _logger);
    }
}
*/
