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
			var config = new LoggingConfiguration();
			var console = new DebuggerTarget();
			config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, console));

			LogManager.ThrowExceptions = true;
			LogManager.Configuration = config;

			LoggingExtensions.Logging.Log.InitializeWith<LoggingExtensions.NLog.NLogLog>();
		}
	}
}