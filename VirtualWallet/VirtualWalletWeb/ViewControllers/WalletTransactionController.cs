using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Dto.ViewModels.CurrencyViewModels;
using VirtualWallet.Dto.ViewModels.WalletTransactionViewModels;
using VirtualWallet.Web.Helper;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.ViewControllers
{
	public class WalletTransactionController : Controller
	{
		private readonly IAuthManagerMvc authManagerMvc;
		private readonly ICurrencyService currencyService;
		private readonly IMapper mapper;

        public WalletTransactionController(IAuthManagerMvc authManagerMvc, ICurrencyService currencyService, IMapper mapper)
        {
            this.authManagerMvc = authManagerMvc;
            this.currencyService = currencyService;
            this.mapper = mapper;
        }

		[HttpGet]
        public IActionResult CreateWalletTransaction(CreateWalletTransactionViewModel walletTransactionForm)
		{
			try
			{
				if (!authManagerMvc.IsLogged("LoggedUser")) 
				{
                    return RedirectToAction("Login", "User");
                }

				//Да се изкара във хелпър?
				walletTransactionForm.Currencies = currencyService.GetCurrencies().Select(c => mapper.Map<CurrencyViewModel>(c)).ToList();

                return View(walletTransactionForm);
            }
			catch (Exception)
			{
				throw;
			}
		}

		[HttpPost]
		public IActionResult ConfirmWalletTransaction(CreateWalletTransactionViewModel walletTransactionForm)
		{
			try
			{
				if (!authManagerMvc.IsLogged("LoggedUser"))
				{
					return RedirectToAction("Login", "User");
				}

				if (!this.ModelState.IsValid)
				{
					//Да се изкара във хелпър?
					this.ViewData["ErrorMessage"] = (walletTransactionForm.Amount <= 0 ? "Please provide positive Amount!" : "Please provide input!");
					return View("CreateTransaction", walletTransactionForm);
				}

				return View(walletTransactionForm);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
