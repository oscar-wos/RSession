using Microsoft.Extensions.Logging;
using RSession.Contracts.Core;
using RSession.Contracts.Database;
using RSession.Contracts.Log;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Players;

namespace RSession.Services.Core;

internal sealed class SessionPlayerService : IRSessionPlayerServiceInternal, IDisposable
{
    private readonly ISwiftlyCore _core;
    private readonly ILogService _logService;
    private readonly ILogger<SessionPlayerService> _logger;

    private readonly IDatabaseService _database;
    private readonly IRSessionEventServiceInternal _sessionEventService;

    private readonly Dictionary<ulong, int> _players = [];
    private readonly Dictionary<ulong, long> _sessions = [];

    public SessionPlayerService(
        ISwiftlyCore core,
        ILogService logService,
        ILogger<SessionPlayerService> logger,
        IDatabaseFactory databaseFactory,
        IRSessionEventServiceInternal sessionEventService
    )
    {
        _core = core;
        _logService = logService;
        _logger = logger;

        _database = databaseFactory.Database;
        _sessionEventService = sessionEventService;

        _sessionEventService.OnServerRegistered += OnServerRegistered;
    }

    public int? GetPlayerId(IPlayer player) =>
        _players.TryGetValue(player.SteamID, out int playerId) ? playerId : null;

    public long? GetSessionId(IPlayer player) =>
        _sessions.TryGetValue(player.SteamID, out long sessionId) ? sessionId : null;

    public void HandlePlayerAuthorize(IPlayer player, short serverId) =>
        Task.Run(async () =>
        {
            ulong steamId = player.SteamID;

            try
            {
                int playerId = await _database.GetPlayerAsync(steamId).ConfigureAwait(false);

                long sessionId = await _database
                    .GetSessionAsync(playerId, serverId, player.IPAddress)
                    .ConfigureAwait(false);

                _logService.LogInformation(
                    $"Player registered - {player.Controller.PlayerName} ({player.SteamID}) | Player ID: {playerId} | Session ID: {sessionId}",
                    logger: _logger
                );

                _players[steamId] = playerId;
                _sessions[steamId] = sessionId;

                _sessionEventService.InvokePlayerRegistered(player, playerId, sessionId);
            }
            catch (Exception ex)
            {
                _logService.LogError(
                    $"Unable to register player - {player.Controller.PlayerName} ({player.SteamID})",
                    exception: ex,
                    logger: _logger
                );
            }
        });

    public void HandlePlayerDisconnected(IPlayer player)
    {
        if (GetPlayerId(player) is null)
        {
            _logService.LogWarning(
                $"Player not registered - {player.Controller.PlayerName} ({player.SteamID})",
                logger: _logger
            );

            return;
        }

        _ = _players.Remove(player.SteamID);
        _ = _sessions.Remove(player.SteamID);
    }

    private void OnServerRegistered(short serverId)
    {
        foreach (IPlayer player in _core.PlayerManager.GetAllPlayers())
        {
            if (player is not { IsValid: true, IsAuthorized: true })
            {
                continue;
            }

            HandlePlayerAuthorize(player, serverId);
        }
    }

    public void Dispose() => _sessionEventService.OnServerRegistered -= OnServerRegistered;
}
