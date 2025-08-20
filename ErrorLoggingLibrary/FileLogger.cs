using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

//  --------------------------------------------------------------------------------
//  Copyright (c) 2025 Michael McKIE
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:

//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.

//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
//  --------------------------------------------------------------------------------

internal class FileLogger (string filePath, LogLevel logLevel = LogLevel.Information) : ILogger {
    private readonly string _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    private readonly LogLevel _logLevel = logLevel;

    //  This method is an implementation of the ILogger interface that, in this case, returns null.
    //  It is intended to start a logging scope, which can be useful for associating a set of log entries
    //  with a particular context. Since the method returns null, it indicates that no special scope
    //  management is needed or used in this implementation. This is often used in simple logging scenarios
    //  where contextual information is not required.
    IDisposable? ILogger.BeginScope<TState>(TState state) => null;

    //  This method checks whether logging is enabled for a specified logLevel.
    //  It compares the given logLevel to the logger’s configured _logLevel.
    //  If the given logLevel is greater than or equal to the _logLevel, it returns
    //  true, indicating that logging for that level is enabled. Otherwise, it
    //  returns false, meaning logging is not enabled for that level. This method
    //  helps control which levels of logs (e.g., Information, Warning, Error) should
    //  be captured based on the configured threshold.
    public bool IsEnabled(LogLevel logLevel) => logLevel >= _logLevel;

    //  This method is responsible for logging messages at various log levels. It first checks if logging is
    //  enabled for the specified logLevel by calling the IsEnabled method. If logging is not enabled for that
    //  level, the method returns early and does nothing. If logging is enabled, it then formats the log entry
    //  using the provided formatter, which combines the state (the data related to the log) and the exception
    //  (if any) into a formatted string. The log message is then timestamped with the current UTC time and
    //  prefixed with the appropriate log level (e.g., Information, Warning, Error). Finally, the log entry is
    //  passed to the `WriteToFile` method to be saved to the log file. This method ensures that logs are written
    //  in a structured and timestamped manner.
    public void Log<TState> (LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter){
        if (!IsEnabled(logLevel)){
            return;
        }

        var logMessage = formatter(state, exception);
        var logEntry = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} [{logLevel}] {logMessage}";

        WriteToFile(logEntry);
    }

    //  This method attempts to write a log entry to a file. First, it tries to determine the root
    //  directory of the project by navigating up the directory structure using
    //  Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.FullName. If it successfully finds the
    //  root directory, it constructs the path to the log.txt file. If the root directory cannot be determined
    //  (e.g., if the application is running from a different location), it sets a fallback directory (C:\Documents)
    //  as the location for the log file.Once the appropriate directory is determined, the method attempts to append
    //  the logEntry to the log.txt file. If any errors occur during this process (e.g., file access issues), an
    //  exception is caught, and an error message is printed to the console.
    private static void WriteToFile (string logEntry){
        try {
            var rootDirectory = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.FullName;

            if (rootDirectory != null){
                var logFilePath = Path.Combine(rootDirectory, "log.txt");
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }  else {
                rootDirectory = @"C:\Documents";
                var logFilePath = Path.Combine(rootDirectory, "log.txt");
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }
        } catch (Exception ex) {
            Console.Error.WriteLine($"Failed to write log to file: {ex.Message}");
        }
    }
}