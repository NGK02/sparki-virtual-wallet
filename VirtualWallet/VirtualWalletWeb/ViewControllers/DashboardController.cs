using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.Dto.TransferDto;
using VirtualWallet.Dto.ViewModels.ExchangeViewModel;
using VirtualWallet.Web.Helper;
using VirtualWallet.Web.Helper.Contracts;

namespace VirtualWallet.Web.ViewControllers
{
    public class DashboardController : Controller
    {
        private readonly IMapper mapper;
        private readonly IExchangeService exchangeService;
        private readonly IAuthManagerMvc authManagerMvc;
        public DashboardController(IMapper mapper, IExchangeService exchangeService, IAuthManagerMvc authManagerMvc)
        {
            this.mapper = mapper;
            this.exchangeService = exchangeService;
            this.authManagerMvc = authManagerMvc;
        }
        [HttpGet]
        public IActionResult Index(int id)
        {
            ViewBag.Id = id;
            return View("DashboardMain");
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
                var Exchanges = exchangeService.GetUserExchanges(id, queryParams).Select(e => mapper.Map<GetExchangeViewModel>(e)).ToList();
                form.Exchanges = Exchanges;

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
