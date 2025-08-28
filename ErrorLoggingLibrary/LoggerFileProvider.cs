using Microsoft.Extensions.Logging;

namespace ErrorLoggingLibrary;

public class LoggerFileProvider : ILoggerProvider {
    private readonly string _filePath;

    public LoggerFileProvider(string filePath){
        _filePath = filePath;

        if (!File.Exists(_filePath)){
            using (File.Create(_filePath)){ }
        }
    }

    public ILogger CreateLogger (string categoryName){
        return new LoggerFile(_filePath);
    }

    public void Dispose (){
        GC.SuppressFinalize(this);
    }
}