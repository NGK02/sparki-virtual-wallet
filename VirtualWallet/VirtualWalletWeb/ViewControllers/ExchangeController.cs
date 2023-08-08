using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.CreateExcahngeDto;
using VirtualWallet.Dto.ExchangeDto;
using VirtualWallet.Dto.ViewModels.CurrencyViewModels;
using VirtualWallet.Web.Helper;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.ViewControllers
{
	public class ExchangeController : Controller
	{
		private readonly IAuthManagerMvc authManagerMVC;
		private readonly ICurrencyService currencyService;
		private readonly IMapper mapper;
		private readonly IWalletService walletService;
		private readonly IExchangeService exchangeService;

		public ExchangeController(IAuthManagerMvc authManagerMVC,
									ICurrencyService currencyService,
									IMapper mapper,
									IWalletService walletService,
									IExchangeService exchangeService)
		{
			this.authManagerMVC = authManagerMVC;
			this.currencyService = currencyService;
			this.mapper = mapper;
			this.walletService = walletService;
			this.exchangeService = exchangeService;
		}
		public IActionResult FinalizeExchangeDev()
		{
			return View("FinalizeExchange");
		}
		public IActionResult MakeExchange(CreateExcahngeDto createExchange)
		{
			try
			{
				if (!authManagerMVC.IsLogged("LoggedUser"))
				{
					return RedirectToAction("Login", "User");
				}
				this.ViewBag.AmountToEdit = createExchange.Amount;
				ViewData["Currencies"] = LoadCurrencies();
				return View(createExchange);

			}
			catch (Exception e)
			{
				this.Response.StatusCode = StatusCodes.Status500InternalServerError;
				this.ViewData["ErrorMessage"] = e.Message;
				return View("Error");
			}
		}

		public IActionResult ConfirmExchange(CreateExcahngeDto createExchange)
		{
			try
			{
				if (!authManagerMVC.IsLogged("LoggedUser"))
				{
					return RedirectToAction("Login", "User");
				}

				var currencies = new List<CurrencyViewModel>();
				//Влизаме тук ако някое от полетата не е попълнено или е подадена негативна или неадекватна стойност за Amount!
				if (!this.ModelState.IsValid)
				{
					currencies = currencyService.GetCurrencies().Select(c => mapper.Map<CurrencyViewModel>(c)).ToList();
					ViewData["Currencies"] = currencies;
					this.ViewData["ErrorMessage"] = (createExchange.Amount <= 0 ? "Please provide positive Amount!" : "Please provide input!");
					return View("MakeExchange", createExchange);
				}

				var loggedUserId = this.HttpContext.Session.GetInt32("userId");
				var loggedUserWallet = walletService.GetWalletById((int)loggedUserId);

				_ = walletService.ValidateFunds(loggedUserWallet, createExchange);

				ViewBag.Rate = exchangeService.GetExchangeRate(createExchange.From, createExchange.To).Result;
				ViewBag.ExpectedAmount = ViewBag.Rate * createExchange.Amount;



				return View(createExchange);

			}
			catch (InsufficientFundsException e)
			{
				this.ViewData["ErrorMessage"] = e.Message;
				ViewData["Currencies"] = LoadCurrencies();
				return View("MakeExchange", createExchange);
			}
			catch (EntityNotFoundException e)
			{
				this.ViewData["ErrorMessage"] = e.Message;
				ViewData["Currencies"] = LoadCurrencies();
				return View("MakeExchange", createExchange);
			}
			catch (ArgumentException e)
			{
				this.ViewData["ErrorMessage"] = e.Message;
				ViewData["Currencies"] = LoadCurrencies();
				return View("MakeExchange", createExchange);
			}
			catch (Exception e)
			{
				this.Response.StatusCode = StatusCodes.Status500InternalServerError;
				this.ViewData["ErrorMessage"] = e.Message;
				return View("Error");
			}
		}
		[HttpPost]
		public IActionResult FinalizeExchange(CreateExcahngeDto createExchange)
		{
			try
			{
				var loggedUserId = this.HttpContext.Session.GetInt32("userId");
				//Да се преправи метода в сървиса да изисква само userId
				//Да се мапва тук към обекта
				//Не съм сигурен дали ExchangeFunds има нужда да е асинхронен.
				var exchange = walletService.ExchangeFunds(createExchange, (int)loggedUserId, (int)loggedUserId).Result;
				var exchangeDto = mapper.Map<GetExchangeDto>(exchange);
				return View(exchangeDto);

			}

			catch (ArgumentException e)
			{
				this.ViewData["ErrorMessage"] = e.Message;
				ViewData["Currencies"] = LoadCurrencies();
				return View("MakeExchange", createExchange);
			}
			catch (Exception e)
			{
				this.Response.StatusCode = StatusCodes.Status500InternalServerError;
				this.ViewData["ErrorMessage"] = e.Message;
				return View("Error");
			}

		}

		private List<CurrencyViewModel> LoadCurrencies()
		{
			return currencyService.GetCurrencies().Select(c => mapper.Map<CurrencyViewModel>(c)).ToList();
		}
	}
}
