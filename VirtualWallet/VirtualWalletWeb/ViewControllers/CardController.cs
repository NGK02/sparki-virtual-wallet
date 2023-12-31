﻿using AutoMapper;
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
        private readonly IAuthManagerMvc authManagerMVC;
        private readonly ICardService cardService;
        private readonly IMapper mapper;

        public CardController(IAuthManagerMvc authManagerMVC, ICardService cardService, IMapper mapper)
        {
            this.authManagerMVC = authManagerMVC;
            this.cardService = cardService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult CreateCard()
        {
            if (!authManagerMVC.IsLogged("LoggedUser"))
            {
                return RedirectToAction("Login", "User");
            }

            return View();
        }

        [HttpPost]
        public IActionResult CreateCard(CardViewModel model)
        {
            try
            {
                if (!authManagerMVC.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }

                int userId = HttpContext.Session.GetInt32("userId") ?? 0;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var card = mapper.Map<Card>(model);
                cardService.CreateCard(card, userId);

                this.ViewBag.SuccessMessage = "Card added successfully!";
                return View("Successful");
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

        [HttpGet]
        public IActionResult ConfirmDeleteCard(int cardId)
        {
            if (!authManagerMVC.IsLogged("LoggedUser"))
            {
                return RedirectToAction("Login", "User");
            }

            //Тези страници за потвърждение на нова страница не работят добре.
            ViewBag.CardId = cardId;

            return View();
        }

        [HttpPost]
        public IActionResult DeleteCard(int cardId)
        {
            try
            {
                if (!authManagerMVC.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }

                int userId = HttpContext.Session.GetInt32("userId") ?? 0;
                var card = cardService.GetCardById(cardId, userId);

                if (!authManagerMVC.IsAdmin("roleId") && !authManagerMVC.IsContentCreator("userId", card.UserId))
                {
                    Response.StatusCode = StatusCodes.Status403Forbidden;
                    return View("Error");
                }

                cardService.DeleteCard(cardId, userId);

                this.ViewBag.SuccessMessage = "Card deleted successfully!";
                return View("Successful");
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

        [HttpGet]
        public IActionResult EditCard(int cardId)
        {
            try
            {
                if (!authManagerMVC.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }

                int userId = HttpContext.Session.GetInt32("userId") ?? 0;
                var card = cardService.GetCardById(cardId, userId);

                if (!authManagerMVC.IsAdmin("roleId") && !authManagerMVC.IsContentCreator("userId", card.UserId))
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

        [HttpPost]
        public IActionResult EditCard(CardViewModel model, int id)
        {
            try
            {
                if (!authManagerMVC.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }

                int userId = HttpContext.Session.GetInt32("userId") ?? 0;

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var card = mapper.Map<Card>(model);
                cardService.UpdateCard(card, id, userId);

                this.ViewBag.SuccessMessage = "Card edited successfully!";
                return View("Successful");
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