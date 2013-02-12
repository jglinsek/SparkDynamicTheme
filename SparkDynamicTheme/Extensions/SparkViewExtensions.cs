using System.Collections.Generic;
using System.Web.Routing;
using Spark.Web.Mvc;

namespace SparkDynamicTheme.Extensions
{

	public static class SparkViewExtensions
	{
		public static bool IsLocal(this SparkView page)
		{
			return page.Request.IsLocal;
		}

		public static string Anchor(this SparkView page, string linkText, string actionName, string controllerName, IDictionary<string, object> htmlAttributes)
		{
			return Anchor(page, linkText, actionName, controllerName, null, htmlAttributes);
		}

		public static string Anchor(this SparkView page, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
		{
			return Anchor(page, linkText, actionName, controllerName, routeValues, htmlAttributes, false);
		}

		public static string Anchor(this SparkView page, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes, bool shouldEncodeLinkText)
		{
			return "<a href='/home'>Link!</a>";
		}

	}
}