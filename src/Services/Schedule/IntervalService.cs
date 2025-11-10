using Microsoft.Extensions.Logging;
using Sessions.API.Contracts.Core;
using Sessions.API.Contracts.Database;
using Sessions.API.Contracts.Log;
using Sessions.API.Contracts.Schedule;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Players;

namespace Sessions.Services.Schedule;

public sealed class IntervalService(
    ISwiftlyCore core,
    IDatabaseFactory databaseFactory,
    ILogService logService,
    ILogger<IntervalService> logger,
    Lazy<IPlayerService> playerService
) : IIntervalService
{
    private readonly ISwiftlyCore _core = core;
    private readonly IDatabaseService _databaseService = databaseFactory.Database;

    private readonly ILogService _logService = logService;
    private readonly ILogger<IntervalService> _logger = logger;

    private readonly Lazy<IPlayerService> _playerService = playerService;

    public void OnInterval() =>
        Task.Run(async () =>
        {
            List<int> playerIds = [];
            List<long> sessionIds = [];

            foreach (IPlayer player in _core.PlayerManager.GetAllPlayers())
            {
                if (_playerService.Value.GetPlayer(player) is not { } sessionsPlayer)
                {
                    continue;
                }

                playerIds.Add(sessionsPlayer.Id);

                if (sessionsPlayer.Session is not { } sessionsSession)
                {
                    continue;
                }

                sessionIds.Add(sessionsSession.Id);
            }

            try
            {
                await _databaseService.UpdateSessionsAsync(playerIds, sessionIds);
            }
            catch (Exception ex)
            {
                _logService.LogError("IntervalService.OnInterval()", ex, logger: _logger);
            }
        });
}
