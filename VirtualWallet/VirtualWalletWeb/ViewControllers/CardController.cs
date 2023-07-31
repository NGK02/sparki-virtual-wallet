using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.CardDto;
using VirtualWallet.Dto.ViewModels.CardViewModels;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.ViewControllers
{
    public class CardController : Controller
    {
        private readonly IAuthManager authManager;
        private readonly IAuthManagerMVC authManagerMVC;
        private readonly ICardService cardService;
        private readonly ICurrencyService currencyService;

        public CardController(IAuthManager authManager, IAuthManagerMVC authManagerMVC, ICardService cardService, ICurrencyService currencyService)
        {
            this.authManager = authManager;
            this.authManagerMVC = authManagerMVC;
            this.cardService = cardService;
            this.currencyService = currencyService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!authManagerMVC.isLogged("LoggedUser"))
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateCardViewModel createCardViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(createCardViewModel);
                }

                int userId = HttpContext.Session.GetInt32("userId") ?? 0;

                // Consider moving the currency assignment logic to the service layer
                var currency = currencyService.GetCurrencyByCode(createCardViewModel.CurrencyCode);

                string date = $"{createCardViewModel.ExpirationMonth}-{createCardViewModel.ExpirationYear}";

                // Move the date parsing logic to a separate method or a utility class
                if (!DateTime.TryParseExact(date, "MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime expirationDate))
                {
                    throw new ArgumentException("Invalid expiration date format.");
                }

                // Use AutoMapper
                var card = new Card
                {
                    CardHolder = createCardViewModel.CardHolder,
                    CardNumber = createCardViewModel.CardNumber,
                    CheckNumber = createCardViewModel.CheckNumber,
                    Currency = currency,
                    CurrencyId = currency.Id,
                    ExpirationDate = expirationDate
                };

                cardService.AddCard(card, userId);
                return RedirectToAction("Index", "Home");
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}