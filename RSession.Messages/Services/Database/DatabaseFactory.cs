using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RSession.Messages.Contracts.Database;
using RSession.Messages.Contracts.Log;
using RSession.Shared.Contracts;

namespace RSession.Messages.Services.Database;

internal class DatabaseFactory(
    ILogService logService,
    ILogger<DatabaseFactory> logger,
    IServiceProvider serviceProvider
) : IDatabaseFactory
{
    private readonly ILogService _logService = logService;
    private readonly ILogger<DatabaseFactory> _logger = logger;

    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private IDatabaseService? _databaseService;

    public void RegisterDatabaseService(ISessionDatabaseService databaseService, string type)
    {
        _databaseService = type.ToLowerInvariant() switch
        {
            "postgres" => _serviceProvider.GetRequiredService<PostgresService>(),
            "mysql" => _serviceProvider.GetRequiredService<SqlService>(),
            _ => throw _logService.LogCritical(
                $"Database is not supported - '{type}' | Supported types: postgres, mysql",
                logger: _logger
            ),
        };

        _databaseService.Initialize(databaseService);
        _ = Task.Run(async () => await _databaseService.InitAsync());

        _logService.LogInformation($"DatabaseFactory initialized - '{type}'", logger: _logger);
    }
}
