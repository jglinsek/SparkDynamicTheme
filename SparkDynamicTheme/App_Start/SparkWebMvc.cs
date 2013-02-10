using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Spark;
using Spark.FileSystem;
using Spark.Web.Mvc;
using Spark.Web.Mvc.Descriptors;
using SparkDynamicTheme.Controllers;
using SparkDynamicTheme.Infrastructure.Spark;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SparkDynamicTheme.App_Start.SparkWebMvc), "Start")]

namespace SparkDynamicTheme.App_Start
{
	public static class SparkWebMvc
	{
		private static readonly string ThemeDirectory = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

		public static void Start()
		{			
			ViewEngines.Engines.Clear();

			var settings = new SparkSettings()
				.AddViewFolder(ViewFolderType.FileSystem, new Dictionary<string, string>
					                                          {
						                                          {"basePath", ThemeDirectory}
					                                          })
				.SetAutomaticEncoding(true);

			if (Debugger.IsAttached)
				settings.SetDebug(true);

			var viewFactory = new SparkViewFactory(settings);

			var sparkServiceContainer = new SparkServiceContainer(settings);
			sparkServiceContainer.SetServiceBuilder<IViewEngine>(c => viewFactory);
			sparkServiceContainer.SetServiceBuilder<IDescriptorBuilder>(c => new DefaultDescriptorBuilder());
			sparkServiceContainer.SetServiceBuilder<ICacheServiceProvider>(c => new DefaultCacheServiceProvider());
			sparkServiceContainer.AddFilter(new AreaDescriptorFilter());
			sparkServiceContainer.AddFilter(CustomThemeFilter.For(GetCustomerId));

			ViewEngines.Engines.Add(sparkServiceContainer.GetService<IViewEngine>());

			var sparkEngine = sparkServiceContainer.GetService<ISparkViewEngine>();
			
			var timer = new Stopwatch();
			timer.Start();
			sparkEngine.BatchCompilation(AllKnownDescriptors(viewFactory));
			timer.Stop();

			"SparkWebMvc".Log().Info("Pre-compile time: {0} seconds", timer.Elapsed.TotalSeconds);
		}

		private static object GetCustomerId(ControllerContext controller)
		{ 
			var routeValues = controller.RouteData.Values;
			string customerId = null;
			if (routeValues.ContainsKey("id"))
			{
				customerId = routeValues["id"].ToString();
			}

			return customerId;
		}

		private static IList<SparkViewDescriptor> AllKnownDescriptors(SparkViewFactory viewFactory)
		{
			//build the batch
			var batch = new SparkBatchDescriptor(); 
			batch
				.For<HomeController>().Layout("Application");


			//find all the custom themes
			var themeMasters = new List<string>();

			var themePath = ThemeDirectory + "\\";
			var themedMasterFiles = Directory.GetFiles(themePath, "index.html", SearchOption.AllDirectories);
			foreach (var master in themedMasterFiles)
			{
				var themeMasterName = master.Remove(0, themePath.Length);
				themeMasters.Add(themeMasterName);
			}

			//adjust the batch to precompile for each custom theme
			var generatedDescriptors = viewFactory.CreateDescriptors(batch);
			var allDescriptors = new List<SparkViewDescriptor>();

			foreach (var descriptor in generatedDescriptors)
			{
				allDescriptors.Add(descriptor);

				var isAppMaster = descriptor.Templates.Any(x => x.Contains("Application"));
				if (!isAppMaster) continue;

				foreach (var themeMaster in themeMasters)
				{
					var themeDescriptor = new SparkViewDescriptor()
						.SetLanguage(descriptor.Language)
						.SetTargetNamespace(descriptor.TargetNamespace)
						.AddTemplate(descriptor.Templates[0]);

					themeDescriptor.AddTemplate(themeMaster);

					allDescriptors.Add(themeDescriptor);
				}
			}

			return allDescriptors;
		}

	}
}
