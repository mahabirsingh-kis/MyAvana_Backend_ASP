using System;
using System.Collections.Generic;
using System.Text;

namespace MyAvana.Logger.Contract
{
    public interface ILogger
    {
        void LogError(string error, Exception exception);
        void LogError(string error);
    }
}
