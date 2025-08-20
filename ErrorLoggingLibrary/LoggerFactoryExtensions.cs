using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

internal static class LoggerFactoryExtensions {
    public static ILoggerFactory AddFile (this ILoggerFactory factory, string filePath, LogLevel logLevel = LogLevel.Information){
        factory.AddProvider(new FileLoggerProvider(filePath, logLevel));
        return factory;
    }
}