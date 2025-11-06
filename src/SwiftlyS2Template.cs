using Microsoft.Extensions.DependencyInjection;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Plugins;

namespace SwiftlyS2Template;

[PluginMetadata(Id = "myplugin", Version = "0.0.0", Name = "MyPlugin")]
public partial class SwiftlyS2Template(ISwiftlyCore core) : BasePlugin(core)
{
    //private IServiceProvider? _serviceProvider;
    //private ILogService? _logService;

    public override void ConfigureSharedInterface(IInterfaceManager interfaceManager)
    {
        ServiceCollection services = new();
        _ = services.AddSwiftly(Core);

        //_logService.LogInformation("MyPlugin Loaded", logger: Core.Logger);
    }

    public override void Load(bool hotReload) { }

    public override void Unload()
    {
        //(_serviceProvider as IDisposable)?.Dispose();
    }
}
