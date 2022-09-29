using HubSpot.NET.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvana.Logger.Contract;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;

namespace MyAvana.CRM.Api.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IHostingEnvironment _env;
        public readonly AvanaContext _context;
        private readonly IStripeServices _stripe;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;
        private readonly HubSpotApi _hubSpotApi;
        public PaymentServices(IHostingEnvironment hostingEnvironment, AvanaContext avanaContext,
                               IConfiguration configuration, IStripeServices stripe, ITokenService tokenService,
                               ILogger logger)
        {
            _context = avanaContext;
            _configuration = configuration;
            _stripe = stripe;
            _env = hostingEnvironment;
            _tokenService = tokenService;
            _logger = logger;
            _hubSpotApi = new HubSpotApi(_configuration.GetSection("Hubspot:Key").Value);
        }

        public (bool success, string error) CancelStripeSubscription(string email)
        {
            try
            {
                if (email != null)
                {

                    var payments = _context.PaymentEntities.Where(x => x.EmailAddress == email).FirstOrDefault();
                    var paymentResponse = _stripe.CancelStripeSubscription(payments.ProviderId);
                    if (paymentResponse.success)
                    {
                        var planName = _context.SubscriptionsEntities.Where(x => x.StripePlanId == payments.ProviderId).Select(x => x.PlanName).FirstOrDefault();
                        return (true, "You are successfully unsubscribed from : " + planName);
                    }
                    else
                    {
                        return (false, paymentResponse.error);
                    }
                }
                return (false, "Something went wrong!");
            }
            catch (Exception ex) { return (false, "Something went wrong!"); }
        }
    }
}
