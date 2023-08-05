using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.CreateExcahngeDto;
using VirtualWallet.Dto.ViewModels.CurrencyViewModels;
using VirtualWallet.Web.Helper;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.ViewControllers
{
    public class ExchangeController : Controller
    {
        private readonly IAuthManagerMVC authManagerMVC;
        private readonly ICurrencyService currencyService;
        private readonly IMapper mapper;
        private readonly IWalletService walletService;

        public ExchangeController(IAuthManagerMVC authManagerMVC, ICurrencyService currencyService, IMapper mapper, IWalletService walletService)
        {
            this.authManagerMVC = authManagerMVC;
            this.currencyService = currencyService;
            this.mapper = mapper;
            this.walletService = walletService;
        }
        public IActionResult ConfirmExchangeDev()
        {
            return View("ConfirmExchange");
        }
        public IActionResult MakeExchange(CreateExcahngeDto createExchange)
        {
            
            if (!authManagerMVC.isLogged("LoggedUser"))
            {
                return RedirectToAction("Login", "User");
            }
            ViewData["Currencies"] = LoadCurrencies();
            return View(createExchange);
        }

        public IActionResult ConfirmExchange(CreateExcahngeDto createExchange)
        {
            try
            {
                if (!authManagerMVC.isLogged("LoggedUser"))
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
                var loggedUserWallet = walletService.GetWalletById((int)loggedUserId, (int)loggedUserId);

                _ = walletService.ValidateFunds(loggedUserWallet, createExchange);


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
        }

        private List<CurrencyViewModel> LoadCurrencies()
        {
            return currencyService.GetCurrencies().Select(c => mapper.Map<CurrencyViewModel>(c)).ToList();
        }
    }
}
