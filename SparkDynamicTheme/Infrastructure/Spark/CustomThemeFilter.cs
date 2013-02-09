using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Spark.Web.Mvc.Descriptors;

namespace SparkDynamicTheme.Infrastructure.Spark
{
	public abstract class CustomThemeFilter : DescriptorFilterBase
	{
		private const string CustomerKey = "customerId";

		public override IEnumerable<string> PotentialLocations(IEnumerable<string> locations, IDictionary<string, object> extra)
		{
			var isMaster = locations.Any(x => x.Contains("Application.spark"));

			if (!isMaster)
			{
				//we only care about the setting the theme locations for master layouts.
				return locations;
			}

			if (extra.ContainsKey(CustomerKey))
			{
				var customerId = extra[CustomerKey] as string;
				if (customerId != null)
				{
					const string masterName = "index.html";
					var newLocations = new List<string>
						                   {
							                   Path.Combine(customerId, masterName)
						                   };
					locations = newLocations.Concat(locations);
				}
			}

			return locations;
		}

		public static CustomThemeFilter For(Func<ControllerContext, object> selector)
		{
			return new Delegated(selector);
		}

		class Delegated : CustomThemeFilter
		{
			private readonly Func<ControllerContext, object> _selector;

			public Delegated(Func<ControllerContext, object> selector)
			{
				_selector = selector;
			}

			public override void ExtraParameters(ControllerContext context, IDictionary<string, object> extra)
			{
				var customerId = _selector(context) as string;
				if (!string.IsNullOrEmpty(customerId))
				{
					extra[CustomerKey] = customerId;
				}
			}
		}
	}
}