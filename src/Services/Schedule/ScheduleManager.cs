using Microsoft.Extensions.Logging;
using Sessions.API.Contracts.Log;
using Sessions.API.Contracts.Schedule;
using SwiftlyS2.Shared;

namespace Sessions.Services.Schedule;

public sealed class ScheduleManager(
    ISwiftlyCore core,
    ILogService logService,
    ILogger<ScheduleManager> logger,
    IIntervalService intervalService
) : IScheduleManager
{
    private readonly ISwiftlyCore _core = core;

    private readonly ILogService _logService = logService;
    private readonly ILogger<ScheduleManager> _logger = logger;

    private readonly IIntervalService _intervalService = intervalService;

    public void Init()
    {
        _ = _core.Scheduler.Repeat(64, _intervalService.OnInterval);
        _logService.LogInformation("ScheduleManager initialized", logger: _logger);
    }
}
