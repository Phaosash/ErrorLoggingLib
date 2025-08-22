using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

internal static class LoggerFactoryExtensions {
    //  This method is an extension method for the ILoggerFactory interface that allows you to
    //  easily add a custom file logger provider to the logging system. It takes a filePath where
    //  the log entries will be saved and an optional logLevel that determines the minimum level
    //  of logs to be written (default is Information).
    //  This method creates a new instance of FileLoggerProvider, passing in the specified filePath
    //  and logLevel, and adds it to the ILoggerFactory. By returning the factory, it allows for
    //  method chaining, enabling fluent configuration of the logging system.
    //  In summary, the method simplifies the process of setting up file-based logging by adding the
    //  FileLoggerProvider to the logging configuration.
    public static ILoggerFactory AddFile (this ILoggerFactory factory, string filePath, LogLevel logLevel = LogLevel.Information){
        factory.AddProvider(new FileLoggerProvider(filePath, logLevel));
        return factory;
    }
}