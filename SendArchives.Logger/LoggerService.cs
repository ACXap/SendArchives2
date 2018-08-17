using NLog;
using System;
using System.Runtime.CompilerServices;

namespace SendArchives.Logger
{
    public class LoggerService : ILoggerService
    {
        private NLog.Logger GetInnerLogger(string sourceFilePath)
        {
            return sourceFilePath == null ? LogManager.GetCurrentClassLogger() : LogManager.GetLogger(sourceFilePath);
        }

        public void Debug(string message, [CallerFilePath] string sourceFilePath = null)
        {
            GetInnerLogger(sourceFilePath).Debug(message);
        }
        public void Info(string message, [CallerFilePath] string sourceFilePath = null)
        {
            GetInnerLogger(sourceFilePath).Info(message);
        }
        public void Warn(string message, Exception ex, [CallerFilePath] string sourceFilePath = null)
        {
            GetInnerLogger(sourceFilePath).Warn(ex, message);
        }
        public void Error(string message, Exception ex, [CallerFilePath] string sourceFilePath = null)
        {
            GetInnerLogger(sourceFilePath).Error(ex, message);
        }

        public LoggerService() ///TODO add variable level of logging
        {
            var config = new NLog.Config.LoggingConfiguration();

            var logfile = new NLog.Targets.FileTarget() { FileName = @"Logs\\log.txt", Name = "logfile" };
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Debug, logfile));

            LogManager.Configuration = config;
        }
    }
}