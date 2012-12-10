using SparkDynamicTheme.Extensions;

namespace SparkDynamicTheme.Services.Themes
{
	public class ThemeResolver
	{
		public static string GetThemeMasterPage(string masterName)
		{
			return "~\\CustomerThemes\\{0}\\index.html".FormatWith(masterName);
		}
	}
}