using Microsoft.Extensions.Logging;
using RSession.API.Contracts.Core;
using RSession.API.Contracts.Database;
using RSession.API.Contracts.Event;
using RSession.API.Contracts.Log;
using RSession.API.Structs;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.Players;

namespace RSession.Services.Core;

public sealed class PlayerService(
    IDatabaseFactory databaseFactory,
    ILogService logService,
    ILogger<PlayerService> logger,
    IEventService eventService,
    Lazy<IServerService> serverService
) : IPlayerService
{
    private readonly IDatabaseService _database = databaseFactory.Database;

    private readonly ILogService _logService = logService;
    private readonly ILogger<PlayerService> _logger = logger;

    private readonly IEventService _eventService = eventService;
    private readonly Lazy<IServerService> _serverService = serverService;

    private readonly Dictionary<ulong, SessionsPlayer> _players = [];
    private readonly Dictionary<ulong, int> _lastAuthorizeAttempt = [];

    public void HandlePlayerAuthorize(IPlayer player) =>
        Task.Run(async () =>
        {
            try
            {
                SessionsPlayer sessionsPlayer = await _database
                    .GetPlayerAsync(player.SteamID)
                    .ConfigureAwait(false);

                SessionsSession sessionsSession = await _database
                    .GetSessionAsync(
                        sessionsPlayer.Id,
                        _serverService.Value.Server?.Id ?? 0,
                        player.IPAddress
                    )
                    .ConfigureAwait(false);

                _players[player.SteamID] = sessionsPlayer with { Session = sessionsSession };
                _eventService.InvokePlayerAuthorized(player, _players[player.SteamID]);

                _ = _lastAuthorizeAttempt.Remove(player.SteamID);

                _logService.LogInformation(
                    $"Authorized player - {player.Controller.PlayerName} ({player.SteamID}) | Player ID: {sessionsPlayer.Id} | Session ID: {sessionsSession.Id} | IP: {player.IPAddress}",
                    logger: _logger
                );

                uint recentAliasHash = 0;
                uint currentAliasHash = MurmurHash2.HashString(player.Controller.PlayerName);

                if (
                    await _database.GetAliasAsync(sessionsPlayer.Id).ConfigureAwait(false) is
                    { } recentAlias
                )
                {
                    recentAliasHash = MurmurHash2.HashString(recentAlias.Name);
                }

                if (recentAliasHash != currentAliasHash)
                {
                    await _database
                        .InsertAliasAsync(sessionsPlayer.Id, player.Controller.PlayerName)
                        .ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError(
                    $"Unable to authorize player - {player.Controller.PlayerName} ({player.SteamID})",
                    exception: ex,
                    logger: _logger
                );

                _lastAuthorizeAttempt[player.SteamID] = Environment.TickCount;
            }
        });

    public void HandlePlayerDisconnected(IPlayer player)
    {
        if (GetPlayer(player) is null)
        {
            _logService.LogWarning(
                $"Player not authorized - {player.Controller.PlayerName} ({player.SteamID})",
                logger: _logger
            );

            return;
        }

        _ = _players.Remove(player.SteamID);
        _ = _lastAuthorizeAttempt.Remove(player.SteamID);
    }

    public void HandlePlayerMessage(IPlayer player, short teamNum, bool teamChat, string message) =>
        Task.Run(async () =>
        {
            try
            {
                if (
                    GetPlayer(player) is not { } sessionsPlayer
                    || sessionsPlayer.Session is not { } sessionsSession
                )
                {
                    _logService.LogWarning(
                        $"Player not authorized - {player.Controller.PlayerName} ({player.SteamID})",
                        logger: _logger
                    );

                    return;
                }

                await _database.InsertMessageAsync(
                    sessionsPlayer.Id,
                    sessionsSession.Id,
                    teamNum,
                    teamChat,
                    message
                );
            }
            catch (Exception ex)
            {
                _logService.LogError(
                    $"Unable to log player message - {player.Controller.PlayerName} ({player.SteamID})",
                    exception: ex,
                    logger: _logger
                );

                return;
            }
        });

    public SessionsPlayer? GetPlayer(IPlayer player) =>
        _players.TryGetValue(player.SteamID, out SessionsPlayer sessionsPlayer)
            ? sessionsPlayer
            : null;
}
