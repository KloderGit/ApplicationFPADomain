using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface ILoggerService
    {
        void Debug(string message);
        void Debug(Exception ex, string message);
        void Debug(string message, params object[] @params);
        void Debug(Exception ex, string message, params object[] @params);

        void Information(string message);
        void Information(string message, params object[] @params);

        void Warning(string message);
        void Warning(Exception ex, string message);
        void Warning(string message, params object[] @params);
        void Warning(Exception ex, string message, params object[] @params);

        void Error(string message);
        void Error(Exception ex, string message);
        void Error(string message, params object[] @params);
        void Error(Exception ex, string message, params object[] @params);
    }
}
