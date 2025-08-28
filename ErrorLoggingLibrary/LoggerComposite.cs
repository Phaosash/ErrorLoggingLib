using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

internal class LoggerComposite(IEnumerable<ILogger> loggers) : ILogger {
    private readonly IEnumerable<ILogger> _loggers = loggers;

    public IDisposable? BeginScope<TState> (TState state) where TState : notnull {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel) => _loggers.All(logger => logger.IsEnabled(logLevel));

    public void Log<TState> (LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter){
        foreach (var logger in _loggers){
            logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}