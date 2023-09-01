using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.QueryParameters;
using VirtualWallet.DataAccess.Repositories;
using VirtualWallet.Dto.TransactionDto;
using VirtualWallet.Dto.UserDto;

namespace VirtualWallet.Web.ApiControllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminApiController : ControllerBase
    {
        private readonly IAuthManager authManager;
        private readonly IAdminService adminService;
        private readonly ICardService cardService;
        private readonly ITransferService transferService;
        private readonly IWalletService walletService;
        private readonly IWalletTransactionService walletTransactionService;
        private readonly IMapper mapper;
        public AdminApiController(IAuthManager authManager,
                                IAdminService adminService,
                                ICardService cardService,
                                ITransferService transferService,
                                IWalletService walletService,
                                IWalletTransactionService walletTransactionService,
                                IMapper mapper)
        {
            this.authManager = authManager;
            this.adminService = adminService;
            this.cardService = cardService;
            this.mapper = mapper;
            this.transferService = transferService;
            this.walletService = walletService;
            this.walletTransactionService = walletTransactionService;
        }

        [HttpPut("block")]
        public IActionResult BlockUser([FromHeader] string credentials, [FromQuery] int? id, string username, string email, string phoneNumber)
        {

            var splitCredentials = authManager.SplitCredentials(credentials);
            authManager.IsAdmin(splitCredentials);

            adminService.BlockUser(id, username, email, phoneNumber);

            return Ok("User blocked successfully!");

        }

        [HttpPut("unblock")]
        public IActionResult UnblockUser(string credentials, [FromQuery] int? id, string username, string email, string phoneNumber)
        {

            var splitCredentials = authManager.SplitCredentials(credentials);
            authManager.IsAdmin(splitCredentials);

            adminService.UnblockUser(id, username, email, phoneNumber);

            return Ok("User unblocked successfully!");

        }

        [HttpGet("cards")]
        public IActionResult GetAllCards([FromHeader] string credentials)
        {

            var splitCredentials = authManager.SplitCredentials(credentials);
            var user = authManager.IsAdmin(splitCredentials);

            var cards = cardService.GetCards();

            return Ok(cards);

        }

        [HttpGet("transfers")]
        public IActionResult GetAllTransfers([FromHeader] string credentials)
        {

            var splitCredentials = authManager.SplitCredentials(credentials);
            var user = authManager.IsAdmin(splitCredentials);

            var transfers = transferService.GetTransfers(user.Id);

            return Ok(transfers);

        }
        [HttpGet("transactions")]
        public IActionResult GetAllWalletTransactions([FromHeader] string credentials, [FromQuery] WalletTransactionQueryParameters queryParameters)
        {

            var splitCredentials = authManager.SplitCredentials(credentials);
            authManager.IsAdmin(splitCredentials);

            var walletTransactions = walletTransactionService.GetWalletTransactions(queryParameters);
            var walletTransactionsMapped = walletTransactions.Select(wt => mapper.Map<GetWalletTransactionDto>(wt)).ToList();

            return Ok(walletTransactionsMapped);

        }


        [HttpGet("users")]
        public IActionResult GetAllUsers(string credentials, [FromQuery] UserQueryParameters userParameters)
        {

            var splitCredentials = authManager.SplitCredentials(credentials);
            authManager.IsAdmin(splitCredentials);

            var result = adminService.GetUsers(userParameters).Select(u => mapper.Map<GetUserDto>(u));

            return Ok(result);

        }
    }
}
