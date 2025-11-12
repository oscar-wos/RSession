using Microsoft.Extensions.Logging;
using RSession.API.Contracts.Database;
using RSession.API.Contracts.Log;
using RSession.API.Contracts.Schedule;
using SwiftlyS2.Shared;

namespace RSession.Services.Schedule;

public sealed class IntervalService(
    ILogService logService,
    ILogger<IntervalService> logger,
    ISwiftlyCore core,
    IDatabaseFactory databaseFactory
) : IIntervalService
{
    private readonly ILogService _logService = logService;
    private readonly ILogger<IntervalService> _logger = logger;

    private readonly ISwiftlyCore _core = core;
    private readonly IDatabaseService _databaseService = databaseFactory.Database;

    public void OnInterval() => throw new NotImplementedException();
}
