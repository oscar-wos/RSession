using Microsoft.Extensions.DependencyInjection;
using RSession.Contracts.Core;
using RSession.Contracts.Event;
using RSession.Contracts.Log;
using RSession.Contracts.Schedule;
using RSession.Extensions;
using RSession.Models.Config;
using RSession.Services.Core;
using RSession.Services.Log;
using RSession.Services.Schedule;
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

    public override void ConfigureSharedInterface(IInterfaceManager interfaceManager)
    {
        ServiceCollection services = new();

        _ = services.AddSwiftly(Core);

        _ = services.AddDatabases();
        _ = services.AddEvents();

        _ = services.AddSingleton<ILogService, LogService>();

        _ = services.AddSingleton<IRSessionEventInternal, EventService>();
        _ = services.AddSingleton<IRSessionPlayerInternal, PlayerService>();
        _ = services.AddSingleton<IRSessionServerInternal, ServerService>();

        _ = services.AddSingleton<IInterval, IntervalService>();

        _ = services.AddOptionsWithValidateOnStart<DatabaseConfig>().BindConfiguration("database");
        _ = services.AddOptionsWithValidateOnStart<SessionConfig>().BindConfiguration("config");

        _serviceProvider = services.BuildServiceProvider();

        /*
        interfaceManager.AddSharedInterface<IRSessionEvent, IRSessionEvent>(
            "RSession.EventService",
            _serviceProvider.GetRequiredService<IRSessionEvent>()
        );

        interfaceManager.AddSharedInterface<IRSessionPlayer, IRSessionPlayer>(
            "RSession.PlayerService",
            _serviceProvider.GetRequiredService<IRSessionPlayer>()
        );

        interfaceManager.AddSharedInterface<IRSessionServer, IRSessionServer>(
            "RSession.ServerService",
            _serviceProvider.GetRequiredService<IRSessionServer>()
        );
        */
    }

    public override void UseSharedInterface(IInterfaceManager interfaceManager)
    {
        foreach (IEventListener listener in _serviceProvider?.GetServices<IEventListener>() ?? [])
        {
            listener.Subscribe();
        }

        _serviceProvider?.GetService<IInterval>()?.Init();

        try
        {
            InteropHelp.TestIfAvailableGameServer();
            _serviceProvider?.GetService<IRSessionServerInternal>()?.Init();
        }
        catch { }
    }

    public override void Load(bool hotReload)
    {
        _ = Core
            .Configuration.InitializeTomlWithModel<DatabaseConfig>("database.toml", "database")
            .Configure(builder =>
                builder.AddTomlFile("database.toml", optional: false, reloadOnChange: true)
            );

        _ = Core
            .Configuration.InitializeTomlWithModel<SessionConfig>("config.toml", "config")
            .Configure(builder =>
                builder.AddTomlFile("config.toml", optional: false, reloadOnChange: true)
            );
    }

    public override void Unload()
    {
        foreach (IEventListener listener in _serviceProvider?.GetServices<IEventListener>() ?? [])
        {
            listener.Unsubscribe();
        }

        (_serviceProvider as IDisposable)?.Dispose();
    }
}
