using System.Web.Mvc;
using SparkDynamicTheme.Services.Themes;

namespace SparkDynamicTheme.Controllers
{
	[Themable]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}
