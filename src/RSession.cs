using Microsoft.Extensions.DependencyInjection;
using RSession.API.Contracts.Core;
using RSession.API.Contracts.Database;
using RSession.API.Contracts.Event;
using RSession.API.Contracts.Log;
using RSession.API.Models.Config;
using RSession.Extensions;
using RSession.Services.Core;
using RSession.Services.Log;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Plugins;
using SwiftlyS2.Shared.SteamAPI;
using Tomlyn.Extensions.Configuration;

namespace RSession;

[PluginMetadata(
    Id = "RSession",
    Version = "0.0.0",
    Name = "RSession",
    Website = "https://github.com/oscar-wos/RSession",
    Author = "oscar-wos"
)]
public sealed partial class RSession(ISwiftlyCore core) : BasePlugin(core)
{
    private IServiceProvider? _serviceProvider;
    private IServerService? _serverService;

    public override void ConfigureSharedInterface(IInterfaceManager interfaceManager)
    {
        ServiceCollection services = new();

        _ = services.AddSwiftly(Core);

        _ = services.AddDatabases();
        _ = services.AddEvents();

        _ = services.AddSingleton<ILogService, LogService>();
        _ = services.AddSingleton<IEventService, EventService>();

        _ = services.AddSingleton<IPlayerService, PlayerService>();
        _ = services.AddSingleton<IServerService, ServerService>();

        _ = Core
            .Configuration.InitializeTomlWithModel<DatabaseConfig>("database.toml", "database")
            .Configure(builder =>
                builder.AddTomlFile("database.toml", optional: false, reloadOnChange: true)
            );

        _ = services.AddOptionsWithValidateOnStart<DatabaseConfig>().BindConfiguration("database");

        _serviceProvider = services.BuildServiceProvider();
        _serverService = _serviceProvider.GetRequiredService<IServerService>();

        interfaceManager.AddSharedInterface<IDatabaseFactory, IDatabaseFactory>(
            "RSession.DatabaseFactory",
            _serviceProvider.GetRequiredService<IDatabaseFactory>()
        );

        interfaceManager.AddSharedInterface<IEventService, IEventService>(
            "RSession.EventService",
            _serviceProvider.GetRequiredService<IEventService>()
        );

        interfaceManager.AddSharedInterface<IPlayerService, IPlayerService>(
            "RSession.PlayerService",
            _serviceProvider.GetRequiredService<IPlayerService>()
        );

        interfaceManager.AddSharedInterface<IServerService, IServerService>(
            "RSession.ServerService",
            _serverService
        );
    }

    public override void UseSharedInterface(IInterfaceManager interfaceManager)
    {
        foreach (IEventListener listener in _serviceProvider?.GetServices<IEventListener>() ?? [])
        {
            listener.Subscribe();
        }

        try
        {
            InteropHelp.TestIfAvailableGameServer();
            _serverService?.HandleInit();
        }
        catch { }
    }

    public override void Load(bool hotReload) { }

    public override void Unload()
    {
        foreach (IEventListener listener in _serviceProvider?.GetServices<IEventListener>() ?? [])
        {
            listener.Unsubscribe();
        }

        (_serviceProvider as IDisposable)?.Dispose();
    }
}
