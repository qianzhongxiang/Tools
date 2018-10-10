using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DONN.Tools.Logger
{
    public static class LogHelper
    {
        private readonly static string _mainDirectory = PathHelper.GetCurrentDirectory();


        private static ILog logger = GenerateLogger("cpp4wcf");

        public static void Log(string msg)
        {
            logger.Info(msg);
        }

        public static void Error(string msg)
        {
            logger.Error(msg);
        }
        public static void Error(string msg, Exception e)
        {
            logger.Error(msg, e);
        }

        private static ILog GenerateLogger(string instanceName)
        {
            var folderPath = Environment.CurrentDirectory;
            PatternLayout layout = new PatternLayout(@"-------------------------------------------------------------------------------%newline
%date{ MM / dd / yyyy HH:mm: ss,fff}[%thread] %-5level %logger %ndc – %message%newline");
            string repositoryName = string.Format("{0}Repository", instanceName);
            ILoggerRepository repository = LoggerManager.CreateRepository(repositoryName);
            string loggerName = string.Format("{0}Logger", instanceName);
            BasicConfigurator.Configure(repository, CreateAppender(Level.Info, layout, instanceName), CreateAppender(Level.Error, layout, instanceName));
            ILog logger = LogManager.GetLogger(repositoryName, loggerName);
            return logger;
        }
        private static RollingFileAppender CreateAppender(Level level, PatternLayout layout, string instanceName)
        {
            DenyAllFilter filterD = new DenyAllFilter();
            filterD.ActivateOptions();
            LevelMatchFilter filter = new LevelMatchFilter();
            filter.AcceptOnMatch = true;
            filter.LevelToMatch = level;
            filter.ActivateOptions();
            RollingFileAppender appender = new RollingFileAppender();
            appender.File = $"{_mainDirectory}\\logs\\";
            appender.ImmediateFlush = true;
            appender.Encoding = Encoding.UTF8;
            appender.StaticLogFileName = false;
            appender.AppendToFile = true;
            appender.RollingStyle = RollingFileAppender.RollingMode.Date;
            appender.DatePattern = $"dd.MM.yyyy\\\\'{level.Name}.log'";
            appender.LockingModel = new FileAppender.MinimalLock();
            appender.Name = $"{instanceName}Appender_{level.Name}";
            appender.AddFilter(filter);
            appender.AddFilter(filterD);
            appender.Layout = layout;
            appender.ActivateOptions();
            return appender;
        }
        public static void Close()
        {
            if (logger != null)
            {
                var appenders = logger.Logger.Repository.GetAppenders();
                foreach (var item in appenders)
                {
                    item.Close();
                }
                logger.Logger.Repository.Shutdown();
            }
        }
    }
}
