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

namespace VirtualWallet.Web.ViewControllers
{
    public class DashboardController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IAuthManagerMvc authManagerMvc;
        private readonly IWalletService walletService;
        private readonly ICardService cardService;

        public DashboardController(IUserService userService,
                                IMapper mapper,
                                IAuthManagerMvc authManagerMvc,
                                IWalletService walletService,
                                ICardService cardService)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.authManagerMvc = authManagerMvc;
            this.walletService = walletService;
            this.cardService = cardService;
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
                var mappedBalances = walletService.GetWalletBalances(userId).Select(b => mapper.Map<GetBalanceViewModel>(b)).ToList(); ;
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
        public IActionResult Exchanges()
        {
            return View("Exchanges");
        }
    }
}
