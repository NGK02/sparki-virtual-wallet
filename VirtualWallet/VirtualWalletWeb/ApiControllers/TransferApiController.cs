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
    [Route("api/transfers")]
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
        public IActionResult AddTransfer([FromBody] TransferInfoDto transferInfoDto, [FromHeader] string credentials)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                string username = splitCredentials[0];
                var card = cardService.GetCardById(transferInfoDto.CardId, username);
                var currency = currencyService.GetCurrencyById(transferInfoDto.CurrencyId);
                var wallet = walletService.GetWalletById(transferInfoDto.WalletId, username);

                var transfer = new Transfer
                {
                    Amount = transferInfoDto.Amount,
                    Card = card,
                    CardId = transferInfoDto.CardId,
                    Currency = currency,
                    CurrencyId = transferInfoDto.CurrencyId,
                    Wallet = wallet,
                    WalletId = transferInfoDto.WalletId
                };

                card.Transfers.Add(transfer);
                wallet.Transfers.Add(transfer);

                transferService.AddTransfer(username, transfer);

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

        [HttpDelete("{transferId}")]
        public IActionResult DeleteTransfer([FromHeader] string credentials, int transferId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                string username = splitCredentials[0];
                transferService.DeleteTransfer(transferId, username);

                return NoContent();
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
        public IActionResult GetTransferById([FromHeader] string credentials, int transferId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                string username = splitCredentials[0];
                var transfer = transferService.GetTransferById(transferId, username);

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
        public IActionResult GetTransfers([FromHeader] string credentials)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                string username = splitCredentials[0];
                var transfers = transferService.GetTransfers(username);

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

        [HttpGet("wallet")]
        public IActionResult GetWalletTransfers([FromHeader] string credentials)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                string username = splitCredentials[0];
                var transfers = transferService.GetWalletTransfers(username);

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