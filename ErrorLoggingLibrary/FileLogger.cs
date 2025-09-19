using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace ErrorLoggingLibrary;

internal class FileLogger (string logFilePath){
    private readonly string _logFilePath = logFilePath ?? throw new ArgumentNullException(nameof(logFilePath));
    private readonly Lock _fileLock = new();

    //  This method logs a message with a specified log level, state, and optional exception by formatting the log entry with a timestamp,
    //  then asynchronously writing it to a file in a thread-safe manner using a lock.
    public async Task LogAsync<TState> (LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter,
                                        [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0){
        ArgumentNullException.ThrowIfNull(formatter);

        var logMessage = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} [{logLevel}] {formatter(state, exception)}" + $"\nSource: {Path.GetFileName(filePath)}.{memberName} Line: {lineNumber}";

        await LogToFileAsync(logMessage);
    }

    //  This method writes a log message to a "logfile.txt" inside a logs directory in the application's base path,
    //  creating the directory if it doesn't exist, and catches any exceptions during the write operation to output
    //  an error to the console.
    private async Task LogToFileAsync (string logMessage){
        try {
            string logsDirectory = Path.GetDirectoryName(_logFilePath) ?? throw new InvalidOperationException("Invalid log file path");

            lock (_fileLock){
                if (!Directory.Exists(logsDirectory)){
                    Directory.CreateDirectory(logsDirectory);
                }
            }

            await WriteToFileAsync(logMessage);
        } catch (Exception ex){
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }

    //  This method writes the log message asynchronously to the file.
    //  Moved to its own method to get around the lock thread error when it was in the lock part of the calling method.
    private async Task WriteToFileAsync (string logMessage){
        try {
            using var streamWriter = new StreamWriter(_logFilePath, append: true);
            await streamWriter.WriteLineAsync(logMessage);
        } catch (Exception ex) {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }
}