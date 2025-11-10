using Microsoft.Extensions.Logging;
using Sessions.API.Contracts.Hook;
using Sessions.API.Contracts.Log;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Misc;
using SwiftlyS2.Shared.ProtobufDefinitions;

namespace Sessions.Services.Hook;

public sealed class HookManager(
    ISwiftlyCore core,
    ILogService logService,
    ILogger<HookManager> logger,
    IOnClientDisconnectedService onClientDisconnectedService,
    IOnClientMessageService onClientMessageService,
    IOnClientSteamAuthorizeService onClientSteamAuthorizeService,
    IOnMapLoadService onMapLoadService,
    IOnSteamAPIActivatedService onSteamAPIActivatedService
) : IHookManager, IDisposable
{
    private readonly ISwiftlyCore _core = core;

    private readonly ILogService _logService = logService;
    private readonly ILogger<HookManager> _logger = logger;

    private readonly IOnClientDisconnectedService _onClientDisconnectedService =
        onClientDisconnectedService;

    private readonly IOnClientMessageService _onClientMessageService = onClientMessageService;

    private readonly IOnClientSteamAuthorizeService _onClientSteamAuthorizeService =
        onClientSteamAuthorizeService;

    private readonly IOnMapLoadService _onMapLoadService = onMapLoadService;

    private readonly IOnSteamAPIActivatedService _onSteamAPIActivatedService =
        onSteamAPIActivatedService;

    private Guid _cUserMessageSayText2Guid;

    public void Init()
    {
        _core.Event.OnClientDisconnected += _onClientDisconnectedService.OnClientDisconnected;
        _logService.LogInformation("OnClientDisconnected hooked", logger: _logger);

        _core.Event.OnClientSteamAuthorize += _onClientSteamAuthorizeService.OnClientSteamAuthorize;
        _logService.LogInformation("OnClientSteamAuthorize hooked", logger: _logger);

        _core.Event.OnMapLoad += _onMapLoadService.OnMapLoad;
        _logService.LogInformation("OnMapLoad hooked", logger: _logger);

        _core.Event.OnSteamAPIActivated += _onSteamAPIActivatedService.OnSteamAPIActivated;
        _logService.LogInformation("OnSteamAPIActivated hooked", logger: _logger);

        _cUserMessageSayText2Guid = _core.NetMessage.HookServerMessage<CUserMessageSayText2>(msg =>
        {
            _onClientMessageService.OnClientMessage(in msg);
            return HookResult.Continue;
        });

        if (_cUserMessageSayText2Guid != Guid.Empty)
        {
            _logService.LogInformation(
                $"CUserMessageSayText2 hooked - {_cUserMessageSayText2Guid}",
                logger: _logger
            );
        }
        else
        {
            _logService.LogWarning("CUserMessageSayText2 not hooked", logger: _logger);
        }
    }

    public void Dispose()
    {
        _logService.LogInformation("Disposing HookManager", logger: _logger);

        _core.Event.OnClientDisconnected -= _onClientDisconnectedService.OnClientDisconnected;
        _core.Event.OnClientSteamAuthorize -= _onClientSteamAuthorizeService.OnClientSteamAuthorize;
        _core.Event.OnMapLoad -= _onMapLoadService.OnMapLoad;
        _core.Event.OnSteamAPIActivated -= _onSteamAPIActivatedService.OnSteamAPIActivated;
        _core.NetMessage.Unhook(_cUserMessageSayText2Guid);
    }
}
