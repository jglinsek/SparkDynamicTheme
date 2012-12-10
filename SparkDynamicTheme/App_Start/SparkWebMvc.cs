using System.Web.Mvc;
using Spark;
using Spark.Web.Mvc;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SparkDynamicTheme.App_Start.SparkWebMvc), "Start")]

namespace SparkDynamicTheme.App_Start
{
	using System.Collections.Generic;
	using System.Reflection;

	using Spark.FileSystem;
	using Spark.Web.Mvc.Descriptors;

	using SparkDynamicTheme.Infrastructure.Spark;

	public static class SparkWebMvc
	{
		public static void Start()
		{
			ViewEngines.Engines.Clear();

			var settings = new SparkSettings()
										.AddAssembly(Assembly.GetAssembly(typeof(ActionResult)))
										.AddViewFolder(ViewFolderType.FileSystem, new Dictionary<string, string>
																													{
																															{"basePath","~\\CustomerThemes"}
																													})
										.SetAutomaticEncoding(true);

			var viewFactory = new SparkViewFactory(settings);

			var sparkServiceContainer = new SparkServiceContainer(settings);
			sparkServiceContainer.SetServiceBuilder<IViewEngine>(c => viewFactory);
			sparkServiceContainer.SetServiceBuilder<IDescriptorBuilder>(c => new ThemeDescriptorBuilder());
			sparkServiceContainer.SetServiceBuilder<ICacheServiceProvider>(c => new DefaultCacheServiceProvider());
			sparkServiceContainer.AddFilter(new AreaDescriptorFilter());

			ViewEngines.Engines.Add(sparkServiceContainer.GetService<IViewEngine>());
		}
	}
}
