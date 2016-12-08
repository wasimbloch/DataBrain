using NLog;
using NLog.Config;
using System;
using System.IO;

namespace DataBrain.Core.Logging
{
    public static class LogSetup
    {
        private static bool hasRun;

        public static void Run()
        {
            if (!hasRun)
            {
                var environment = Config.Get("Environment");
                var configFileName = string.Format("nlog-{0}.config", environment);
                var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileName);
                if (File.Exists(configFilePath))
                {
                    LogManager.Configuration = new XmlLoggingConfiguration(configFilePath, true);
                }
                hasRun = true;
            }
        }
    }
}
