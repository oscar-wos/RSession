using Microsoft.Extensions.DependencyInjection;
using Sessions.Contracts;
using Sessions.Services;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Plugins;

namespace Sessions;

[PluginMetadata(Id = "sessions", Version = "0.0.0", Name = "Sessions")]
public partial class Sessions(ISwiftlyCore core) : BasePlugin(core)
{
    private IServiceProvider? _serviceProvider;
    private ILogService? _logService;

    public override void ConfigureSharedInterface(IInterfaceManager interfaceManager)
    {
        ServiceCollection services = new();

        _ = services.AddSwiftly(Core);
        _ = services.AddSingleton<ILogService, LogService>();

        _serviceProvider = services.BuildServiceProvider();

        _logService = _serviceProvider.GetRequiredService<ILogService>();
        _logService.LogInformation("Sessions Loaded", logger: Core.Logger);
    }

    public override void Load(bool hotReload) { }

    public override void Unload() => (_serviceProvider as IDisposable)?.Dispose();
}
