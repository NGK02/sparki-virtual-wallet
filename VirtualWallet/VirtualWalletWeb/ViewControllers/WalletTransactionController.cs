using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.ExchangeDto;
using VirtualWallet.Dto.ViewModels.CurrencyViewModels;
using VirtualWallet.Dto.ViewModels.WalletTransactionViewModels;
using VirtualWallet.Web.Helper;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.ViewControllers
{
    public class WalletTransactionController : Controller
    {
        private readonly IAuthManagerMvc authManagerMvc;
        private readonly ICurrencyService currencyService;
        private readonly IMapper mapper;
        private readonly IWalletTransactionService walletTransactionService;

        public WalletTransactionController(IAuthManagerMvc authManagerMvc,
                                            ICurrencyService currencyService,
                                            IMapper mapper,
                                            IWalletTransactionService walletTransactionService)
        {
            this.authManagerMvc = authManagerMvc;
            this.currencyService = currencyService;
            this.mapper = mapper;
            this.walletTransactionService = walletTransactionService;
        }

        [HttpGet]
        public IActionResult CreateWalletTransaction(CreateWalletTransactionViewModel walletTransactionForm)
        {
            try
            {
                if (!authManagerMvc.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }
                if (authManagerMvc.IsBlocked("roleId"))
                {
                    Response.StatusCode = StatusCodes.Status403Forbidden;
                    ViewData["ErrorMessage"] = AuthManagerMvc.bloked;
                    return View("Error");
                }

                LoadCurrencies();

                return View(walletTransactionForm);
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
            catch (EntityNotFoundException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = ex.Message;

                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult ConfirmWalletTransaction(CreateWalletTransactionViewModel walletTransactionForm)
        {
            try
            {
                if (!authManagerMvc.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }

                LoadCurrencies();

                if (!InputsAreValid(walletTransactionForm))
				{
                    return View("CreateWalletTransaction", walletTransactionForm);
                }

                return View(walletTransactionForm);
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
            catch (EntityNotFoundException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = ex.Message;

                return View("Error");
            }
        }

        [HttpPost]
        public IActionResult FinalizeWalletTransaction(CreateWalletTransactionViewModel walletTransactionForm)
        {
            try
            {
                if (!authManagerMvc.IsLogged("LoggedUser"))
                {
                    return RedirectToAction("Login", "User");
                }
                var loggedUserId = (int)this.HttpContext.Session.GetInt32("userId");

                //Може би loggedUserId да се намапва директно тука и да не се предава нататък за вадене на юзъра?
                walletTransactionForm.SenderId = loggedUserId;
                var walletTransaction = mapper.Map<WalletTransaction>(walletTransactionForm);
                _ = walletTransactionService.CreateTransaction(walletTransaction);

                ViewBag.SuccessMessage = "Transaction completed successfully!";
                return View("Successful");
            }
            catch (EntityNotFoundException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = ex.Message;

                return View("Error");
            }
            catch (InsufficientFundsException ex)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
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
            catch (InvalidOperationException ex)
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
            //catch (Exception ex)
            //{
            //	Response.StatusCode = StatusCodes.Status500InternalServerError;
            //	ViewData["ErrorMessage"] = ex.Message;

            //	return View("Error");
            //}
        }

        private bool InputsAreValid(CreateWalletTransactionViewModel walletTransactionForm)
        {
            bool isValid = true;

            if (walletTransactionForm.RecipientIdentifierValue is null)
            { 
                this.ViewData["ErrorMessage"] = "Please provide a recipient identifier value!";

                isValid = false;
            }
            if (walletTransactionForm.RecipientIdentifier is null)
            {
                this.ViewData["ErrorMessage"] = "Please provide a recipient identifier!";

                isValid = false;
            }
            if (walletTransactionForm.Amount <= 0M)
            {
                this.ViewData["ErrorMessage"] = "Please provide amount greater than 0!";

                isValid = false;
            }
            if (walletTransactionForm.CurrencyId <= 0)
            {
                this.ViewData["ErrorMessage"] = "Please provide a currency!";

                isValid = false;
            }

            return isValid;
        }

        private void LoadCurrencies()
        {
            ViewData["Currencies"] = currencyService.GetCurrencies().Select(c => mapper.Map<CurrencyViewModel>(c)).ToList();
        }
    }
}
