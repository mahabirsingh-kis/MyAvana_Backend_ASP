using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribersController : Controller
    {

        private readonly ISubscriberService _SusbcriberService;
        private readonly IPaymentServices _paymentService;
        private readonly IBaseBusiness _baseBusiness;
        public SubscribersController(ISubscriberService SubscribersService, IBaseBusiness baseBusiness, IPaymentServices paymentService)
        {
            _SusbcriberService = SubscribersService;
            _baseBusiness = baseBusiness;
            _paymentService = paymentService;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}
        [HttpGet("GetSubscriberList")]
        public JObject GetSubscriberList()
        {
            List<SubscriberModel> result = _SusbcriberService.GetSubscriberList();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost]
        [Route("CancelSubscription")]
        public JObject CancelSubscription(SubscriberModel subscriber)
        {
            (bool cancelStripeResult, string error) = _paymentService.CancelStripeSubscription(subscriber.UserEmail);
            if (cancelStripeResult && error == null) _SusbcriberService.CancelSubscription(subscriber);
            if (cancelStripeResult)
                return _baseBusiness.AddDataOnJson("Success", "1", subscriber);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }


    }
}
