using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MyAvana.CRM.Api.Contract;
using MyAvana.Logger.Contract;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class StripeServices : IStripeServices
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        private readonly ILogger _logger;
        public StripeServices(IConfiguration configuration, IHostingEnvironment hostingEnvironment,ILogger logger)
        {
            _configuration = configuration;
            _env = hostingEnvironment;
            _logger = logger;
        }
		public (Subscription subscriptionDetails,bool success, string error) CancelStripeSubscription(string providerId)
		{
			try
			{
				StripeConfiguration.ApiKey = (Convert.ToBoolean(_configuration.GetSection("Payment:IsLive").Value))
													? _configuration.GetSection("Payment:KeyLive").Value : _configuration.GetSection("Payment:KeyTest").Value;

				var service = new Stripe.SubscriptionService();
				var cancelOptions = new SubscriptionCancelOptions
				{
					InvoiceNow = false,
					Prorate = false,
				};
				Subscription subscription = service.Cancel(providerId, cancelOptions);
				return (subscription, true,"");

			}
			catch (Exception Ex) {
				_logger.LogError(Ex.Message, Ex);
				return (null,false, Ex.Message);
			}

		}
    }
}
