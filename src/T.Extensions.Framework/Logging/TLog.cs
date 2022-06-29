using NLog;
using System;

namespace T.Extensions.Framework.Logging
{
    /// <summary>
    /// 基于NLog的日志扩展类
    /// </summary>
    public static class TLog
    {
        private static readonly Logger logger;

        static TLog()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Formats and writes a trace log message.
        /// </summary>
        /// <param name="message"></param>
        public static void LogTrace(string message)
        {
            logger.Log(LogLevel.Trace, message);
        }

        /// <summary>
        /// Formats and writes a trace log message.
        /// </summary>
        /// <param name="exception"></param>
        public static void LogTrace(Exception exception)
        {
            logger.Log(LogLevel.Trace, exception);
        }

        /// <summary>
        /// Formats and writes a trace log message.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        public static void LogTrace(Exception exception, string message)
        {
            logger.Log(LogLevel.Trace, exception, message);
        }

        /// <summary>
        /// Formats and writes a debug log message.
        /// </summary>
        /// <param name="message"></param>
        public static void LogDebug(string message)
        {
            logger.Log(LogLevel.Debug, message);
        }

        /// <summary>
        /// Formats and writes a debug log message.
        /// </summary>
        /// <param name="exception"></param>
        public static void LogDebug(Exception exception)
        {
            logger.Log(LogLevel.Debug, exception);
        }

        /// <summary>
        /// Formats and writes a debug log message.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        public static void LogDebug(Exception exception, string message)
        {
            logger.Log(LogLevel.Debug, exception, message);
        }

        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        /// <param name="message"></param>
        public static void LogInformation(string message)
        {
            logger.Log(LogLevel.Info, message);
        }

        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        /// <param name="exception"></param>
        public static void LogInformation(Exception exception)
        {
            logger.Log(LogLevel.Info, exception);
        }

        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        public static void LogInformation(Exception exception, string message)
        {
            logger.Log(LogLevel.Info, exception, message);
        }

        /// <summary>
        /// Formats and writes a warning log message.
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(string message)
        {
            logger.Log(LogLevel.Warn, message);
        }

        /// <summary>
        /// Formats and writes a warning log message.
        /// </summary>
        /// <param name="exception"></param>
        public static void LogWarning(Exception exception)
        {
            logger.Log(LogLevel.Warn, exception);
        }

        /// <summary>
        /// Formats and writes a warning log message.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        public static void LogWarning(Exception exception, string message)
        {
            logger.Log(LogLevel.Warn, exception, message);
        }

        /// <summary>
        /// Formats and writes an error log message.
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(string message)
        {
            logger.Log(LogLevel.Error, message);
        }

        /// <summary>
        /// Formats and writes an error log message.
        /// </summary>
        /// <param name="exception"></param>
        public static void LogError(Exception exception)
        {
            logger.Log(LogLevel.Error, exception);
        }

        /// <summary>
        /// Formats and writes an error log message.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        public static void LogError(Exception exception, string message)
        {
            logger.Log(LogLevel.Error, exception, message);
        }

        /// <summary>
        /// Formats and writes a critical log message.
        /// </summary>
        /// <param name="message"></param>
        public static void LogCritical(string message)
        {
            logger.Log(LogLevel.Fatal, message);
        }

        /// <summary>
        /// Formats and writes a critical log message.
        /// </summary>
        /// <param name="exception"></param>
        public static void LogCritical(Exception exception)
        {
            logger.Log(LogLevel.Fatal, exception);
        }

        /// <summary>
        /// Formats and writes a critical log message.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        public static void LogCritical(Exception exception, string message)
        {
            logger.Log(LogLevel.Fatal, exception, message);
        }

        /// <summary>
        /// Formats and writes a log message at the specified log level.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public static void Log(LogLevel level, string message)
        {
            logger.Log(level, message);
        }

        /// <summary>
        /// Formats and writes a log message at the specified log level.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="exception"></param>
        public static void Log(LogLevel level, Exception exception)
        {
            logger.Log(level, exception);
        }

        /// <summary>
        /// Formats and writes a log message at the specified log level.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        public static void Log(LogLevel level, Exception exception, string message)
        {
            logger.Log(level, exception, message);
        }
    }
}
