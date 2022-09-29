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
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace MyAvana.Payments.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentServices _paymentServices;
        public PaymentsController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }

        [HttpPost("Checkout")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Authorize]
        public IActionResult Checkout([FromBody] CheckoutRequest checkoutRequest)
        {
            var result = _paymentServices.Checkout(checkoutRequest, HttpContext.User);
            if (result.success) return Ok(new JsonResult("Payment done succesfully.") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpGet("GetPaymentStatus")]
        [Authorize]
        public IActionResult GetPaymentStatus()
        {
            var result = _paymentServices.GetPaymentStatus(HttpContext.User);
            if (result.success) return Ok(new JsonResult(result.success) { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.success) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

		[HttpGet("CancelStripeSubscription")]
		[Authorize]
		public IActionResult CancelStripeSubscription()
		{
			var result = _paymentServices.CancelStripeSubscription(HttpContext.User);
			if (result.success) return Ok(result);
			return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
		}

		[HttpPost("SaveAppleResponse")]
        [Authorize]
        public IActionResult SaveAppleResponse([FromBody]AppleRequest appleRequest)
        {
            var result = _paymentServices.SaveAppleResponse(appleRequest, HttpContext.User);
            if (result.success) return Ok(new JsonResult(result.success) { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.success) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

		[HttpPost("SavePromoCodeSubscription")]
		[Authorize]
		public IActionResult SavePromoCodeSubscription([FromBody]PromoCodeSubscription promoCodeSubscription)
		{
			var result = _paymentServices.SavePromoCodeSubscription(promoCodeSubscription, HttpContext.User);
			if (result.success) return Ok(new JsonResult(result.success) { StatusCode = (int)HttpStatusCode.OK });
			return BadRequest(new JsonResult(result.success) { StatusCode = (int)HttpStatusCode.BadRequest });
		}

        [HttpPost("CardPayment")]
        [Authorize]
        public IActionResult CardPayment([FromBody] CheckoutRequest checkoutRequest)
        {
            var result = _paymentServices.Checkout(checkoutRequest, HttpContext.User);
            if (result.success) return Ok(new JsonResult("Payment done succesfully.") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

    }
}