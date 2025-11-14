using Microsoft.Extensions.Logging;
using RSession.Contracts.Event;
using RSession.Shared.Contracts.Core;
using RSession.Shared.Contracts.Log;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;

namespace RSession.Services.Event;

public sealed class OnClientSteamAuthorizeService(
    ISwiftlyCore core,
    IRSessionLog logService,
    ILogger<OnClientSteamAuthorizeService> logger,
    IRSessionPlayer playerService,
    IRSessionServer serverService
) : IEventListener
{
    private readonly ISwiftlyCore _core = core;
    private readonly IRSessionLog _logService = logService;
    private readonly ILogger<OnClientSteamAuthorizeService> _logger = logger;

    private readonly IRSessionPlayer _playerService = playerService;
    private readonly IRSessionServer _serverService = serverService;

    public void Subscribe()
    {
        _core.Event.OnClientSteamAuthorize += OnClientSteamAuthorize;
        _logService.LogInformation("OnClientSteamAuthorize subscribed", logger: _logger);
    }

    public void Unsubscribe()
    {
        _core.Event.OnClientSteamAuthorize -= OnClientSteamAuthorize;
        _logService.LogInformation("OnClientSteamAuthorize unsubscribed", logger: _logger);
    }

    private void OnClientSteamAuthorize(IOnClientSteamAuthorizeEvent @event)
    {
        int playerId = @event.PlayerId;

        if (_core.PlayerManager.GetPlayer(playerId) is not { } player)
        {
            _logService.LogWarning(
                $"OnClientSteamAuthorize Player not found - {playerId}",
                logger: _logger
            );

            return;
        }

        if (_serverService.Id is not { } serverId)
        {
            return;
        }

        _logService.LogDebug(
            $"Authorizing player - {player.Controller.PlayerName} ({player.SteamID})",
            logger: _logger
        );

        _playerService.HandlePlayerAuthorize(player, serverId);
    }
}
