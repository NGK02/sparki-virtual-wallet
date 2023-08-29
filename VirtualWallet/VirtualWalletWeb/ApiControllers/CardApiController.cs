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
using VirtualWallet.Dto.TransferDto;

namespace VirtualWallet.Web.ApiControllers
{
    [ApiController]
    [Route("api/users/{userId}/cards")]
    public class CardApiController : ControllerBase
    {
        private readonly IAuthManager authManager;
        private readonly ICardService cardService;
        private readonly IMapper mapper;

        public CardApiController(IAuthManager authManager, ICardService cardService, IMapper mapper)
        {
            this.authManager = authManager;
            this.cardService = cardService;
            this.mapper = mapper;
        }

        [HttpPost]
        public IActionResult AddCard([FromBody] CardInfoDto cardInfoDto, [FromHeader] string credentials, int userId)
        {
            try
            {
				var splitCredentials = authManager.SplitCredentials(credentials);
                var user = authManager.IsAuthenticated(splitCredentials);

                authManager.IsContentCreatorOrAdmin(user, userId);
                var card = mapper.Map<Card>(cardInfoDto);

                cardService.CreateCard(card, userId);
                return StatusCode(201, cardInfoDto);
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
                var user = authManager.IsAuthenticated(splitCredentials);

                authManager.IsContentCreatorOrAdmin(user, userId);
                cardService.DeleteCard(cardId, userId);
                return Ok("Card deleted successfully.");
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
                var user = authManager.IsAuthenticated(splitCredentials);

                authManager.IsContentCreatorOrAdmin(user, userId);
                var card = cardService.GetCardById(cardId, userId);
                var cardInfoDto = mapper.Map<CardInfoDto>(card);

                return Ok(cardInfoDto);
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
                var user = authManager.IsAuthenticated(splitCredentials);

                authManager.IsContentCreatorOrAdmin(user, userId);
                var cards = cardService.GetUserCards(userId).Select(c => mapper.Map<CardInfoDto>(c)).ToList();

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
                var user = authManager.IsAuthenticated(splitCredentials);

                authManager.IsContentCreatorOrAdmin(user, userId);
                var card = mapper.Map<Card>(cardInfoDto);

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