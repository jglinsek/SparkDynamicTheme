using Spark.Web.Mvc;

namespace SparkDynamicTheme.Extensions
{

	public static class SparkViewExtensions
	{
		public static bool IsLocal(this SparkView page)
		{
			return page.Request.IsLocal;
		}

	}
}