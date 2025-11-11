using Microsoft.Extensions.Logging;
using RSession.API.Contracts.Core;
using RSession.API.Contracts.Database;
using RSession.API.Contracts.Log;
using RSession.API.Structs;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Players;

namespace RSession.Services.Core;

public sealed class ServerService(
    ISwiftlyCore core,
    IDatabaseFactory databaseFactory,
    ILogService logService,
    ILogger<ServerService> logger,
    Lazy<IPlayerService> playerService
) : IServerService
{
    private readonly ISwiftlyCore _core = core;
    private readonly IDatabaseService _database = databaseFactory.Database;

    private readonly ILogService _logService = logService;
    private readonly ILogger<ServerService> _logger = logger;

    private readonly Lazy<IPlayerService> _playerService = playerService;

    public SessionsServer? Server { get; private set; }

    public void Init() =>
        Task.Run(async () =>
        {
            try
            {
                string serverIp = _core.Engine.ServerIP ?? "0.0.0.0";
                ushort serverPort = (ushort)(_core.ConVar.Find<int>("hostport")?.Value ?? 0);

                _logService.LogInformation(
                    $"Initializing server - {serverIp}:{serverPort}",
                    logger: _logger
                );

                await _database.InitAsync().ConfigureAwait(false);

                SessionsServer sessionsServer = await _database
                    .GetServerAsync(serverIp, serverPort)
                    .ConfigureAwait(false);

                SessionsMap sessionsMap = await _database
                    .GetMapAsync(_core.Engine.GlobalVars.MapName)
                    .ConfigureAwait(false);

                await _database
                    .InsertRotationAsync(sessionsServer.Id, sessionsMap.Id)
                    .ConfigureAwait(false);

                Server = sessionsServer with { Map = sessionsMap };

                foreach (IPlayer player in _core.PlayerManager.GetAllPlayers())
                {
                    if (player is not { IsValid: true, IsAuthorized: true })
                    {
                        continue;
                    }

                    _playerService.Value.HandlePlayerAuthorize(player);
                }
            }
            catch (Exception ex)
            {
                throw _logService.LogCritical(
                    "ServerService.Init()",
                    exception: ex,
                    logger: _logger
                );
            }
        });

    public void HandleMapLoad(string mapName) =>
        Task.Run(async () =>
        {
            try
            {
                if (Server is not { } sessionsServer)
                {
                    return;
                }

                SessionsMap sessionsMap = await _database
                    .GetMapAsync(mapName)
                    .ConfigureAwait(false);

                await _database
                    .InsertRotationAsync(sessionsServer.Id, sessionsMap.Id)
                    .ConfigureAwait(false);

                Server = sessionsServer with { Map = sessionsMap };
            }
            catch (Exception ex)
            {
                _logService.LogError(
                    "ServerService.HandleMapLoad()",
                    exception: ex,
                    logger: _logger
                );
            }
        });
}
