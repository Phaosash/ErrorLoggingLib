# Error Logging Class Library

A simple yet effective logging class library for error and event logging, utilizing `Microsoft.Extensions.Logging` for logging capabilities. This implementation writes logs to a text file located at the root directory of the project, with a fallback option for scenarios where the root directory cannot be determined.

## Features

* **Custom Logging**: Implements a custom `FileLogger` class to log events, errors, and warnings to a text file.
* **Singleton Pattern**: Ensures only one instance of the logger is used throughout the application's lifecycle.
* **Exception Logging**: Logs both error messages and associated exceptions.
* **Fallback Directory**: Provides a fallback directory in case the root directory cannot be determined, ensuring logging continues seamlessly.
* **Log Levels**: Configurable log level to determine which messages are logged (e.g., Information, Warning, Error).

## Installation

Clone the repository and install the necessary dependencies:

```bash
git clone https://github.com/Phaosash/ErrorLoggingLib.git
cd your-repo-name
dotnet restore
```

## Usage

### Setup

To use the logging functionality, follow these steps:

1. **Create a new instance** of the `LoggingHandler` class:

   ```csharp
   var logger = LoggingHandler.Instance;
   ```

2. **Log messages** of different severities:

   * **Information**: Logs informational messages.
   * **Warning**: Logs warning messages.
   * **Error**: Logs error messages with exception details.

   Example:

   ```csharp
   logger.LogInformation("This is an informational message.");
   logger.LogWarning("This is a warning message.");
   logger.LogError("This is an error message.", new Exception("Test exception"));
   ```

### Configuration

* The log file will be written to the **root directory** of the project. If the root directory cannot be determined, a fallback directory (e.g., `D:\Documents`) is used.
* Log entries will be written to `log.txt` in the root directory.

You can also modify the log file location by changing the path in the `WriteToFile` method.

## Code Walkthrough

### LoggingHandler Class

The `LoggingHandler` class is a singleton that provides methods to log messages. It utilizes the `Microsoft.Extensions.Logging` package and allows you to log messages at different levels (Information, Warning, Error).

```csharp
public class LoggingHandler
{
    private readonly ILogger<LoggingHandler>? _logger;
    private static LoggingHandler? _instance;
    private static readonly object _lock = new();

    private LoggingHandler(ILogger<LoggingHandler> logger)
    {
        _logger = logger;
    }

    public static LoggingHandler Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        var loggerFactory = LoggerFactory.Create(builder =>
                        {
                            builder.AddFile("log.txt", LogLevel.Information);
                        });

                        var logger = loggerFactory.CreateLogger<LoggingHandler>();
                        _instance = new LoggingHandler(logger);
                    }
                }
            }

            return _instance;
        }
    }

    public void LogError(string msg, Exception ex)
    {
        _logger!.LogError("Error: {Message}, Exception: {Exception}", msg, ex);
    }

    public void LogInformation(string msg)
    {
        _logger!.LogInformation("Information: {Message}", msg);
    }

    public void LogWarning(string msg)
    {
        _logger!.LogWarning("Warning: {Message}", msg);
    }
}
```

### FileLogger Class

The `FileLogger` class implements the `ILogger` interface and writes log entries to a file. It provides a custom implementation of `Log` and `BeginScope`.

```csharp
public class FileLogger : ILogger
{
    private readonly string _filePath;
    private readonly LogLevel _logLevel;

    public FileLogger(string filePath, LogLevel logLevel = LogLevel.Information)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        _logLevel = logLevel;
    }

    public IDisposable? BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _logLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var logMessage = formatter(state, exception);
        var logEntry = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} [{logLevel}] {logMessage}";
        WriteToFile(logEntry);
    }

    private void WriteToFile(string logEntry)
    {
        try
        {
            var rootDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.FullName;

            if (rootDirectory == null)
            {
                rootDirectory = @"D:\Documents"; // Default fallback directory
            }

            var logFilePath = Path.Combine(rootDirectory, "log.txt");
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to write log to file: {ex.Message}");
        }
    }
}
```

## Contributing

We welcome contributions! If you have ideas for enhancements or find a bug, feel free to submit an issue or pull request.

### Steps to Contribute:

1. Fork this repository.
2. Clone your forked repository to your local machine.
3. Make changes or fix bugs in a new branch.
4. Run tests and ensure that everything works as expected.
5. Submit a pull request with a clear description of what you've changed.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

### Notes

* If the log file is not located in the expected directory, please verify that your application is not running from a different directory (e.g., from `bin\Debug\net9.0`). You may need to adjust the logging path accordingly.

---
