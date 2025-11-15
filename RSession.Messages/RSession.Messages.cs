/*using Microsoft.Extensions.DependencyInjection;
using RSession.Messages.Contracts.Core;
using RSession.Messages.Contracts.Hook;
using RSession.Messages.Contracts.Log;
using RSession.Messages.Services.Core;
using RSession.Messages.Services.Database;
using RSession.Messages.Services.Log;
using RSession.Shared.Contracts;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Plugins;

namespace RSession.Messages;

[PluginMetadata(
    Id = "RSession.Messages",
    Version = "0.0.0",
    Name = "RSession.Messages",
    Website = "https://github.com/oscar-wos/RSession",
    Author = "oscar-wos"
)]
public sealed partial class Messages(ISwiftlyCore core) : BasePlugin(core)
{
    private IServiceProvider? _serviceProvider;

    public override void UseSharedInterface(IInterfaceManager interfaceManager)
    {
        if (
            !interfaceManager.HasSharedInterface("RSession.Database")
            || !interfaceManager.HasSharedInterface("RSession.Event")
            || !interfaceManager.HasSharedInterface("RSession.Player")
            || !interfaceManager.HasSharedInterface("RSession.Server")
        )
        {
            return;
        }

        IRSessionDatabaseService sessionDatabase =
            interfaceManager.GetSharedInterface<IRSessionDatabaseService>(
                "RSession.DatabaseService"
            );

        ISessionEventService sessionEvent =
            interfaceManager.GetSharedInterface<ISessionEventService>("RSession.EventService");

        Shared.Contracts.ISessionPlayerService sessionPlayer =
            interfaceManager.GetSharedInterface<Shared.Contracts.ISessionPlayerService>(
                "RSession.PlayerService"
            );

        ISessionServerService sessionServer =
            interfaceManager.GetSharedInterface<ISessionServerService>("RSession.ServerService");

        _serviceProvider
            ?.GetRequiredService<Contracts.Core.IPlayerService>()
            .Initialize(sessionPlayer, sessionServer);

        //_serviceProvider?.GetRequiredService<DatabaseFactory>().Initialize(sessionEvent);
    }

    public override void Load(bool hotReload)
    {
        ServiceCollection services = new();

        _ = services.AddSwiftly(Core);

        _ = services.AddSingleton<ILogService, LogService>();
        _ = services.AddSingleton<Contracts.Core.IPlayerService, PlayerService>();

        _ = services.AddSingleton<PostgresService>();
        _ = services.AddSingleton<SqlService>();
        _ = services.AddSingleton<DatabaseFactory>();

        _ = services.AddSingleton<IHook, OnUserMessageSayText2Service>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public override void Unload()
    {
        foreach (IHook hook in _serviceProvider?.GetServices<IHook>() ?? [])
        {
            hook.Unregister();
        }

        (_serviceProvider as IDisposable)?.Dispose();
    }
}
*/
