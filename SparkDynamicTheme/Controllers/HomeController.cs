using System.Web.Mvc;

namespace SparkDynamicTheme.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index(string id)
		{
			return View();
		}
	}
}
