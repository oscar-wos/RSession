using Microsoft.Extensions.Logging;
using Sessions.API.Contracts.Hook;
using Sessions.API.Contracts.Log;
using SwiftlyS2.Shared;

namespace Sessions.Services.Hook;

public sealed class PlayerMessageService(
    ISwiftlyCore core,
    ILogService logService,
    ILogger<PlayerMessageService> logger
) : IPlayerMessageService
{
    private readonly ISwiftlyCore _core = core;
    private readonly ILogService _logService = logService;
    private readonly ILogger<PlayerMessageService> _logger = logger;
}
