using Microsoft.AspNetCore.Mvc;
using BasitDestekTalebiSistemi.Business.Abstract;
using BasitDestekTalebiSistemi.Entities.DTOs;

namespace BasitDestekTalebiSistemi.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {

        private readonly ITicketService _ticketService;

        private readonly IReplyService _replyService;

        public TicketsController(ITicketService ticketService, IReplyService replyService)
        {
            _ticketService = ticketService;
            _replyService = replyService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateTicketRequest request)
        {
            var result = _ticketService.Create(request);

            if (!result.Success)
            {

                return BadRequest(new { success = false, message = result.Message });
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, new
            {
                success = true,
                message = result.Message,
                data = result.Data
            });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _ticketService.GetAll();

            return Ok(new
            {
                success = result.Success,
                message = result.Message,
                data = result.Data
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var result = _ticketService.GetById(id);

            if (!result.Success)
            {

                return NotFound(new { success = false, message = result.Message });
            }

            return Ok(new
            {
                success = true,
                message = result.Message,
                data = result.Data
            });
        }

        [HttpPut("{id}/close")]
        public IActionResult Close([FromRoute] int id)
        {
            var result = _ticketService.CloseTicket(id);

            if (!result.Success)
            {

                if (result.Message != null && result.Message.Contains("bulunamadı"))
                {
                    return NotFound(new { success = false, message = result.Message });
                }
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new
            {
                success = true,
                message = result.Message
            });
        }

        [HttpGet("{id}/replies")]
        public IActionResult GetReplies([FromRoute] int id)
        {
            var result = _replyService.GetRepliesByTicketId(id);

            if (!result.Success)
            {
                return NotFound(new { success = false, message = result.Message });
            }

            return Ok(new
            {
                success = true,
                message = result.Message,
                data = result.Data
            });
        }

        [HttpPost("{id}/replies")]
        public IActionResult AddReply([FromRoute] int id, [FromBody] CreateReplyRequest request)
        {
            var result = _replyService.AddReply(id, request);

            if (!result.Success)
            {

                if (result.Message != null && result.Message.Contains("bulunamadı"))
                {
                    return NotFound(new { success = false, message = result.Message });
                }
                return BadRequest(new { success = false, message = result.Message });
            }

            return CreatedAtAction(nameof(GetReplies), new { id }, new
            {
                success = true,
                message = result.Message,
                data = result.Data
            });
        }
    }
}
