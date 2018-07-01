using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Logging definitions

namespace Utils
{
    // Log levels

    public enum LogLevel
    {
        Critical,
        Normal,
        Verbose
    }

    // Log message types

    public enum LogType
    {
        Info,
        Warning,
        Error
    }

    // Logger delegate definition

    public delegate void FnLog(LogType logType, LogLevel logLevel, string message, params Object[] args);

    // Logger helper class

    public class Logger
    {
        protected FnLog _logger;

        public Logger(FnLog logger)
        {
            _logger = logger;
        }

        public void Log(string message, params Object[] args)
        {
            if (_logger != null)
                _logger(LogType.Info, LogLevel.Normal, message, args);
        }

        public void Log(LogType logType, LogLevel logLevel, string message, params Object[] args)
        {
            if (_logger != null)
                _logger(logType, logLevel, message, args);
        }
    }
}
