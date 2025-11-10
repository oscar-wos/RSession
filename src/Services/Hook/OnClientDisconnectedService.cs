using Microsoft.Extensions.Logging;
using Sessions.API.Contracts.Core;
using Sessions.API.Contracts.Hook;
using Sessions.API.Contracts.Log;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace Sessions.Services.Hook;

public sealed class OnClientDisconnectedService(
    ISwiftlyCore core,
    ILogService logService,
    ILogger<OnClientDisconnectedService> logger,
    Lazy<IPlayerService> playerService
) : IOnClientDisconnectedService
{
    private readonly ISwiftlyCore _core = core;

    private readonly ILogService _logService = logService;
    private readonly ILogger<OnClientDisconnectedService> _logger = logger;

    private readonly Lazy<IPlayerService> _playerService = playerService;

    public void OnClientDisconnected(IOnClientDisconnectedEvent @event)
    {
        if (@event.Reason == ENetworkDisconnectionReason.NETWORK_DISCONNECT_SHUTDOWN)
        {
            return;
        }

        if (_core.PlayerManager.GetPlayer(@event.PlayerId) is not { } player)
        {
            _logService.LogWarning($"Player not found - {@event.PlayerId}", logger: _logger);
            return;
        }

        if (!player.IsAuthorized)
        {
            return;
        }

        _playerService.Value.HandlePlayerDisconnected(player);
    }
}
