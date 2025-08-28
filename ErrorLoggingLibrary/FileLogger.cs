using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

internal class FileLogger {
    private readonly Lock _fileLock = new();

    public void Log<TState> (LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter){
        ArgumentNullException.ThrowIfNull(formatter);

        var logMessage = $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} [{logLevel}] {formatter(state, exception)}";

        Task.Run(() => {
            lock (_fileLock){
                LogToFile(logMessage);
            }
        });
    }

    private static void LogToFile (string logMessage){
        try {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "logfile.txt");
            File.AppendAllText(filePath, logMessage + Environment.NewLine);
        } catch (Exception ex) {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }
}