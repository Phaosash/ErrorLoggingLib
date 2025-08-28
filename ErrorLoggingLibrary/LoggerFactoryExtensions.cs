using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

internal static class LoggerFactoryExtensions {
    public static ILoggerFactory AddFile (this ILoggerFactory factory, string filePath, LogLevel logLevel = LogLevel.Information){
        factory.AddProvider(new FileLoggerProvider(filePath, logLevel));
        return factory;
    }

    public static ILoggerFactory AddConsoleConditional (this ILoggerFactory factory, LogLevel logLevel = LogLevel.Information){
        var builder = LoggerFactory.Create(builder => {
            if (System.Diagnostics.Debugger.IsAttached || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"){
                builder.AddConsole(options => {
                    options.LogToStandardErrorThreshold = logLevel;
                });

                factory.AddFile("log.txt", logLevel);
            }
        });

        return factory;
    }
}