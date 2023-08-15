using AutoMapper;
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
using VirtualWallet.Dto.TransferDto;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.Dto.TransactionDto;
using VirtualWallet.Dto.ViewModels.ExchangeViewModels;
using VirtualWallet.Dto.ViewModels.WalletTransactionViewModels;
using VirtualWallet.Dto.ViewModels.TransferViewModels;

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
        private readonly IWalletTransactionService walletTransactionService;
        private readonly ITransferService transferService;

        public DashboardController(IUserService userService,
                                IMapper mapper,
                                IAuthManagerMvc authManagerMvc,
                                IWalletService walletService,
                                ICardService cardService,
                                IExchangeService exchangeService,
                                IWalletTransactionService walletTransactionService,
                                ITransferService transferService)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.authManagerMvc = authManagerMvc;
            this.walletService = walletService;
            this.cardService = cardService;
            this.exchangeService = exchangeService;
            this.walletTransactionService = walletTransactionService;
            this.transferService = transferService;
        }

        [HttpGet]
        public IActionResult Index(int id)
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
                    this.ViewData["ErrorMessage"] = AuthManagerMvc.notAuthorized;
                    return View("Error");
                }

                ViewBag.Id = id;

                var mappedCards = cardService.ListUserCards(id).Select(c => mapper.Map<GetCardViewModel>(c)).ToList();
                var mappedBalances = walletService.GetWalletBalances(id).Select(b => mapper.Map<GetBalanceViewModel>(b)).ToList();
                var incomingData = walletTransactionService.GetUserIncomingTransactionsForLastWeek(id, CurrencyCode.USD);
                var outgoingData = walletTransactionService.GetUserOutgoingTransactionsForLastWeek(id, CurrencyCode.USD);

                var dashBoardViewModel = new DashboardIndexViewModel
                {
                    Cards = mappedCards,
                    Balances = mappedBalances,
                    IncomingWalletTransactions = incomingData,
                    OutgoingWalletTransactions = outgoingData
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
                    this.ViewData["ErrorMessage"] = AuthManagerMvc.notAuthorized;
                    return View("Error");
                }
                ViewBag.Id = id;

                var queryParams = mapper.Map<QueryParams>(form);
                form.Exchanges = exchangeService.GetUserExchanges(id, queryParams).Select(e => mapper.Map<GetExchangeViewModel>(e)).ToList();

                // Pagination logic
                var currentPage = form.Page ?? 1;
                var pageSize = 5;
                var totalExchanges = form.Exchanges.Count;
                var totalPages = (int)Math.Ceiling(totalExchanges / (double)pageSize);

                ViewBag.CurrentPage = currentPage;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalExchanges = totalExchanges;

                // Apply pagination
                form.Exchanges = form.Exchanges.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                return View(form);

            }
            catch (EntityNotFoundException e)
            {
                //Това трябва да се оправи, да не се хвърля изобщо ексепшън.
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
        public IActionResult WalletTransactions(int id, PaginateWalletTransactions form)
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
                    this.ViewData["ErrorMessage"] = AuthManagerMvc.notAuthorized;
                    return View("Error");
                }
                ViewBag.Id = id;

                var queryParams = mapper.Map<WalletTransactionQueryParameters>(form);
                form.WalletTransactions = walletTransactionService.GetUserWalletTransactions(queryParams, id).Select(wt => mapper.Map<GetWalletTransactionViewModel>(wt)).ToList();

                // Pagination logic
                var currentPage = form.Page ?? 1;
                var pageSize = 5;
                var totalWalletTransactions = form.WalletTransactions.Count;
                var totalPages = (int)Math.Ceiling(totalWalletTransactions / (double)pageSize);

                ViewBag.CurrentPage = currentPage;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalWalletTransactions = totalWalletTransactions;

                // Apply pagination
                form.WalletTransactions = form.WalletTransactions.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

                return View(form);

            }
            catch (EntityNotFoundException e)
            {
                //Това трябва да се оправи, да не се хвърля изобщо ексепшън.
                this.Response.StatusCode = StatusCodes.Status200OK;
                this.ViewData["ErrorMessage"] = e.Message;
                ViewBag.Id = id;
                return View("Exchanges", form);
            }
            catch (Exception e)
            {
                this.Response.StatusCode = StatusCodes.Status500InternalServerError;
                this.ViewData["ErrorMessage"] = e.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Transfers(int id, PaginatedTransfersViewModel model)
        {
            try
            {
                if (!authManagerMvc.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }

                if (!authManagerMvc.IsAdmin("roleId") && !authManagerMvc.IsContentCreator("userId", id))
                {
                    HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    ViewData["ErrorMessage"] = AuthManagerMvc.notAuthorized;

                    return View("Error");
                }

                ViewBag.Id = id;

                var queryParams = mapper.Map<QueryParams>(model);

                model.Transfers = transferService.GetUserTransfers(id, queryParams).Select(t => mapper.Map<GetTransferViewModel>(t)).ToList();
                var currentPage = model.Page ?? 1;
                var pageSize = 5;
                var totalTransfers = model.Transfers.Count;

                var totalPages = (int) Math.Ceiling(totalTransfers / (double) pageSize);
                ViewBag.CurrentPage = currentPage;
                ViewBag.TotalPages = totalPages;
                ViewBag.TotalTransfers = totalTransfers;

                model.Transfers = model.Transfers.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                return View(model);
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;

                ViewBag.Id = id;

                return View("Transfers", model);
            }
            catch (Exception e)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }
    }
}