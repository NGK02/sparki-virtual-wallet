﻿using AutoMapper;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.CardDto;
using VirtualWallet.Dto.ViewModels.CardViewModels;
using VirtualWallet.Dto.ViewModels.CurrencyViewModels;
using VirtualWallet.Dto.ViewModels.TransferViewModels;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.ViewControllers
{
    public class TransferController : Controller
    {
        private readonly IAuthManagerMVC authManagerMVC;
        private readonly ICardService cardService;
        private readonly ICurrencyService currencyService;
        private readonly IMapper mapper;
        private readonly ITransferService transferService;

        public TransferController(IAuthManagerMVC authManagerMVC, ICardService cardService, ICurrencyService currencyService, IMapper mapper, ITransferService transferService)
        {
            this.authManagerMVC = authManagerMVC;
            this.cardService = cardService;
            this.currencyService = currencyService;
            this.mapper = mapper;
            this.transferService = transferService;
        }

        [HttpGet("Transfer/Add")]
        public IActionResult Add()
        {
            try
            {
                if (!authManagerMVC.isLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }

                int userId = HttpContext.Session.GetInt32("userId") ?? 0;

                var cards = cardService.GetUserCards(userId).Select(c => mapper.Map<SelectCardViewModel>(c)).ToList();
                ViewData["Cards"] = cards;

                var currencies = currencyService.GetCurrencies().Select(c => mapper.Map<CurrencyViewModel>(c)).ToList();
                ViewData["Currencies"] = currencies;

                var model = new TransferViewModel();
                ViewData["TransferViewModel"] = model;

                model.WalletId = userId;
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

        [HttpPost("Transfer/Add")]
        public IActionResult Add(TransferViewModel model)
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

                var transfer = mapper.Map<Transfer>(model);

                transferService.AddTransfer(userId, transfer);

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