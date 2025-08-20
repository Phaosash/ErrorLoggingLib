using Microsoft.Extensions.Logging;
using Serilog;

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
    
public class LoggingHandler {
    private readonly ILogger<LoggingHandler>? _logger;
    private static LoggingHandler? _instance;
    private static readonly Lock _lock = new();

    // This method is a constructor that accepts an ILogger<LoggingHandler> object as a parameter
    // and assigns it to a private field _logger. This allows the class to use the logger for
    // logging operations throughout its lifecycle.
    private LoggingHandler (ILogger<LoggingHandler> logger){
        _logger = logger;
    }

    // This is a thread-safe singleton implementation of the LoggingHandler class.
    // It checks if the _instance is null and, if so, acquires a lock to ensure only one thread
    // can create the instance. It then creates a logger using LoggerFactory, passing in a file logger,
    // and uses it to initialize the LoggingHandler. If the instance already exists, it simply
    // returns the existing _instance.
    public static LoggingHandler Instance {
        get {
            if (_instance == null){
                lock (_lock){
                    if (_instance == null){
                        Log.Logger = new LoggerConfiguration()
                            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                            .CreateLogger();

                        var loggerFactory = LoggerFactory.Create(builder => {
                            builder.AddSerilog();
                        });

                        var logger = loggerFactory.CreateLogger<LoggingHandler>();
                        _instance = new LoggingHandler(logger);
                    }
                }
            }

            return _instance;
        }
    }

    // This method logs an error message along with an exception using the _logger.
    // It takes two parameters: a string msg for the error message and an Exception ex
    // to capture the exception details. The method then formats the log entry with the
    // message and exception, and passes it to the logger to be recorded at the error level.
    public void LogError (string msg, Exception ex){
        _logger!.LogError("Error: {Message}, Exception: {Exception}", msg, ex);
    }

    // This method logs an informational message using the _logger.
    // It takes a single parameter, msg, which represents the message to be logged.
    // The method formats the log entry by prepending the message with "Information: "
    // and then passes it to the logger to be recorded at the information level.
    public void LogInformation (string msg){
        _logger!.LogInformation("Information: {Message}", msg);
    }

    // This method logs a warning message using the _logger.
    // It accepts a string parameter, msg, which represents the warning message.
    // The method formats the log entry by prefixing the message with "Warning: "
    // and then sends it to the logger to be recorded at the warning level.
    public void LogWarning (string msg){
        _logger!.LogWarning("Warning: {Message}", msg);
    }
}