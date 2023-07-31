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
        public IActionResult Add()
        {
            if (!authManagerMVC.isLogged("LoggedUser"))
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Add(CreateCardViewModel model)
        {
            try
            {
                if (!authManagerMVC.isLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }

                int userId = HttpContext.Session.GetInt32("userId") ?? 0;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // Consider moving the currency assignment logic to the service layer
                var currency = currencyService.GetCurrencyByCode(model.CurrencyCode);

                string date = $"{model.ExpirationMonth}-{model.ExpirationYear}";

                // Move the date parsing logic to a separate method or a utility class
                if (!DateTime.TryParseExact(date, "MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime expirationDate))
                {
                    throw new ArgumentException("Invalid expiration date format.");
                }

                // Use AutoMapper
                var card = new Card
                {
                    CardHolder = model.CardHolder,
                    CardNumber = model.CardNumber,
                    CheckNumber = model.CheckNumber,
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

        [HttpGet("Card/Edit/{cardId}")]
        public IActionResult Edit(int cardId)
        {
            if (!authManagerMVC.isLogged("LoggedUser"))
            {
                return RedirectToAction("Login", "User");
            }

            int userId = HttpContext.Session.GetInt32("userId") ?? 0;
            var card = cardService.GetCardById(cardId, userId);

            if (!authManagerMVC.isAdmin("roleId") && !authManagerMVC.isContentCreator("userId", card.UserId))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                return View("Error");
            }

            string month = card.ExpirationDate.ToString("MM");
            string year = card.ExpirationDate.ToString("yyyy");

            var model = new EditCardViewModel
            {
                CardHolder = card.CardHolder,
                CardNumber = card.CardNumber,
                CheckNumber = card.CheckNumber,
                CurrencyCode = card.Currency.Code.ToString(),
                ExpirationMonth = month,
                ExpirationYear = year
            };

            return View(model);
        }

        [HttpPost("Card/Edit/{cardId}")]
        public IActionResult Edit(EditCardViewModel model, int cardId)
        {
            try
            {
                if (!authManagerMVC.isLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }

                int userId = HttpContext.Session.GetInt32("userId") ?? 0;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // Consider moving the currency assignment logic to the service layer
                var currency = currencyService.GetCurrencyByCode(model.CurrencyCode);

                string date = $"{model.ExpirationMonth}-{model.ExpirationYear}";

                // Move the date parsing logic to a separate method or a utility class
                if (!DateTime.TryParseExact(date, "MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime expirationDate))
                {
                    throw new ArgumentException("Invalid expiration date format.");
                }

                // Use AutoMapper
                var card = new Card
                {
                    CardHolder = model.CardHolder,
                    CardNumber = model.CardNumber,
                    CheckNumber = model.CheckNumber,
                    Currency = currency,
                    CurrencyId = currency.Id,
                    ExpirationDate = expirationDate
                };

                cardService.UpdateCard(card, cardId, userId);
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