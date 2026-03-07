// Copyright (C) 2026 oscar-wos
using Microsoft.Extensions.Logging;
using RSession.Contracts.Core;
using RSession.Contracts.Event;
using RSession.Contracts.Log;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Events;

namespace RSession.Services.Event;

internal sealed class OnMapUnloadService(
    ISwiftlyCore core,
    ILogService logService,
    ILogger<OnMapUnloadService> logger,
    IMapService mapService
) : IEventListener
{
    private readonly ISwiftlyCore _core = core;
    private readonly ILogService _logService = logService;
    private readonly ILogger<OnMapUnloadService> _logger = logger;

    private readonly IMapService _mapService = mapService;

    public void Subscribe()
    {
        _core.Event.OnMapUnload += OnMapUnload;
        _logService.LogInformation("OnMapUnload subscribed", logger: _logger);
    }

    private void OnMapUnload(IOnMapUnloadEvent @event) => _mapService.HandleMapUnload();

    public void Dispose() => _core.Event.OnMapUnload -= OnMapUnload;
}
