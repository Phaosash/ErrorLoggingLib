using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

internal class LoggerFile(string filePath, LogLevel minLogLevel = LogLevel.Information, long maxFileSizeBytes = 5 * 1024 * 1024) : ILogger {
    private readonly string _filePath = filePath;
    private readonly long _maxFileSizeBytes = maxFileSizeBytes;
    private readonly LogLevel _minLogLevel = minLogLevel;
    private readonly Lock _fileLock = new();

    //  This can be used to return something useful if necessary
    public IDisposable? BeginScope<TState> (TState state) where TState : notnull {
        return null;
    }

    public bool IsEnabled (LogLevel logLevel) => logLevel >= _minLogLevel;

    public void Log<TState> (LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter){
        ArgumentNullException.ThrowIfNull(formatter);

        var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] {formatter(state, exception)}";

        Task.Run(() => {
            lock (_fileLock){
                LogToFile(logMessage);
            }
        });
    }

    private void LogToFile (string logMessage){
        try {
            RotateLogFileIfNeeded();
            File.AppendAllText(_filePath, logMessage + Environment.NewLine);
        } catch (Exception ex) {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }

    private void RotateLogFileIfNeeded (){
        var fileInfo = new FileInfo(_filePath);

        if (fileInfo.Length > _maxFileSizeBytes){
            var backupFilePath = $"{_filePath}.{DateTime.Now:yyyyMMddHHmmss}.bak";

            File.Move(_filePath, backupFilePath);
            File.Create(_filePath).Dispose();
        }
    }
}