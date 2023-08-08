using Microsoft.AspNetCore.Mvc;

namespace VirtualWallet.Web.ViewControllers
{
    public class DashboardController : Controller
    {
        public IActionResult index()
        {
            return View("DashboardMenu");
        }
    }
}
