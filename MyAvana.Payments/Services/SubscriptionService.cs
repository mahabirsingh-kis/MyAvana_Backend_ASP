using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvana.Logger.Contract;
using MyAvana.Payments.Api.Contract;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.Payments.Api.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IConfiguration _configuration;
        private readonly AvanaContext _context;
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;
        public SubscriptionService(IConfiguration configuration, AvanaContext context, ILogger logger, ITokenService tokenService)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
            _tokenService = tokenService;
        }

        public (JsonResult result, bool success, string error) GetActiveSubscription()
        {
            try
            {
                var result = _context.SubscriptionsEntities.Where(s => s.Active).ToList();
                if (result.Count > 0)
                    return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                return (new JsonResult(""), false, "");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again.");
            }
        }

        public (JsonResult result, bool success, string error) GetMySubscription(ClaimsPrincipal user)
        {
            try
            {
                var account = _tokenService.GetAccountNo(user);
                if (account != null)
                {
                    var mySubscription = _context.PaymentEntities.FirstOrDefault(s => s.EmailAddress == account.Email);
                    if (mySubscription != null)
                    {
                        var subscription = _context.SubscriptionsEntities.FirstOrDefault(s => s.StripePlanId == mySubscription.SubscriptionId);
                        return (new JsonResult(subscription), true, "");
                    }
                    return (new JsonResult(""), false, "User doesn't have any active subscription.");
                }
                return (new JsonResult(""), false, "Invalid user.");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again.");
            }
        }

        public (bool success, string error) StripeWebhook(Stripe.Event stripeEvent)
        {
            try
            {
                _logger.LogError(JsonConvert.SerializeObject(stripeEvent));

                switch (stripeEvent.Type)
                {
                    case "customer.created":
                        var customer = stripeEvent.Data.Object as Customer;
                        // do work

                        break;

                    case "customer.subscription.created":
                    case "customer.subscription.updated":
                    case "customer.subscription.deleted":
                    case "customer.subscription.trial_will_end":
                        var subscription = stripeEvent.Data.Object as Subscription;
                        // do work

                        break;

                    case "invoice.created":
                        var newinvoice = stripeEvent.Data.Object as Invoice;
                        // do work

                        break;

                    case "invoice.upcoming":
                    case "invoice.payment_succeeded":
                    case "invoice.payment_failed":
                        var invoice = stripeEvent.Data.Object as Invoice;
                        // do work

                        break;

                    case "coupon.created":
                    case "coupon.updated":
                    case "coupon.deleted":
                        var coupon = stripeEvent.Data.Object as Coupon;
                        // do work

                        break;

                        // DO SAME FOR OTHER EVENTS
                }

                return (true, "");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (false, "Something went wrong. Please try again.");
            }
        }
    }
}
