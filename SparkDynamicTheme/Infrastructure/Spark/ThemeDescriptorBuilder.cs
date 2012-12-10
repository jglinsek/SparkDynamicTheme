using System.Collections.Generic;
using Spark.Web.Mvc;
using SparkDynamicTheme.Services.Themes;

namespace SparkDynamicTheme.Infrastructure.Spark
{
	public class ThemeDescriptorBuilder : DefaultDescriptorBuilder
	{
		protected override IEnumerable<string> PotentialMasterLocations(string masterName, IDictionary<string, object> extra)
		{
			var locations = new List<string>();
			locations.AddRange(base.PotentialMasterLocations(masterName, extra));
			locations.AddRange(new[]
																	 {
																			 ThemeResolver.GetThemeMasterPage(masterName)
																	 });
			return locations;
		}
	}
}