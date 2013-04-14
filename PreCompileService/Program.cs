using System;

namespace PreCompileService
{
	class Program
	{
		static void Main(string[] args)
		{
			LoggingExtensions.Logging.Log.InitializeWith<LoggingExtensions.NLog.NLogLog>();
			FilePaths.EnsureFoldersExist();

			"Program".Log().Info("Staring. Check your Working Memory for this process. Press a key to begin.");
			Console.ReadKey();

			var compileManager = new CompileManager();
			compileManager.CompileThemes();

			"Program".Log().Info("Done! Check your Working Memory for this process. Press a key to exit.");
			Console.ReadKey();
		}
	}


}
