using System;
using System.Runtime.CompilerServices;

namespace SendArchives.Logger
{
    public interface ILoggerService
    {
        void Debug(string message, [CallerFilePath] string sourceFilePath = null);
        void Info(string message, [CallerFilePath] string sourceFilePath = null);
        void Warn(string message, Exception ex, [CallerFilePath] string sourceFilePath = null);
        void Error(string message, Exception ex, [CallerFilePath] string sourceFilePath = null);
    }
}