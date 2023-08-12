﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Repositories.Contracts;
using VirtualWallet.Dto.ViewModels.CardViewModels;
using VirtualWallet.Dto.ViewModels.CurrencyViewModels;
using VirtualWallet.Dto.ViewModels.UserViewModels;
using VirtualWallet.Web.Helper;
using VirtualWallet.Web.Helper.Contracts;
using VirtualWallet.Dto.ViewModels.ExchangeViewModel;
using VirtualWallet.Dto.TransferDto;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Web.ViewControllers
{
    public class DashboardController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IAuthManagerMvc authManagerMvc;
        private readonly IWalletService walletService;
        private readonly ICardService cardService;
        private readonly IExchangeService exchangeService;

        public DashboardController(IUserService userService,
                                IMapper mapper,
                                IAuthManagerMvc authManagerMvc,
                                IWalletService walletService,
                                ICardService cardService,
                                IExchangeService exchangeService)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.authManagerMvc = authManagerMvc;
            this.walletService = walletService;
            this.cardService = cardService;
            this.exchangeService = exchangeService;
        }

        [HttpGet]
        public IActionResult Index(int userId)
        {
            try
            {
                if (!authManagerMvc.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }
                if (!authManagerMvc.IsAdmin("roleId") && !authManagerMvc.IsContentCreator("userId", userId))
                {
                    this.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    this.ViewData["ErrorMessage"] = AuthManagerMvc.notAthorized;
                    return View("Error");
                }

                //Този метод може да хвърли exception!
                var mappedCards = cardService.GetUserCards(userId).Select(c => mapper.Map<GetCardViewModel>(c)).ToList();
                var mappedBalances = walletService.GetWalletBalances(userId).Select(b => mapper.Map<GetBalanceViewModel>(b)).ToList();
                var dashBoardViewModel = new DashboardIndexViewModel
                {
                    Cards = mappedCards,
                    Balances = mappedBalances,
                };

                return View(dashBoardViewModel);
            }
            catch (EntityNotFoundException ex) //Трябва да се махне по назад във веригата хвърлянето и?
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
            catch (ArgumentException ex) //Може би няма нужда?
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                ViewData["ErrorMessage"] = ex.Message;

                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Exchanges(int id, PaginateExchanges form)
        {
            try
            {
                if (!authManagerMvc.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }
                if (!authManagerMvc.IsAdmin("roleId") && !authManagerMvc.IsContentCreator("userId", id))
                {
                    this.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    this.ViewData["ErrorMessage"] = AuthManagerMvc.notAthorized;
                    return View("Error");
                }
                ViewBag.Id = id;

                var queryParams = mapper.Map<QueryParameters>(form);
                form.Exchanges = exchangeService.GetUserExchanges(id, queryParams).Select(e => mapper.Map<GetExchangeViewModel>(e)).ToList();
                // Pagination logic
                var currentPage = form.Page ?? 1;
                var pageSize = 5;
                var totalUsers = form.Exchanges.Count;

                var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

                ViewBag.CurrentPage = currentPage;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalUsers = totalUsers;

                // Apply pagination
                form.Exchanges = form.Exchanges.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                return View(form);

            }
            catch (EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status200OK;
                this.ViewData["ErrorMessage"] = e.Message;
                ViewBag.Id = id;
                return View("Exchanges",form);
            }
            catch (Exception e)
            {
                this.Response.StatusCode = StatusCodes.Status500InternalServerError;
                this.ViewData["ErrorMessage"] = e.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Transactions(int id)
        {


            if (!authManagerMvc.IsLogged("LoggedUser"))
            {
                return RedirectToAction("Login", "User");
            }
            if (!authManagerMvc.IsAdmin("roleId") && !authManagerMvc.IsContentCreator("userId", id))
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = AuthManagerMvc.notAthorized;
                return View("Error");
            }
            ViewBag.Id = id;



            return View();
        }
        [HttpGet]
        public IActionResult Transfers(int id)
        {


            if (!authManagerMvc.IsLogged("LoggedUser"))
            {
                return RedirectToAction("Login", "User");
            }
            if (!authManagerMvc.IsAdmin("roleId") && !authManagerMvc.IsContentCreator("userId", id))
            {
                this.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = AuthManagerMvc.notAthorized;
                return View("Error");
            }
            ViewBag.Id = id;



            return View();
        }

    }
}
