using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.ViewModels.CardViewModels;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.ViewControllers
{
    public class CardController : Controller
    {
        private readonly IAuthManagerMVC authManagerMVC;
        private readonly ICardService cardService;
        private readonly IMapper mapper;

        public CardController(IAuthManagerMVC authManagerMVC, ICardService cardService, IMapper mapper)
        {
            this.authManagerMVC = authManagerMVC;
            this.cardService = cardService;
            this.mapper = mapper;
        }

        [HttpGet("Card/Add")]
        public IActionResult Add()
        {
            if (!authManagerMVC.isLogged("LoggedUser"))
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        [HttpPost("Card/Add")]
        public IActionResult Add(CardViewModel model)
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

                var card = mapper.Map<Card>(model);

                cardService.AddCard(card, userId);
                return RedirectToAction("Index", "Home");
            }
            catch (EntityNotFoundException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
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

        [HttpPost("Card/Delete/{cardId}")]
        public IActionResult Delete(int cardId)
        {
            try
            {
                if (!authManagerMVC.isLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }

                int userId = HttpContext.Session.GetInt32("userId") ?? 0;
                var card = cardService.GetCardById(cardId, userId);

                if (!authManagerMVC.isAdmin("roleId") && !authManagerMVC.isContentCreator("userId", card.UserId))
                {
                    Response.StatusCode = StatusCodes.Status403Forbidden;
                    return View("Error");
                }

                cardService.DeleteCard(cardId, userId);
                return RedirectToAction("Index", "Home");
            }
            catch (EntityNotFoundException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
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

        [HttpGet("Card/Update/{cardId}")]
        public IActionResult Update(int cardId)
        {
            try
            {
                if (!authManagerMVC.isLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }

                int userId = HttpContext.Session.GetInt32("userId") ?? 0;
                var card = cardService.GetCardById(cardId, userId);

                if (!authManagerMVC.isAdmin("roleId") && !authManagerMVC.isContentCreator("userId", card.UserId))
                {
                    Response.StatusCode = StatusCodes.Status403Forbidden;
                    return View("Error");
                }

                var model = mapper.Map<CardViewModel>(card);

                return View(model);
            }
            catch (EntityNotFoundException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
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

        [HttpPost("Card/Update/{cardId}")]
        public IActionResult Update(CardViewModel model, int cardId)
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

                var card = mapper.Map<Card>(model);

                cardService.UpdateCard(card, cardId, userId);
                return RedirectToAction("Index", "Home");
            }
            catch (EntityNotFoundException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
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