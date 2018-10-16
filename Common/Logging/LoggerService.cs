using Common.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Logging
{
    public class LoggerService : ILoggerService
    {
        ILogger loger;

        public LoggerService()
        {
            loger = new LoggerConfiguration()
                    .WriteTo.Seq("http://logs.fitness-pro.ru:5341")
                    .CreateLogger();
        }

        public LoggerService(Uri seqURl)
        {
            loger = new LoggerConfiguration()
                    .WriteTo.Seq(seqURl.AbsolutePath)
                    .CreateLogger();
        }

        public void Debug(string message)
        {
            loger.Debug(message);
        }

        public void Debug(Exception ex, string message)
        {
            loger.Debug(ex, message);
        }

        public void Debug(string message, params object[] @params)
        {
            loger.Debug(message, @params);
        }

        public void Debug(Exception ex, string message, params object[] @params)
        {
            loger.Debug(ex, message, @params);
        }

        public void Error(string message)
        {
            loger.Error(message);
        }

        public void Error(Exception ex, string message)
        {
            loger.Error(ex, message);
        }

        public void Error(string message, params object[] @params)
        {
            loger.Error(message, @params);
        }

        public void Error(Exception ex, string message, params object[] @params)
        {
            loger.Error(ex, message, @params);
        }

        public void Information(string message)
        {
            loger.Information(message);
        }

        public void Information(string message, params object[] @params)
        {
            loger.Information(message, @params);
        }

        public void Warning(string message)
        {
            loger.Warning(message);
        }

        public void Warning(Exception ex, string message)
        {
            loger.Warning(ex, message);
        }

        public void Warning(string message, params object[] @params)
        {
            loger.Warning(message, @params);
        }

        public void Warning(Exception ex, string message, params object[] @params)
        {
            loger.Warning(ex, message, @params);
        }
    }
}
