using Microsoft.Extensions.Logging;
using RSession.API.Contracts.Core;
using RSession.API.Contracts.Hook;
using RSession.API.Contracts.Log;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;

namespace RSession.Services.Hook;

public sealed class OnClientSteamAuthorizeService(
    ISwiftlyCore core,
    ILogService logService,
    ILogger<OnClientSteamAuthorizeService> logger,
    Lazy<IPlayerService> playerService
) : IOnClientSteamAuthorizeService
{
    private readonly ISwiftlyCore _core = core;

    private readonly ILogService _logService = logService;
    private readonly ILogger<OnClientSteamAuthorizeService> _logger = logger;

    private readonly Lazy<IPlayerService> _playerService = playerService;

    public void OnClientSteamAuthorize(IOnClientSteamAuthorizeEvent @event)
    {
        if (_core.PlayerManager.GetPlayer(@event.PlayerId) is not { } player)
        {
            _logService.LogWarning($"Player not authorized - {@event.PlayerId}", logger: _logger);
            return;
        }

        _playerService.Value.HandlePlayerAuthorize(player);
    }
}
