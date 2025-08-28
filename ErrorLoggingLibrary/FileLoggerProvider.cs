using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

internal class FileLoggerProvider : ILoggerProvider {
    private readonly string _filePath;
    private readonly LogLevel _logLevel;

    public FileLoggerProvider (string filePath, LogLevel logLevel = LogLevel.Information){
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        _logLevel = logLevel;

        var logDir = Path.GetDirectoryName(_filePath);

        if (logDir != null && !Directory.Exists(logDir)){
            Directory.CreateDirectory(logDir);
        }
    }

    public ILogger CreateLogger(string categoryName)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}