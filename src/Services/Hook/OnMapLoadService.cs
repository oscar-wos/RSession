using Microsoft.Extensions.Logging;
using Sessions.API.Contracts.Core;
using Sessions.API.Contracts.Hook;
using Sessions.API.Contracts.Log;
using SwiftlyS2.Shared.Events;

namespace Sessions.Services.Hook;

public class OnMapLoadService(
    ILogService logService,
    ILogger<OnMapLoadService> logger,
    Lazy<IServerService> serverService
) : IOnMapLoadService
{
    private readonly ILogService _logService = logService;
    private readonly ILogger<OnMapLoadService> _logger = logger;

    private readonly Lazy<IServerService> _serverService = serverService;

    public void OnMapLoad(IOnMapLoadEvent @event)
    {
        _logService.LogInformation($"Map loaded: {@event.MapName}", logger: _logger);
        _serverService.Value.HandleMapLoad(@event.MapName);
    }
}
