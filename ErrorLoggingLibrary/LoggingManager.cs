using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

//  How to use: string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "app_log.txt");
public sealed class LoggingManager {
    private static readonly Lazy<LoggingManager> _instance = new(() => new LoggingManager("default_log.txt"));
    private readonly FileLogger _fileLogger;

    private LoggingManager (string filePath){
        _fileLogger = new FileLogger(filePath);
    }

    public static LoggingManager Instance => _instance.Value;

    //  This method logs an informational message using an internal file logger, formatting the log entry to include the provided message.
    public async Task LogInformation (string message){
        await _fileLogger.LogAsync(LogLevel.Information, message, null, (state, exception) => $"Message: {state}");
    }

    //  This method logs an error message along with exception details using an internal file logger, including the exception message and stack trace for better diagnostics.
    public async Task LogError (Exception ex, string message){
        await _fileLogger.LogAsync(LogLevel.Error, message, ex, (state, exception) => $"{state}: {exception?.Message}\nStackTrace: {exception?.StackTrace}");
    }

    //  This method logs a warning message using an internal file logger, prefixing the log entry with "Warning:" to highlight its severity.
    public async Task LogWarning (string message){
        await _fileLogger.LogAsync(LogLevel.Warning, message, null, (state, exception) => $"Warning: {state}");
    }
}