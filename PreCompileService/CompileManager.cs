using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PreCompileService
{
	public class CompileManager
	{

		public void CompileThemes()
		{
			var themeMasterFiles = GetThemeMasterFiles().ToList();
			this.Log().Info("Begin compilation of {0} themes.", themeMasterFiles.Count());
			var sparkViewFactory = new SparkStarter().Configure();

			foreach (var masterFile in themeMasterFiles)
			{
				this.Log().Debug("Begin compile for {0}", masterFile);

				/*
				 * Memory use goes up with each compile and doesn't get released until the AppDomain is unloaded.
				 * So, you can create a temporary AppDomain, do the compile within it and then release it as a workaround.
				 * In this scenario, you'd have to do that anyways to release the handle on the newly created dll, 
				 * but in a web host scenario, it would be nice if the memory got released so your AppPool doesn't consume tons of memory.
				 */
				try
				{
					var compiler = new SparkCompiler(sparkViewFactory, masterFile);
					compiler.Compile();
					this.Log().Info("Compile complete for {0}", masterFile);
				}
				catch (Exception ex)
				{
					this.Log().Error("Compile failed for {0}", masterFile);
					this.Log().Error(ex.ToString());
				}

				this.Log().Debug("End compile");
			}

			this.Log().Info("End compilation.");
		}

		private IEnumerable<string> GetThemeMasterFiles()
		{
			return Directory.GetFiles(FilePaths.ThemesFolder, "index.html", SearchOption.AllDirectories);

		}
	}
}