using System.Collections.Generic;
using System.Reflection;
using Spark;
using Spark.FileSystem;
using Spark.Web.Mvc;
using SparkDynamicTheme.Controllers;

namespace PreCompileService
{
	public class SparkStarter
	{
		public SparkViewFactory Configure()
		{
			var assemblyWithViews = Assembly.GetAssembly(typeof(HomeController));

			var settings = new SparkSettings()
				.AddAssembly(assemblyWithViews)
				.AddViewFolder(typeof(VirtualFileSystemViewFolder), new Dictionary<string, string>
					                                          {
						                                          {"basePath", FilePaths.ViewsFolder.Replace("\\Views", string.Empty)}
					                                          })
				.AddViewFolder(ViewFolderType.FileSystem, new Dictionary<string, string>
					                                          {
						                                          {"basePath", FilePaths.ViewsFolder}
					                                          })
				.AddViewFolder(ViewFolderType.FileSystem, new Dictionary<string, string>
					                                          {
						                                          {"basePath", FilePaths.ThemesFolder}
					                                          })
				.SetAutomaticEncoding(true);


			foreach (var assemblyName in assemblyWithViews.GetReferencedAssemblies())
			{
				settings.AddAssembly(assemblyName.FullName);
			}

			return new SparkViewFactory(settings);
		}
		 
	}
}