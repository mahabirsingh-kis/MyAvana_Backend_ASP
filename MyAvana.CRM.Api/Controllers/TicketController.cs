using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.ViewModels;

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("createticket")]
		[Authorize(AuthenticationSchemes = "TestKey")]
		public IActionResult CreateTicket([FromBody]SupportTicket supportTicket)
        {
            var (succeeded, error) = _ticketService.CreateTicket(HttpContext.User, supportTicket);
            if (succeeded) return Ok(new JsonResult("Ticket created Successfully") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("createuser")]
		[Authorize(AuthenticationSchemes = "TestKey")]
		public IActionResult CreateUser()
        {
            var (succeeded, error) = _ticketService.CreateUser(HttpContext.User);
            if (succeeded) return Ok(new JsonResult("User created Successfully") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(error) { StatusCode = (int)HttpStatusCode.BadRequest });

        }
        [HttpGet("getuserticket")]
		[Authorize(AuthenticationSchemes = "TestKey")]
		public IActionResult GetMytickets()
        {
            var (result, succeeded, error) = _ticketService.GetMytickets(HttpContext.User);
            if (succeeded) return Ok(result);
            return BadRequest(new JsonResult(error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("getallticket")]
		[Authorize(AuthenticationSchemes = "TestKey")]
		public IActionResult GetAllTicket()
        {
            var (result, succeeded, error) = _ticketService.GetAllTicket(HttpContext.User);
            if (succeeded) return Ok(result);
            return BadRequest(new JsonResult(error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("replyticket")]
		[Authorize(AuthenticationSchemes = "TestKey")]
		public IActionResult ReplyTicket(string id, [FromBody]SupportTicket supportTicket)
        {
            var (succeeded, error) = _ticketService.ReplyTicket(HttpContext.User, id, supportTicket);
            if (succeeded) return Ok(new JsonResult("Reply posted Successfully") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("closeticket")]
		[Authorize(AuthenticationSchemes = "TestKey")]
		public IActionResult CloseTicket(string id)
        {
            var (succeeded, error) = _ticketService.CloseTicket(HttpContext.User, id);
            if (succeeded) return Ok(new JsonResult("Closed ticket Successfully") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpGet("getsingleticket")]
		[Authorize(AuthenticationSchemes = "TestKey")]
		public IActionResult GetSingleTicket(string ticketId)
        {
            var (result, succeeded, error) = _ticketService.GetSingleTicket(ticketId);
            if (succeeded) return Ok(result);
            return BadRequest(new JsonResult(error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("getusersupportid")]
		[Authorize(AuthenticationSchemes = "TestKey")]
		public IActionResult GetUserSupportID()
        {
            var (result, succeeded, error) = _ticketService.GetUserSupportID(HttpContext.User);
            if (succeeded) return Ok(result);
            return BadRequest(new JsonResult(error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("saveNotes")]
		[Authorize(AuthenticationSchemes = "TestKey")]
		public IActionResult saveNotes(string message)
        {
            var result = _ticketService.saveNotes(message, HttpContext.User);
            if (result.success) return Ok(new JsonResult(result.success) { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

    }
}