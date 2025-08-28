using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

public class LoggerManager (ILogger<LoggerManager> logger, LoggerFile? loggerFile = null) : IDisposable {
    private readonly ILogger<LoggerManager> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly LoggerFile? _loggerFile = loggerFile;

    public void LogInformation (string message){
        _logger.LogInformation("Message: {Message}", message);
    }

    public void LogError (Exception ex, string message){
        _logger.LogError(ex, "Error occurred: {Message}", message);
    }

    public void LogWarning (string message){
        _logger.LogWarning("Warning: {Message}", message);
    }

    public void Dispose (){
        _loggerFile?.Dispose();
        GC.SuppressFinalize(this);
    }
}