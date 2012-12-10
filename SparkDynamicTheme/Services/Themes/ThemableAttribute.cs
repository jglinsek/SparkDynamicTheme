using System;
using System.Web;
using System.Web.Mvc;
using SparkDynamicTheme.Extensions;

namespace SparkDynamicTheme.Services.Themes
{
	public class ThemableAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var actionResult = filterContext.Result as ViewResult;
			if (actionResult == null || !String.IsNullOrEmpty(actionResult.MasterName)) return;

			if(HttpContext.Current.Request["customer"].HasValue())
				actionResult.MasterName = HttpContext.Current.Request["customer"];
		}
	}
}