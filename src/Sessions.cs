using Microsoft.Extensions.DependencyInjection;
using Sessions.API.Contracts.Core;
using Sessions.API.Contracts.Hook;
using Sessions.API.Contracts.Log;
using Sessions.API.Contracts.Schedule;
using Sessions.API.Models.Config;
using Sessions.Extensions;
using Sessions.Services.Core;
using Sessions.Services.Log;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Plugins;
using SwiftlyS2.Shared.SteamAPI;
using Tomlyn.Extensions.Configuration;

namespace Sessions;

[PluginMetadata(
    Id = "Sessions",
    Version = "0.0.0",
    Name = "Sessions",
    Website = "https://github.com/oscar-wos/Sessions-swiftly",
    Author = "oscar-wos"
)]
public sealed partial class Sessions(ISwiftlyCore core) : BasePlugin(core)
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
            "Sessions.ServiceProvider",
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
