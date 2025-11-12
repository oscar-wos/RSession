using Microsoft.Extensions.Logging;
using RSession.API.Contracts.Core;
using RSession.API.Contracts.Event;
using RSession.API.Contracts.Log;
using SwiftlyS2.Shared;

namespace RSession.Services.Event;

public sealed class OnSteamAPIActivatedService(
    ILogService logService,
    ILogger<OnSteamAPIActivatedService> logger,
    ISwiftlyCore core,
    IServerService serverService
) : IEventListener
{
    private readonly ILogService _logService = logService;
    private readonly ILogger<OnSteamAPIActivatedService> _logger = logger;

    private readonly ISwiftlyCore _core = core;
    private readonly IServerService _serverService = serverService;

    public void Subscribe()
    {
        _core.Event.OnSteamAPIActivated += OnSteamAPIActivated;
        _logService.LogInformation("OnSteamAPIActivated subscribed", logger: _logger);
    }

    public void Unsubscribe()
    {
        _core.Event.OnSteamAPIActivated -= OnSteamAPIActivated;
        _logService.LogInformation("OnSteamAPIActivated unsubscribed", logger: _logger);
    }

    private void OnSteamAPIActivated()
    {
        _logService.LogInformation($"SteamAPI activated", logger: _logger);
        _serverService.HandleInit();
    }
}
