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
        private readonly ICardService cardService;
        private readonly ICurrencyService currencyService;
        private readonly ITransferService transferService;
        private readonly IWalletService walletService;

        public TransferApiController(IAuthManager authManager, ICardService cardService, ICurrencyService currencyService, ITransferService transferService, IWalletService walletService)
        {
            this.authManager = authManager;
            this.cardService = cardService;
            this.currencyService = currencyService;
            this.transferService = transferService;
            this.walletService = walletService;
        }

        [HttpPost]
        public IActionResult AddTransfer([FromBody] TransferInfoDto transferInfoDto, [FromHeader] string credentials, int userId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);
                var user = authManager.IsAuthenticated(splitCredentials);

                authManager.IsContentCreatorOrAdmin(user, userId);
                var card = cardService.GetCardById(transferInfoDto.CardId, userId);
                var currency = currencyService.GetCurrencyById(transferInfoDto.CurrencyId);
                var wallet = walletService.GetWalletById(transferInfoDto.WalletId, userId);

                var transfer = new Transfer
                {
                    Amount = transferInfoDto.Amount,
                    Card = card,
                    CardId = transferInfoDto.CardId,
                    Currency = currency,
                    CurrencyId = transferInfoDto.CurrencyId,
                    HasCardSender = transferInfoDto.HasCardSender,
                    Wallet = wallet,
                    WalletId = transferInfoDto.WalletId
                };

                card.Transfers.Add(transfer);
                wallet.Transfers.Add(transfer);

                transferService.AddTransfer(transfer);

                return StatusCode(201, transfer);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{transferId}")]
        public IActionResult DeleteTransfer([FromHeader] string credentials, int transferId, int userId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);
                var user = authManager.IsAuthenticated(splitCredentials);

                authManager.IsContentCreatorOrAdmin(user, userId);
                transferService.DeleteTransfer(transferId, userId);

                return Ok("Transfer deleted successfully.");
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{transferId}")]
        public IActionResult GetTransferById([FromHeader] string credentials, int transferId, int userId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);
                var user = authManager.IsAuthenticated(splitCredentials);

                authManager.IsContentCreatorOrAdmin(user, userId);
                var transfer = transferService.GetTransferById(transferId, userId);

                return Ok(transfer);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetWalletTransfers([FromHeader] string credentials, int userId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);
                var user = authManager.IsAuthenticated(splitCredentials);

                authManager.IsContentCreatorOrAdmin(user, userId);
                var transfers = transferService.GetWalletTransfers(userId);

                return Ok(transfers);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (UnauthenticatedOperationException ex)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return StatusCode(StatusCodes.Status403Forbidden, ex.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}