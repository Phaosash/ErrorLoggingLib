using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

internal static class LoggerFactorySetup {
    public static ILoggerFactory CreateLoggerFactory (string logFilePath){
        var factory = LoggerFactory.Create(builder => {
            builder.AddProvider(new LoggerFileProvider(logFilePath));
        });

        return factory;
    }
}