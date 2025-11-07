using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sessions.API.Contracts.Hook;
using Sessions.API.Contracts.Log;
using SwiftlyS2.Shared;

namespace Sessions.Services.Hook;

internal sealed class HookManager : IHookManager, IDisposable
{
    private readonly ISwiftlyCore _core;
    private readonly IServiceProvider _services;
    private readonly ILogService _logService;
    private readonly ILogger<HookManager> _logger;

    private readonly IPlayerAuthorizeService _playerAuthorizeService;

    public HookManager(
        ISwiftlyCore core,
        IServiceProvider services,
        ILogService logService,
        ILogger<HookManager> logger
    )
    {
        _core = core;
        _services = services;
        _logService = logService;
        _logger = logger;

        _playerAuthorizeService = _services.GetRequiredService<IPlayerAuthorizeService>();

        _core.Event.OnClientSteamAuthorize += _playerAuthorizeService.OnClientSteamAuthorize;
        _logService.LogInformation("Loaded HookManager", logger: _logger);
    }

    public void Dispose()
    {
        _logService.LogInformation("Disposing HookManager", logger: _logger);
        _core.Event.OnClientSteamAuthorize -= _playerAuthorizeService.OnClientSteamAuthorize;
    }
}
