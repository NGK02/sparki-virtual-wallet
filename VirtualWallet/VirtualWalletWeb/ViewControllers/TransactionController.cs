using Microsoft.AspNetCore.Mvc;

namespace VirtualWallet.Web.ViewControllers
{
	public class TransactionController : Controller
	{
		public IActionResult CreateTransfer()
		{
			return View();
		}
	}
}
