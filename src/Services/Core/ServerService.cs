using Microsoft.Extensions.Logging;
using Sessions.API.Contracts.Core;
using Sessions.API.Contracts.Database;
using Sessions.API.Contracts.Log;
using Sessions.API.Structs;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Players;

namespace Sessions.Services.Core;

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

                SessionsMap sessionsMap = await _database.GetMapAsync(
                    _core.Engine.GlobalVars.MapName
                );

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
}
