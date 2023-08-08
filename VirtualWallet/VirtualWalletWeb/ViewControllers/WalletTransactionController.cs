using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.ExchangeDto;
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
		private readonly IWalletTransactionService walletTransactionService;

        public WalletTransactionController(IAuthManagerMvc authManagerMvc, ICurrencyService currencyService, IMapper mapper, IWalletTransactionService walletTransactionService)
        {
            this.authManagerMvc = authManagerMvc;
            this.currencyService = currencyService;
            this.mapper = mapper;
			this.walletTransactionService = walletTransactionService;
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
				ViewData["Currencies"] = currencyService.GetCurrencies().Select(c => mapper.Map<CurrencyViewModel>(c)).ToList();

                return View(walletTransactionForm);
            }
			catch (UnauthenticatedOperationException ex)
			{
				Response.StatusCode = StatusCodes.Status401Unauthorized;
				ViewData["ErrorMessage"] = ex.Message;

				return View("Error");
			}
			catch (UnauthorizedOperationException ex)
			{
				Response.StatusCode = StatusCodes.Status403Forbidden;
				ViewData["ErrorMessage"] = ex.Message;

				return View("Error");
			}
			catch (EntityNotFoundException ex)
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
				ViewData["ErrorMessage"] = ex.Message;

				return View("Error");
			}
		}

		[HttpGet]
		public IActionResult ConfirmWalletTransaction(CreateWalletTransactionViewModel walletTransactionForm)
		{
			try
			{
				if (!authManagerMvc.IsLogged("LoggedUser"))
				{
					return RedirectToAction("Login", "User");
				}

				ViewData["Currencies"] = currencyService.GetCurrencies().Select(c => mapper.Map<CurrencyViewModel>(c)).ToList();

				if (!this.ModelState.IsValid)
				{
					//Да се изкара във хелпър?
					return View("CreateTransaction", walletTransactionForm);
				}

				return View(walletTransactionForm);
			}
			catch (UnauthenticatedOperationException ex)
			{
				Response.StatusCode = StatusCodes.Status401Unauthorized;
				ViewData["ErrorMessage"] = ex.Message;

				return View("Error");
			}
			catch (UnauthorizedOperationException ex)
			{
				Response.StatusCode = StatusCodes.Status403Forbidden;
				ViewData["ErrorMessage"] = ex.Message;

				return View("Error");
			}
			catch (EntityNotFoundException ex)
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
				ViewData["ErrorMessage"] = ex.Message;

				return View("Error");
			}
		}

		[HttpPost]
		public IActionResult FinalizeWalletTransaction(CreateWalletTransactionViewModel walletTransactionForm)
		{
			try
			{
				if (!authManagerMvc.IsLogged("LoggedUser"))
				{
					return RedirectToAction("Login", "User");
				}

				var loggedUserId = this.HttpContext.Session.GetInt32("userId");

				var walletTransaction = mapper.Map<WalletTransaction>(walletTransactionForm);
				//Може би loggedUserId да се намапва директно тука и да не се предава нататък за вадене на юзъра?
				//Да не се пази транзакцията като променлива?
				walletTransaction = walletTransactionService.CreateTransaction(walletTransaction, (int)loggedUserId);

				ViewBag.SuccessMessage = "Transaction completed successfully!";
				return View("Successful");
			}
			catch (EntityNotFoundException ex)
			{
				Response.StatusCode = StatusCodes.Status404NotFound;
				ViewData["ErrorMessage"] = ex.Message;

				return View("Error");
			}
			catch (InsufficientFundsException ex)
			{
				Response.StatusCode = StatusCodes.Status400BadRequest;
				ViewData["ErrorMessage"] = ex.Message;

				return View("Error");
			}
			catch (UnauthenticatedOperationException ex)
			{
				Response.StatusCode = StatusCodes.Status401Unauthorized;
				ViewData["ErrorMessage"] = ex.Message;

				return View("Error");
			}
			catch (UnauthorizedOperationException ex)
			{
				Response.StatusCode = StatusCodes.Status403Forbidden;
				ViewData["ErrorMessage"] = ex.Message;

				return View("Error");
			}
			catch (ArgumentException ex)
			{
				Response.StatusCode = StatusCodes.Status400BadRequest;
				ViewData["ErrorMessage"] = ex.Message;

				return View("Error");
			}
			catch (Exception ex)
			{
				Response.StatusCode = StatusCodes.Status500InternalServerError;
				ViewData["ErrorMessage"] = ex.Message;

				return View("Error");
			}
		}
	}
}
