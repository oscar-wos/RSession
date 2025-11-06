using Microsoft.Extensions.Logging;
using RLogger;
using Sessions.Contracts;
using SwiftlyS2.Shared;

namespace Sessions.Services;

public class LogService(ISwiftlyCore core) : ILogService, IDisposable
{
    private readonly Logger _logger = new(
        Path.Join(core.GameDirectory, "logs", "Sessions"),
        accuracy: 1
    );

    public void LogDebug(string message, Exception? exception = null, ILogger? logger = null)
    {
#if DEBUG
        logger?.LogDebug(exception, "{message}", message);
#endif

        _logger.Debug(message, exception);
    }

    public void LogInformation(string message, Exception? exception = null, ILogger? logger = null)
    {
#if DEBUG
        logger?.LogInformation(exception, "{message}", message);
#endif

        _logger.Information(message, exception);
    }

    public void LogWarning(string message, Exception? exception = null, ILogger? logger = null)
    {
#if DEBUG
        logger?.LogWarning(exception, "{message}", message);
#endif

        _logger.Warning(message, exception);
    }

    public void LogError(string message, Exception? exception = null, ILogger? logger = null)
    {
#if DEBUG
        logger?.LogError(exception, "{message}", message);
#endif

        _logger.Error(message, exception);
    }

    public Exception LogCritical(
        string message,
        Exception? exception = null,
        ILogger? logger = null
    )
    {
#if DEBUG
        logger?.LogCritical(exception, "{message}", message);
#endif

        return _logger.Critical(message, exception);
    }

    public void Dispose()
    {
        _logger.Dispose();
        GC.SuppressFinalize(this);
    }
}
