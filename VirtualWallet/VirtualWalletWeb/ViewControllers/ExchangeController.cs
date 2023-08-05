using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
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

        public ExchangeController(IAuthManagerMVC authManagerMVC, ICurrencyService currencyService,IMapper mapper)
        {
            this.authManagerMVC = authManagerMVC;
            this.currencyService = currencyService;
            this.mapper = mapper;
        }
    
        public IActionResult MakeExchange(CreateExcahngeDto createExchange)
        {
            if (!authManagerMVC.isLogged("LoggedUser"))
            {
                return RedirectToAction("Login", "User");
            }
            var currencies = currencyService.GetCurrencies().Select(c => mapper.Map<CurrencyViewModel>(c)).ToList();
            ViewData["Currencies"] = currencies;
            return View(createExchange);
        }

        public IActionResult ConfirmExchange(CreateExcahngeDto createExchange)
        {

            var currencies = currencyService.GetCurrencies().Select(c => mapper.Map<CurrencyViewModel>(c)).ToList();
            ViewData["Currencies"] = currencies;
            return View("MakeExchange", createExchange);
        }
    }
}
