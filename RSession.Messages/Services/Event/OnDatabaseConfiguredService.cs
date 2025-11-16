using Microsoft.Extensions.Logging;
using RSession.Messages.Contracts.Database;
using RSession.Messages.Contracts.Log;
using RSession.Shared.Contracts;

namespace RSession.Messages.Services.Event;

internal sealed class OnDatabaseConfiguredService(
    ILogService logService,
    ILogger<OnDatabaseConfiguredService> logger,
    IDatabaseFactory databaseFactory
)
{
    private readonly ILogService _logService = logService;
    private readonly ILogger<OnDatabaseConfiguredService> _logger = logger;

    private readonly IDatabaseFactory _databaseFactory = databaseFactory;

    public void Initialize(ISessionEventService eventService)
    {
        eventService.OnDatabaseConfigured += OnDatabaseConfigured;
        _logService.LogInformation("OnDatabaseConfigured subscribed", logger: _logger);
    }

    private void OnDatabaseConfigured(ISessionDatabaseService databaseService, string type) =>
        _databaseFactory.RegisterDatabaseService(databaseService, type);
}
