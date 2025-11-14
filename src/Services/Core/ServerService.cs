using Microsoft.Extensions.Logging;
using RSession.Contracts.Database;
using RSession.Shared.Contracts.Core;
using RSession.Shared.Contracts.Log;
using SwiftlyS2.Shared;

namespace RSession.Services.Core;

public sealed class ServerService(
    ISwiftlyCore core,
    IRSessionLog logService,
    ILogger<ServerService> logger,
    IDatabaseFactory databaseFactory,
    IRSessionEvent _eventService
) : IRSessionServer
{
    private readonly ISwiftlyCore _core = core;
    private readonly IRSessionLog _logService = logService;
    private readonly ILogger<ServerService> _logger = logger;

    private readonly IDatabaseService _database = databaseFactory.Database;
    private readonly IRSessionEvent _eventService = _eventService;

    public short? Id { get; private set; }

    public void Init() =>
        Task.Run(async () =>
        {
            string ip = _core.Engine.ServerIP ?? "0.0.0.0";
            ushort port = (ushort)(_core.ConVar.Find<int>("hostport")?.Value ?? 0);

            try
            {
                await _database.InitAsync().ConfigureAwait(false);
                short serverId = await _database.GetServerAsync(ip, port).ConfigureAwait(false);

                _logService.LogInformation(
                    $"Server registered - {ip}:{port} | Server ID: {serverId}",
                    logger: _logger
                );

                Id = serverId;

                _eventService.InvokeServerRegistered(serverId);
            }
            catch (Exception ex)
            {
                _logService.LogError(
                    $"Unable to register server - {ip}:{port}",
                    exception: ex,
                    logger: _logger
                );
            }
        });
}
