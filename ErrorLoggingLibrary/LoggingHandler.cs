using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ErrorLoggingLibrary;

public class LoggingHandler {
    private readonly ILogger<LoggingHandler>? _logger;
    private static LoggingHandler? _instance;
    private static readonly Lock _lock = new();

    private LoggingHandler (ILogger<LoggingHandler> logger){
        _logger = logger;
    }

    public static LoggingHandler Instance {
        get {
            if (_instance == null){
                lock (_lock){
                    if (_instance == null){
                        var builder = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

                        IConfiguration config = builder.Build();

                        var filePath = config.GetSection("FileLogging:FilePath").Value ?? "logs/app.log";

                        Log.Logger = new LoggerConfiguration().WriteTo.File(filePath, rollingInterval: RollingInterval.Day).CreateLogger();

                        var loggerFactory = LoggerFactory.Create(builder => {
                            builder.AddSerilog();
                            builder.AddConsole();
                        });

                        loggerFactory.AddFile(filePath);

                        var logger = loggerFactory.CreateLogger<LoggingHandler>();
                        _instance = new LoggingHandler(logger);
                    }
                }
            }

            return _instance;
        }
    }

    public void LogError (string msg, Exception ex){
        _logger?.LogError(ex, "Error occurred: {Message}", msg);
    }

    public void LogInformation (string msg){
        _logger?.LogInformation("Information: {Message}", msg);
    }

    public void LogWarning (string msg){
        _logger?.LogWarning("Warning: {Message}", msg);
    }
}