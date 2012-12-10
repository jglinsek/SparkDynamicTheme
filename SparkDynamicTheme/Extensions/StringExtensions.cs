using System;

namespace SparkDynamicTheme.Extensions
{
	public static class StringExtensions
	{
		public static string FormatWith(this string value, params object[] args)
		{
			return String.Format(value, args);
		}

		public static bool HasValue(this string value)
		{
			if (string.IsNullOrEmpty(value)) return false;
			return (value.Trim().Length > 0);
		}
	}
}