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