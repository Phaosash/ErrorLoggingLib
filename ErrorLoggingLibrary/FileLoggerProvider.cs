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

internal class FileLoggerProvider (string filePath, LogLevel logLevel = LogLevel.Information) : ILoggerProvider {
    private readonly string _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    private readonly LogLevel _logLevel = logLevel;

    //  This method creates and returns a new instance of FileLogger. It takes a categoryName
    //  as input, which can be used for categorizing logs, though it is not used in this
    //  implementation. The method uses the _filePath and _logLevel fields to configure the
    //  logger to write logs to the specified file at or above the given log level. The
    //  logger created by this method is responsible for logging messages to the file.
    public ILogger CreateLogger (string categoryName){
        return new FileLogger(_filePath, _logLevel);
    }

    //  This method is part of the IDisposable interface, and in this case, it is an empty
    //  implementation. Its purpose is to release any resources held by the object when it
    //  is no longer needed. Since this method is empty, it indicates that the FileLogger
    //  or the class implementing this method does not have any unmanaged resources (such
    //  as file handles, database connections, etc.) that need to be explicitly cleaned up.
    //  However, it provides a placeholder in case resource management becomes necessary
    //  in the future.
    public void Dispose() { }
}