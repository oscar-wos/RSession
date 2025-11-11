using Microsoft.Extensions.Logging;
using RSession.API.Contracts.Core;
using RSession.API.Contracts.Hook;
using RSession.API.Contracts.Log;

namespace RSession.Services.Hook;

public sealed class OnSteamAPIActivatedService(
    ILogService logService,
    ILogger<OnSteamAPIActivatedService> logger,
    Lazy<IServerService> serviceService
) : IOnSteamAPIActivatedService
{
    private readonly ILogService _logService = logService;
    private readonly ILogger<OnSteamAPIActivatedService> _logger = logger;

    private readonly Lazy<IServerService> _serviceService = serviceService;

    public void OnSteamAPIActivated()
    {
        _logService.LogInformation("Steam API activated", logger: _logger);
        _serviceService.Value.Init();
    }
}
