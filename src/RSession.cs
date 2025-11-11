using Microsoft.Extensions.DependencyInjection;
using RSession.API.Contracts.Core;
using RSession.API.Contracts.Event;
using RSession.API.Contracts.Hook;
using RSession.API.Contracts.Log;
using RSession.API.Contracts.Schedule;
using RSession.API.Models.Config;
using RSession.Extensions;
using RSession.Services.Core;
using RSession.Services.Event;
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
    private IServiceProvider? _services;

    private IHookManager? _hookManager;
    private IScheduleManager? _scheduleManager;

    private IServerService? _serverService;

    public override void ConfigureSharedInterface(IInterfaceManager interfaceManager)
    {
        ServiceCollection services = new();

        _ = services.AddSwiftly(Core);

        _ = services.AddDatabases();
        _ = services.AddHooks();
        _ = services.AddSchedules();

        _ = services.AddSingleton<IPlayerService, PlayerService>();

        _ = services.AddSingleton(provider => new Lazy<IPlayerService>(
            provider.GetRequiredService<IPlayerService>
        ));

        _ = services.AddSingleton<IServerService, ServerService>();

        _ = services.AddSingleton(provider => new Lazy<IServerService>(
            provider.GetRequiredService<IServerService>
        ));

        _ = services.AddSingleton<IEventService, EventService>();
        _ = services.AddSingleton<ILogService, LogService>();

        _ = Core
            .Configuration.InitializeTomlWithModel<DatabaseConfig>("database.toml", "database")
            .Configure(builder =>
                builder.AddTomlFile("database.toml", optional: false, reloadOnChange: true)
            );

        _ = services.AddOptionsWithValidateOnStart<DatabaseConfig>().BindConfiguration("database");

        _services = services.BuildServiceProvider();

        _hookManager = _services.GetRequiredService<IHookManager>();
        _scheduleManager = _services.GetRequiredService<IScheduleManager>();

        _serverService = _services.GetRequiredService<IServerService>();

        interfaceManager.AddSharedInterface<IServiceProvider, IServiceProvider>(
            "RSession.ServiceProvider",
            _services
        );
    }

    public override void UseSharedInterface(IInterfaceManager interfaceManager)
    {
        _hookManager?.Init();
        _scheduleManager?.Init();

        try
        {
            InteropHelp.TestIfAvailableGameServer();
            _serverService?.Init();
        }
        catch { }
    }

    public override void Load(bool hotReload) { }

    public override void Unload() => (_services as IDisposable)?.Dispose();
}
