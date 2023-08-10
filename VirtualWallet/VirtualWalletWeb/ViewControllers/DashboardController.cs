using Microsoft.AspNetCore.Mvc;

namespace VirtualWallet.Web.ViewControllers
{
    public class DashboardController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("DashboardMain");
        }

		[HttpGet]
		public IActionResult Exchanges()
		{
			return View("Exchanges");
		}


	}
}
