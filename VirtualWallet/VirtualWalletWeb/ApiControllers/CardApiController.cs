using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Net;
using VirtualWallet.Business.AuthManager;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Enums;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.Dto.CardDto;

namespace VirtualWallet.Web.ApiControllers
{
    [ApiController]
    [Route("api/users/{userId}/cards")]
    public class CardApiController : ControllerBase
    {
        private readonly IAuthManager authManager;
        private readonly ICardService cardService;
        private readonly ICurrencyService currencyService;
        private readonly IUserService userService;

        public CardApiController(IAuthManager authManager, ICardService cardService, ICurrencyService currencyService, IUserService userService)
        {
            this.authManager = authManager;
            this.cardService = cardService;
            this.currencyService = currencyService;
            this.userService = userService;
        }

        [HttpPost]
        public IActionResult AddCard([FromBody] CardInfoDto cardInfoDto, [FromHeader] string credentials, int userId)
        {
            try
            {
				var splitCredentials = authManager.SplitCredentials(credentials);

				authManager.IsAuthenticated(splitCredentials);
                var currency = currencyService.GetCurrencyByCode(cardInfoDto.CurrencyCode);

                var card = new Card
                {
                    CardHolder = cardInfoDto.CardHolder,
                    CardNumber = cardInfoDto.CardNumber,
                    CheckNumber = cardInfoDto.CheckNumber,
                    Currency = currency,
                    CurrencyId = currency.Id,
                    ExpirationDate = cardInfoDto.ExpirationDate
                };

                cardService.AddCard(card, userId);
                return StatusCode(201, card);
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

        [HttpDelete("{cardId}")]
        public IActionResult DeleteCard([FromHeader] string credentials, int cardId, int userId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                cardService.DeleteCard(cardId, userId);
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

        [HttpGet("{cardId}")]
        public IActionResult GetCardById([FromHeader] string credentials, int cardId, int userId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                var card = cardService.GetCardById(cardId, userId);

                return Ok(card);
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
        public IActionResult GetUserCards([FromHeader] string credentials, int userId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                string username = splitCredentials[0];
                var user = userService.GetUserByUsername(username);

                authManager.IsContentCreatorOrAdmin(user, userId);
                var cards = cardService.GetUserCards(userId);

                return Ok(cards);
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

        [HttpPut("{cardId}")]
        public IActionResult UpdateCard([FromBody] CardInfoDto cardInfoDto, [FromHeader] string credentials, int cardId, int userId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                var currency = currencyService.GetCurrencyByCode(cardInfoDto.CurrencyCode);

                var card = new Card
                {
                    CardHolder = cardInfoDto.CardHolder,
                    CardNumber = cardInfoDto.CardNumber,
                    CheckNumber = cardInfoDto.CheckNumber,
                    Currency = currency,
                    CurrencyId = currency.Id,
                    ExpirationDate = cardInfoDto.ExpirationDate
                };

                cardService.UpdateCard(card, cardId, userId);
                return Ok(card);
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