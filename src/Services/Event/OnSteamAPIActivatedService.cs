using Microsoft.Extensions.Logging;
using RSession.Contracts.Core;
using RSession.Contracts.Event;
using RSession.Contracts.Log;
using SwiftlyS2.Shared;

namespace RSession.Services.Event;

internal sealed class OnSteamAPIActivatedService(
    ISwiftlyCore core,
    ILogService logService,
    ILogger<OnSteamAPIActivatedService> logger,
    IRSessionServerServiceInternal sessionServerService
) : IEventListener
{
    private readonly ISwiftlyCore _core = core;
    private readonly ILogService _logService = logService;
    private readonly ILogger<OnSteamAPIActivatedService> _logger = logger;

    private readonly IRSessionServerServiceInternal _sessionServerService = sessionServerService;

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
        _logService.LogDebug($"SteamAPI activated", logger: _logger);
        _sessionServerService.Initialize();
    }
}
