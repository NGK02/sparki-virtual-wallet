using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.UserDto;
using System.Net;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Dto.CreateExcahngeDto;
using VirtualWallet.Dto.ExchangeDto;
using VirtualWallet.Web.Helper.Contracts;
using VirtualWallet.DataAccess.QueryParameters;

namespace VirtualWallet.Web.ApiControllers
{
    [ApiController]
    [Route("api/users")]
    public class UserApiController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IAuthManager authManager;
        private readonly IWalletService walletService;
        private readonly IExchangeService exchangeService;
        private readonly IImageManager imageManager;
        public UserApiController(IMapper mapper,
                                IUserService userService,
                                IAuthManager authManager,
                                IWalletService walletService,
                                IExchangeService exchangeService,
                                IImageManager imageManager)
        {
            this.mapper = mapper;
            this.userService = userService;
            this.authManager = authManager;
            this.walletService = walletService;
            this.exchangeService = exchangeService;
            this.imageManager = imageManager;
        }


        [HttpPost("")]
        public IActionResult CreateUser([FromBody] CreateUserDto userDto)
        {
            try
            {
                User mappedUser = mapper.Map<User>(userDto);

                var ProfilePic = imageManager.GeneratePlaceholderAvatar(userDto.FirstName, userDto.LastName);
                mappedUser.ProfilePicPath = imageManager.UploadGeneratedProfilePicInRoot(ProfilePic).Result;

                _ = userService.CreateUser(mappedUser);

                return Ok("Registered Successfully!");
            }
            catch (EmailAlreadyExistException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (UsernameAlreadyExistException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (PhoneNumberAlreadyExistException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById([FromHeader] string credentials, [FromRoute] int id)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);
                _ = authManager.IsAuthenticated(splitCredentials);
                string username = splitCredentials[0];
                var user = userService.GetUserById(id);
                var mappedUser = mapper.Map<GetUserDto>(user);
                return Ok(mappedUser);

            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (UnauthenticatedOperationException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditUser([FromHeader] string credentials, [FromRoute] int id, [FromBody] UpdateUserDto userValues)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);
                var loggedUser = authManager.IsAuthenticated(splitCredentials);
                authManager.IsContentCreatorOrAdmin(loggedUser, id);
                string username = splitCredentials[0];
                var mapped = mapper.Map<User>(userValues);
                var updatedUser = userService.UpdateUser(username, mapped);

                return Ok("Updated Successfully!");

            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (UnauthenticatedOperationException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (EmailAlreadyExistException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (PhoneNumberAlreadyExistException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteUser([FromHeader] string credentials, [FromRoute] int id)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);
                var loggedUser = authManager.IsAuthenticated(splitCredentials);
                authManager.IsContentCreatorOrAdmin(loggedUser, id);
                string username = splitCredentials[0];

                imageManager.DeleteProfilePicFromRoot(loggedUser.ProfilePicPath);
                userService.DeleteUser(username, null);
                return Ok("User Deleted!");

            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
            catch (UnauthenticatedOperationException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost("{id}/exchange")]
        public async Task<IActionResult> ExchangeCurrency([FromHeader] string credentials, [FromRoute] int id, [FromBody] CreateExcahngeDto excahngeAmounts)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);
                var loggedUser = authManager.IsAuthenticated(splitCredentials);
                authManager.IsContentCreatorOrAdmin(loggedUser, id);
                //string username = splitCredentials[0];

                var userWallet = walletService.GetWalletById(id);
                _ = walletService.ValidateFunds(userWallet, excahngeAmounts);
                var exchange = await walletService.ExchangeFunds(excahngeAmounts, loggedUser.WalletId, id);
                var GetExchange = mapper.Map<GetExchangeDto>(exchange);
                return Ok(GetExchange);
            }
            catch(EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (InsufficientFundsException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (UnauthenticatedOperationException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}/exchanges")]
        public IActionResult GetUserExchanges([FromHeader] string credentials, int id)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);
                var user = authManager.IsAuthenticated(splitCredentials);

                authManager.IsContentCreatorOrAdmin(user, id);
                var parameters = new QueryParameters();
                var exchanges = exchangeService.GetUserExchanges(id,parameters).Select(e => mapper.Map<GetExchangeDto>(e));

                return Ok(exchanges);
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
