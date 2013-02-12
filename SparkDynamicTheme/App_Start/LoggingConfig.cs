using NLog;
using NLog.Config;
using NLog.Targets;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SparkDynamicTheme.App_Start.LoggingConfig), "Configure")]

namespace SparkDynamicTheme.App_Start
{
	public static class LoggingConfig
	{
		public static void Configure()
		{
			if (Configured) return;

			var config = new LoggingConfiguration();
			var console = new DebuggerTarget();
			config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, console));

			LogManager.ThrowExceptions = true;
			LogManager.Configuration = config;

			var logger = LogManager.GetCurrentClassLogger();
			logger.Info("Logging Configured!");

			LoggingExtensions.Logging.Log.InitializeWith<LoggingExtensions.NLog.NLogLog>();

			Configured = true;
		}

		public static bool Configured { get; private set; }
	}
}