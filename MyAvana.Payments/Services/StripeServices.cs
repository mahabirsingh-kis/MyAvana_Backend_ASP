using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MyAvana.Logger.Contract;
using MyAvana.Payments.Api.Contract;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.Payments.Api.Services
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
        public (Subscription charge, string error) StripPayments(CheckoutRequest checkout, UserEntity userEntity)
        {
            try
            {
                //StripeConfiguration.ApiKey = (Convert.ToBoolean(_configuration.GetSection("Payment:IsLive").Value)) 
                //                                    ? _configuration.GetSection("Payment:KeyLive").Value : _configuration.GetSection("Payment:KeyTest").Value;
                StripeConfiguration.ApiKey = _configuration.GetSection("Payment:KeyTest").Value;

               //Create Card Object to create Token  
                CreditCardOptions card = new Stripe.CreditCardOptions();
                card.Name = checkout.CardOwnerFirstName + " " + checkout.CardOwnerLastName;
                card.Number = checkout.CardNumber;
                card.ExpYear = checkout.ExpirationYear;
                card.ExpMonth = checkout.ExpirationMonth;
                card.Cvc = checkout.CVV2;

                //Assign Card to Token Object and create Token  
                TokenCreateOptions token = new TokenCreateOptions
                {
                    Card = card
                };
                Stripe.TokenService serviceToken = new Stripe.TokenService();
                Token newToken = serviceToken.Create(token);

                //Create Customer Object and Register it on Stripe  
                CustomerCreateOptions myCustomer = new CustomerCreateOptions
                {
                    Email = userEntity.Email,
                    Source = newToken.Id,
                    Name = checkout.CardOwnerFirstName + " " + checkout.CardOwnerLastName,
                    Address = new AddressOptions() { State = checkout.State, Country = checkout.Country, PostalCode = checkout.Zipcode, Line1 = checkout.Address }

                };
                var customerService = new CustomerService();
                Customer stripeCustomer = customerService.Create(myCustomer);

                var items = new List<SubscriptionItemOption> {
                                new SubscriptionItemOption {PlanId = checkout.SubscriptionId}
                            };
                var suboptions = new SubscriptionCreateOptions
                {
                    CustomerId = stripeCustomer.Id,
                    Items = items,
                    TrialPeriodDays = 7
                };

                var subservice = new Stripe.SubscriptionService();
                Subscription subscription = subservice.Create(suboptions);

                return (subscription, "");

                ////Create Charge Object with details of Charge  
                //var options = new ChargeCreateOptions
                //{
                //    Amount = Convert.ToInt32(checkout.Amount) * 100,
                //    Currency = "USD",
                //    ReceiptEmail = userEntity.Email,
                //    CustomerId = stripeCustomer.Id,
                //    Description = GetDescription(checkout), //Optional 

                //};

                ////and Create Method of this object is doing the payment execution.  
                //var service = new ChargeService();
                //Charge charge = service.Create(options); // This will do the Payment 
                //return (charge, "");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (null, Ex.Message);
            }
        }
        public bool CreateSubscription()
        {
            try
            {
                StripeConfiguration.ApiKey = (Convert.ToBoolean(_configuration.GetSection("Payment:IsLive").Value))
                                                    ? _configuration.GetSection("Payment:KeyLive").Value : _configuration.GetSection("Payment:KeyTest").Value;

                var productoption = new ProductCreateOptions
                {
                    Name = "MyAvana Developer",
                    Type = "service",
                };
                var productService = new Stripe.ProductService();
                Product product = productService.Create(productoption);


                var options = new PlanCreateOptions
                {
                    Product = product.Id,
                    Nickname = "Your Hair Is As Unique As Your Fingerprint",
                    Active = true,
                    Interval = "year",
                    Currency = "usd",
                    Amount = 10000,

                };
                var service = new PlanService();
                Plan plan = service.Create(options);
                if (plan != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return false;
            }
        }
        private string GetDescription(CheckoutRequest checkout)
        {
            return "Payment completed with CC No. :- " + checkout.CardNumber + " for amount of " + checkout.Amount;
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
