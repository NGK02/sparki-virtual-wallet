using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VirtualWallet.Business.Exceptions;
using VirtualWallet.Business.Services;
using VirtualWallet.Business.Services.Contracts;
using VirtualWallet.DataAccess.Models;
using VirtualWallet.DTO.CardDto;

namespace VirtualWallet.Web.ApiControllers
{
    [ApiController]
    [Route("api/cards")]
    public class CardApiController : ControllerBase
    {
        private readonly ICardService cardService;
        private readonly IMapper mapper;

        public CardApiController(ICardService cardService, IMapper mapper)
        {
            this.cardService = cardService;
            this.mapper = mapper;
        }

        [HttpPost("{userId}")]
        public IActionResult AddCard([FromBody] CardDto cardDto, int userId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var card = mapper.Map<Card>(cardDto);

                cardService.AddCard(card, userId);
                return Ok(card);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{cardId}")]
        public IActionResult DeleteCard(int cardId)
        {
            try
            {
                cardService.DeleteCard(cardId);
                return NoContent();
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAllCards()
        {
            try
            {
                var cards = cardService.GetCards();

                return Ok(cards);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{cardId}")]
        public IActionResult GetCardById(int cardId)
        {
            try
            {
                var card = cardService.GetCardById(cardId);

                return Ok(card);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{cardId}")]
        public IActionResult UpdateCard([FromBody] CardDto cardDto, int cardId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var card = mapper.Map<Card>(cardDto);

                cardService.UpdateCard(card, cardId);
                return Ok(card);
            }
            catch (EntityNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}