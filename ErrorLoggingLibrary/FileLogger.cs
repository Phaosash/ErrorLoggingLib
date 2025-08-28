using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

internal class FileLogger (string filePath, LogLevel logLevel = LogLevel.Information) : ILogger {
    private readonly string _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    private readonly LogLevel _logLevel = logLevel;

    IDisposable? ILogger.BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _logLevel;

    public void Log<TState> (LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter){
        if (!IsEnabled(logLevel)){ 
            return;
        }

        var logMessage = formatter(state, exception);
        var logEntry = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} [{logLevel}] {logMessage}";

        if (exception != null){
            logEntry += Environment.NewLine + exception.ToString();
        }

        WriteToFile(logEntry);
    }

    private async void WriteToFile (string logEntry){
        try {
            await File.AppendAllTextAsync(_filePath, logEntry + Environment.NewLine);
        } catch (Exception ex) {
            Console.Error.WriteLine($"Failed to write log to file: {ex.Message}");
        }
    }
}