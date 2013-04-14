using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Spark;
using Spark.Web.Mvc;

namespace PreCompileService
{
	internal class SparkCompiler
	{
		private const string ApplicationLayout = "Application";

		private readonly SparkViewFactory _sparkViewFactory;
		private readonly string _indexFileName;
		private readonly string _dllCachePath;
		private readonly string _themePath;

		public SparkCompiler(SparkViewFactory sparkViewFactory, string indexFileName)
		{
			_sparkViewFactory = sparkViewFactory;
			_indexFileName = indexFileName;
			_dllCachePath = FilePaths.StorageFolder;
			_themePath = FilePaths.ThemesFolder;
		}

		public Assembly Compile()
		{
			if (!ThemeExists()) return null;

			var sparkViewDescriptors = ThemableDescriptors(_sparkViewFactory);

			var timer = new Stopwatch();
			timer.Start();
			var assembly = _sparkViewFactory.Engine.BatchCompilation(DllPath, sparkViewDescriptors);
			timer.Stop();

			this.Log().Debug("Precompile Time: {0}m {1}s, {2} total seconds", timer.Elapsed.Minutes, timer.Elapsed.Seconds, timer.Elapsed.TotalSeconds);
			return assembly;
		}

		private bool ThemeExists()
		{
			string themePath = FilePaths.ThemesFolder;
			var masterPageFile = Path.Combine(themePath, MasterPage);
			return File.Exists(masterPageFile);
		}

		private string MasterPage
		{
			get { return _indexFileName.Replace(_themePath, ""); }
		}

		private string DllPath
		{
			get
			{
				var fileName = MasterPage
					.Replace(Path.DirectorySeparatorChar + "index.html", "")
					.Replace(Path.DirectorySeparatorChar, '-');
				return Path.Combine(_dllCachePath, fileName + ".dll");
			}
		}

		private IList<SparkViewDescriptor> ThemableDescriptors(SparkViewFactory viewFactory)
		{
			var generatedDescriptors = RootThemableDescriptors(viewFactory);
			var allDescriptors = new List<SparkViewDescriptor>();

			foreach (var descriptor in generatedDescriptors)
			{
				var isAppMaster = descriptor.Templates.Any(x => x.Contains(ApplicationLayout));
				if (!isAppMaster) continue;

				var themeDescriptor = new SparkViewDescriptor()
					.SetLanguage(descriptor.Language)
					.SetTargetNamespace(descriptor.TargetNamespace)
					.AddTemplate(descriptor.Templates[0]);

				themeDescriptor.AddTemplate(MasterPage);

				allDescriptors.Add(themeDescriptor);
			}

			return allDescriptors;
		}


		private static IEnumerable<SparkViewDescriptor> RootThemableDescriptors(SparkViewFactory viewFactory)
		{
			var batch = new SparkBatchDescriptor();
			batch
				//Guest Interface
				.For<SparkDynamicTheme.Controllers.HomeController>().Layout(ApplicationLayout)
				.For<SparkDynamicTheme.Controllers.FirstController>().Layout(ApplicationLayout)
				.For<SparkDynamicTheme.Controllers.SecondController>().Layout(ApplicationLayout)
				.For<SparkDynamicTheme.Controllers.ThirdController>().Layout(ApplicationLayout)
				.For<SparkDynamicTheme.Controllers.FourthController>().Layout(ApplicationLayout)
				.For<SparkDynamicTheme.Controllers.FifthController>().Layout(ApplicationLayout)
				.For<SparkDynamicTheme.Controllers.SixthController>().Layout(ApplicationLayout)
				.For<SparkDynamicTheme.Controllers.SeventhController>().Layout(ApplicationLayout)
				.For<SparkDynamicTheme.Controllers.EigthController>().Layout(ApplicationLayout)
				.For<SparkDynamicTheme.Controllers.NinthController>().Layout(ApplicationLayout)
				.For<SparkDynamicTheme.Controllers.TenthController>().Layout(ApplicationLayout);

			return viewFactory.CreateDescriptors(batch);
		} 


	}
}