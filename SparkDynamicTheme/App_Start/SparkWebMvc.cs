using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Spark;
using Spark.FileSystem;
using Spark.Web.Mvc;
using Spark.Web.Mvc.Descriptors;
using SparkDynamicTheme.Infrastructure.Spark;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SparkDynamicTheme.App_Start.SparkWebMvc), "Start")]

namespace SparkDynamicTheme.App_Start
{
	public static class SparkWebMvc
	{
		public static void Start()
		{			
			ViewEngines.Engines.Clear();

			var settings = new SparkSettings()
				.AddViewFolder(ViewFolderType.FileSystem, new Dictionary<string, string>
					                                          {
								 								  //{"basePath", "D:\\code\\SparkDynamicTheme\\SparkDynamicTheme\\CustomerThemes\\"}
						                                          {"basePath", AppDomain.CurrentDomain.GetData("DataDirectory").ToString()}
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

	}
}
