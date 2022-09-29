using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.Models.ViewModels;
using MyAvana.Payments.Api.Contract;

namespace MyAvana.Payments.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly IBillingService _billingService;
        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }
        [HttpPost("AddBillingAddress")]
        [Authorize]
        public IActionResult AddBillingAddress([FromBody] BillingAddress billingAddress)
        {
            var result = _billingService.AddBillingAddress(billingAddress,HttpContext.User);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
    }
}