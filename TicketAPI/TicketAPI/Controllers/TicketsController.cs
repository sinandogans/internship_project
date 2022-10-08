using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Abstract;

namespace ServiceLayer.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <summary>
        /// Gets all ticket objects from database.
        /// </summary>
        /// <returns>List of tickets</returns>
        /// <response code="200">Returns the tickets</response>
        /// <response code="500">Internal server error</response>

        [HttpGet("getall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAll()
        {
            var result = _ticketService.GetAll();
            return Ok(result);
        }


        /// <summary>
        /// Gets all matching ticket objects from database.
        /// </summary>
        /// <returns>List of tickets</returns>
        /// <response code="200">Returns the tickets</response>
        /// <response code="500">Internal server error</response>

        [HttpGet("getallbyuserid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllByUserId(Guid userId)
        {
            var result = _ticketService.GetAllByUserId(userId);
            return Ok(result);
        }





        /// <summary>
        /// Returns the single ticket object from database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ticket object</returns>
        /// <response code="200">If ok</response>
        /// <response code="404">If ticket is not exist</response>
        /// <response code="500">Internal server error</response>

        [HttpGet("getbyid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetById(Guid id)
        {
            var result = _ticketService.GetById(id);
            return Ok(result);
        }

        /// <summary>
        /// Adds the given ticket object to database.
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="userId"></param>
        /// <returns>Success message</returns>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST
        ///     {
        ///         "subject": "samplesubject",
        ///         "body": "samplebody"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Returns the added ticket object</response>
        /// <response code="400">If user is not exist or user is admin</response>
        /// <response code="500">Internal server error</response>

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Add(Ticket ticket, Guid userId)
        {
            _ticketService.Add(ticket, userId);
            return Ok("Ticket added.");
        }




        /// <summary>
        /// Deletes the ticket object from database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Success message</returns>
        /// <response code="200">Deletes the ticket</response>
        /// <response code="404">If the ticket is not exist</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Delete(Guid id)
        {
            _ticketService.Delete(id);
            return Ok("Ticket deleted.");
        }




        ///// <summary>
        ///// Delete all tickets which are created by given userid
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns>Success message</returns>
        ///// <response code="200">Deletes the tickets</response>
        ///// <response code="500">Internal server error</response>

        //[HttpPost("deleteallbyuserid")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult DeleteAllByUserId(Guid userId)
        //{
        //    _ticketService.DeleteAllByUserId(userId);
        //    return Ok("Tickets deleted");
        //}




        /// <summary>
        /// Adds answer object to ticket object's answers field.
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="userId"></param>
        /// <param name="answer"></param>
        /// <returns>Success message</returns>
        /// <response code="200">Adds answer to the ticket</response>
        /// <response code="400">If no permission to reply</response>
        /// <response code="404">If ticket is no exist</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("addanswer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddAnswer(Guid ticketId, Guid userId, [FromBody] Answer answer)
        {
            _ticketService.AddAnswer(ticketId, userId, answer);
            return Ok("Answer added.");
        }
    }
}
