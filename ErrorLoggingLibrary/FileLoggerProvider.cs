using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

internal class FileLoggerProvider (string filePath, LogLevel logLevel = LogLevel.Information) : ILoggerProvider {
    private readonly string _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    private readonly LogLevel _logLevel = logLevel;

    //  This method creates and returns a new instance of FileLogger. It takes a categoryName
    //  as input, which can be used for categorizing logs, though it is not used in this
    //  implementation. The method uses the _filePath and _logLevel fields to configure the
    //  logger to write logs to the specified file at or above the given log level. The
    //  logger created by this method is responsible for logging messages to the file.
    public ILogger CreateLogger (string categoryName){
        return new FileLogger(_filePath, _logLevel);
    }

    //  This method is part of the IDisposable interface, and in this case, it is an empty
    //  implementation. Its purpose is to release any resources held by the object when it
    //  is no longer needed. Since this method is empty, it indicates that the FileLogger
    //  or the class implementing this method does not have any unmanaged resources (such
    //  as file handles, database connections, etc.) that need to be explicitly cleaned up.
    //  However, it provides a placeholder in case resource management becomes necessary
    //  in the future.
    public void Dispose() { }
}