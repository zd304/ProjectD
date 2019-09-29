using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using System.IO;

namespace LobbyServer
{
    public class Debug
    {
        private static ILogger log = null;

        public static void Initialize(GameApplication application)
        {
            log = LogManager.GetCurrentClassLogger();

            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(application.ApplicationRootPath, "log");

            string configPath = Path.Combine(application.BinaryPath, "log4net.config");
            FileInfo configFileInfo = new FileInfo(configPath);
            if (configFileInfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance); // 让Photon知道使用的是哪个日志插件（log4net）
                XmlConfigurator.ConfigureAndWatch(configFileInfo); // 让log4net读取配置文件
            }

            Log("Debug系统初始化完成！");
        }

        public static void Uninitialize()
        {
            log = null;
        }

        public static void Log(object message)
        {
            log.Info(message);
        }

        public static void LogFormat(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        public static void LogError(object message)
        {
            log.Error(message);
        }

        public static void LogErrorFormat(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        public static void LogWarning(object message)
        {
            log.Warn(message);
        }

        public static void LogWarningFormat(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }
    }
}
