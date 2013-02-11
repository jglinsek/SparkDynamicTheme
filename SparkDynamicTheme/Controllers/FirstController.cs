using System.Web.Mvc;

namespace SparkDynamicTheme.Controllers
{
	public class FirstController : Controller
	{
		public ActionResult Index(string id)
		{
			return View();
		}
	}
}
