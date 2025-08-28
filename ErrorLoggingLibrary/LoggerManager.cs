using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

public sealed class LoggerManager {
    private static readonly Lazy<LoggerManager> _instance = new(() => new LoggerManager(new FileLogger()));
    private readonly FileLogger _fileLogger;

    private LoggerManager (FileLogger fileLogger){
        _fileLogger = fileLogger ?? throw new ArgumentNullException(nameof(fileLogger));
    }

    public static LoggerManager Instance => _instance.Value;

    public void LogInformation (string message){
        _fileLogger.Log(LogLevel.Information, message, null, (state, exception) => $"Message: {state}");
    }

    public void LogError (Exception ex, string message){
        _fileLogger.Log(LogLevel.Error, message, ex, (state, exception) => $"{state}: {exception?.Message}\nStackTrace: {exception?.StackTrace}");
    }

    public void LogWarning (string message){
        _fileLogger.Log(LogLevel.Warning, message, null, (state, exception) => $"Warning: {state}");
    }

    public void LogWarningWithException (Exception ex, string message){
        _fileLogger.Log(LogLevel.Warning, message, ex, (state, exception) => $"Warning: {state}. Exception: {exception?.Message}");
    }
}