using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Dto.ViewModels;

namespace VirtualWallet.Web.ViewControllers
{
    public class HomeController : Controller
    {
        private readonly IUserService userService;
        private readonly IWalletTransactionService walletTransactionService;

        public HomeController(IUserService userService,IWalletTransactionService walletTransactionService)
        {
            this.userService = userService;
            this.walletTransactionService = walletTransactionService;
        }

        public IActionResult Index()
        {
            int usersCount = userService.GetUsersCount();
            int transactionsCount = walletTransactionService.GetTransactionsCount();
            var HomePageViewModel = new HomePageViewModel();
            HomePageViewModel.usersCount = usersCount;
            HomePageViewModel.transactionsCount = transactionsCount;
            return View(HomePageViewModel);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Successful()
        {
            return View();
        }
    }
}
