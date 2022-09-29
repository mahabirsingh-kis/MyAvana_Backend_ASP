using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyAvana.Logger.Contract;
using MyAvana.Payments.Api.Contract;
using Stripe;

namespace MyAvana.Payments.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscription;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public SubscriptionController(ISubscriptionService subscription, IConfiguration configuration,ILogger logger)
        {
            _subscription = subscription;
            _configuration = configuration;
            _logger = logger;
        }
        [HttpGet("GetSubscriptions")]
        public IActionResult GetSubscriptions()
        {
            var result = _subscription.GetActiveSubscription();
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("webhook")]
        public IActionResult StripeWebhook()
        {
            var json = new StreamReader(HttpContext.Request.Body).ReadToEnd();
            _logger.LogError(json);
            _logger.LogError(Request.Headers["Stripe-Signature"].ToString());
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _configuration.GetSection("Stripe:Token").Value);
            var result = _subscription.StripeWebhook(stripeEvent);
            if (result.success) return Ok();
            return BadRequest(result.error);
        }
        [HttpGet("GetMySubscription")]
        [Authorize]
        public IActionResult GetMySubscription()
        {
            var result = _subscription.GetMySubscription(HttpContext.User);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
    }
}