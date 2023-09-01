using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.TransferDto;

namespace VirtualWallet.Web.ApiControllers
{
    [ApiController]
    [Route("api/users/{userId}/transfers")]
    public class TransferApiController : ControllerBase
    {
        private readonly IAuthManager authManager;
        private readonly IMapper mapper;
        private readonly ITransferService transferService;

        public TransferApiController(IAuthManager authManager, IMapper mapper, ITransferService transferService)
        {
            this.authManager = authManager;
            this.mapper = mapper;
            this.transferService = transferService;
        }

        [HttpPost]
        public IActionResult AddTransfer([FromBody] CreateTransferDto createTransferDto, [FromHeader] string credentials, int userId)
        {

            var splitCredentials = authManager.SplitCredentials(credentials);
            var user = authManager.IsAuthenticated(splitCredentials);

            authManager.IsContentCreatorOrAdmin(user, userId);
            var transfer = mapper.Map<Transfer>(createTransferDto);

            transferService.CreateTransfer(transfer);
            var mappedTransfer = mapper.Map<GetTransferDto>(transfer);

            return StatusCode(201, mappedTransfer);

        }

        [HttpDelete("{transferId}")]
        public IActionResult DeleteTransfer([FromHeader] string credentials, int transferId, int userId)
        {

            var splitCredentials = authManager.SplitCredentials(credentials);
            var user = authManager.IsAuthenticated(splitCredentials);

            authManager.IsContentCreatorOrAdmin(user, userId);
            transferService.DeleteTransfer(transferId, userId);

            return Ok("Transfer deleted successfully.");

        }

        [HttpGet("{transferId}")]
        public IActionResult GetTransferById([FromHeader] string credentials, int transferId, int userId)
        {

            var splitCredentials = authManager.SplitCredentials(credentials);
            var user = authManager.IsAuthenticated(splitCredentials);

            authManager.IsContentCreatorOrAdmin(user, userId);
            var mappedTransfer = mapper.Map<GetTransferDto>(transferService.GetTransferById(transferId, userId));

            return Ok(mappedTransfer);

        }

        [HttpGet]
        public IActionResult GetUserTransfers([FromHeader] string credentials, int userId)
        {

            var splitCredentials = authManager.SplitCredentials(credentials);
            var user = authManager.IsAuthenticated(splitCredentials);

            authManager.IsContentCreatorOrAdmin(user, userId);
            var mappedTransfers = transferService.GetUserTransfers(userId).Select(t => mapper.Map<GetTransferDto>(t)).ToList();

            return Ok(mappedTransfers);

        }
    }
}