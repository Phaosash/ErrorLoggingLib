using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

internal class FileLoggerProvider (string filePath, LogLevel logLevel = LogLevel.Information) : ILoggerProvider {
    private readonly string _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    private readonly LogLevel _logLevel = logLevel;

    public ILogger CreateLogger (string categoryName){
        return new FileLogger(_filePath, _logLevel);
    }

    public void Dispose() { }
}