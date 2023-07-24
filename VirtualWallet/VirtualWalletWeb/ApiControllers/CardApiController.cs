﻿using AutoMapper;
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
    [Route("api/cards")]
    public class CardApiController : ControllerBase
    {
        private readonly IAuthManager authManager;
        private readonly ICardService cardService;
        private readonly ICurrencyService currencyService;

        public CardApiController(IAuthManager authManager, ICardService cardService, ICurrencyService currencyService)
        {
            this.authManager = authManager;
            this.cardService = cardService;
            this.currencyService = currencyService;
        }

        [HttpPost]
        public IActionResult AddCard([FromBody] CardInfoDto cardInfoDto, [FromHeader] string credentials)
        {
            try
            {
				var splitCredentials = authManager.SplitCredentials(credentials);

				authManager.IsAuthenticated(splitCredentials);
				string username = splitCredentials[0];
                var currency = currencyService.GetCurrencyByCode(cardInfoDto.CurrencyCode);

                var card = new Card
                {
                    CardHolder = cardInfoDto.CardHolder,
                    CardNumber = cardInfoDto.CardNumber,
                    CheckNumber = cardInfoDto.CheckNumber,
                    Currency = currency,
                    ExpirationDate = cardInfoDto.ExpirationDate
                };

                cardService.AddCard(card, username);
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

        [HttpDelete("{cardId}")]
        public IActionResult DeleteCard([FromHeader] string credentials, int cardId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                string username = splitCredentials[0];

                cardService.DeleteCard(cardId, username);
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
        public IActionResult GetCardById([FromHeader] string credentials, int cardId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                string username = splitCredentials[0];
                var card = cardService.GetCardById(cardId, username);

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
        public IActionResult GetCards([FromHeader] string credentials)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                string username = splitCredentials[0];
                var cards = cardService.GetCards(username);

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

        [HttpGet("user")]
        public IActionResult GetUserCards([FromHeader] string credentials)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                string username = splitCredentials[0];
                var cards = cardService.GetUserCards(username);

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
        public IActionResult UpdateCard([FromBody] CardInfoDto cardInfoDto, [FromHeader] string credentials, int cardId)
        {
            try
            {
                var splitCredentials = authManager.SplitCredentials(credentials);

                authManager.IsAuthenticated(splitCredentials);
                string username = splitCredentials[0];
                var currency = currencyService.GetCurrencyByCode(cardInfoDto.CurrencyCode);

                var card = new Card
                {
                    CardHolder = cardInfoDto.CardHolder,
                    CardNumber = cardInfoDto.CardNumber,
                    CheckNumber = cardInfoDto.CheckNumber,
                    Currency = currency,
                    ExpirationDate = cardInfoDto.ExpirationDate
                };

                cardService.UpdateCard(card, cardId, username);
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