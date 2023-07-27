using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.TransactionDto;
using VirtualWallet.Dto.TransferDto;

namespace VirtualWallet.Web.ApiControllers
{
    [ApiController]
    [Route("api/users/{userId}/transfers")]
    public class TransferApiController : ControllerBase
    {
        private readonly IAuthManager authManager;
        private readonly ITransferService transferService;
        private readonly IMapper mapper;

        public TransferApiController(IAuthManager authManager, ITransferService transferService, IMapper mapper)
        {
            this.authManager = authManager;
            this.transferService = transferService;
            this.mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateTransfer([FromBody] CreateTransferDto createTransferDto, [FromHeader] string credentials, int userId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);
                var user = authManager.IsAuthenticated(splitCredentials);
                authManager.IsContentCreatorOrAdmin(user, userId);

				var transfer = mapper.Map<Transfer>(createTransferDto);
				transferService.CreateTransfer(userId, transfer);

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
        public IActionResult GetUserTransferById([FromHeader] string credentials, int transferId, int userId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);
                var user = authManager.IsAuthenticated(splitCredentials);
                authManager.IsContentCreatorOrAdmin(user, userId);

                var mappedTransfer = mapper.Map<GetTransferDto>(transferService.GetTransferById(transferId, userId));
   
                return Ok(mappedTransfer);
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
        public IActionResult GetUserTransfers([FromHeader] string credentials, int userId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);
                var user = authManager.IsAuthenticated(splitCredentials);

                authManager.IsContentCreatorOrAdmin(user, userId);
                var mappedTransfers = transferService.GetUserTransfers(userId).Select(t => mapper.Map<GetTransferDto>(t)).ToList();

                return Ok(mappedTransfers);
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