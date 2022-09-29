using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Logger.Services
{
    public class NLogServices : Contract.ILogger
    {
        /// <summary>
        /// NLog instance 
        /// </summary>
        private static NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        public NLogServices()
        {
        }


        public void LogError(string error, Exception exception)
        {
            _logger.LogException(level: LogLevel.Error, message: error, exception: exception);
        }

        public void LogError(string error)
        {
            _logger.LogException(level: LogLevel.Error, message: error, exception: null);
        }
    }
}
