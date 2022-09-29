using HubSpot.NET.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvana.Logger.Contract;
using MyAvana.Payments.Api.Contract;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;

namespace MyAvana.Payments.Api.Services
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
                               IConfiguration configuration, IStripeServices stripe,ITokenService tokenService,
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
        public (bool success, string error) Checkout(CheckoutRequest checkout, ClaimsPrincipal user)
        {
            try
            {
                var subscription = _context.SubscriptionsEntities.Where(s => s.StripePlanId.Trim() == checkout.SubscriptionId.Trim()).FirstOrDefault();
                if (subscription != null)
                {
                    var accountNo = _tokenService.GetAccountNo(user);
                    if (accountNo != null)
                    {
                        checkout.Amount = subscription.Amount;
                        var paymentResponse = _stripe.StripPayments(checkout, accountNo);
                        if (paymentResponse.charge != null)
                        {
                            _context.PaymentEntities.Add(new PaymentEntity()
                            {
                                EmailAddress = accountNo.Email,
                                CCNumber = checkout.CardNumber,
                                CreatedDate = DateTime.UtcNow,
                                PaymentAmount = subscription.Amount.ToString(),
                                PaymentId = Guid.NewGuid(),
                                SubscriptionId = checkout.SubscriptionId.ToString(),
                                ProviderId = paymentResponse.charge.Id,
                                ProviderName = "STRIPE"
                            });
                            accountNo.StripeCustomerId = paymentResponse.charge.CustomerId;
                            accountNo.Country = checkout.Country;
                            accountNo.State = checkout.State;
                            accountNo.ZipCode = checkout.Zipcode;
                            accountNo.Address = checkout.Address;
                            _context.SaveChanges();
                            return (true, "");
                        }
                        return (false, "Error in processing payments." + "Error :- " + paymentResponse.error);

                    }
                    return (false, "Invalid User");
                }
                return (false, "Invalid subscription Id");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (false, Ex.Message);
            }
        }
        public (bool success, string error) SaveAppleResponse(AppleRequest appleRequest, ClaimsPrincipal user)
        {
            try
            {
                var accountNo = _tokenService.GetAccountNo(user);
                if (accountNo != null)
                {
                    _context.PaymentEntities.Add(new PaymentEntity()
                    {
                        EmailAddress = accountNo.Email,
                        CCNumber = "",
                        CreatedDate = DateTime.UtcNow,
                        PaymentAmount = "",
                        PaymentId = Guid.NewGuid(),
                        SubscriptionId = appleRequest.SubscriptionId,
                        ProviderId = appleRequest.TransactionID,
                        ProviderName = "APPLE"
                    });
                    _context.SaveChanges();
                    return (true, "");
                }
                return (false, "Invalid User.");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (false, "Something went wrong. Please try again later.");
            }
        }

		public (bool success, string error) SavePromoCodeSubscription(PromoCodeSubscription codeSubscription, ClaimsPrincipal user)
		{
			try
			{
				var accountNo = _tokenService.GetAccountNo(user);
				
				if (accountNo != null)
				{
					var response = _context.PromoCodes.Where(x => x.Code == codeSubscription.PromoCode).FirstOrDefault();
					if (response.ExpireDate >= DateTime.UtcNow)
					{
						var subscribe = _context.PaymentEntities.Where(x => x.SubscriptionId == codeSubscription.PromoCode && x.EmailAddress == accountNo.Email).FirstOrDefault();
						if (subscribe == null)
						{
							_context.PaymentEntities.Add(new PaymentEntity()
							{
								EmailAddress = accountNo.Email,
								CCNumber = "",
								CreatedDate = DateTime.UtcNow,
								PaymentAmount = "",
								PaymentId = Guid.NewGuid(),
								SubscriptionId = response.StripePlanId,
								ProviderId = null,
								ProviderName = "PROMOCODE"
							});
							_context.SaveChanges();
							return (true, "");
						}
						return (false, "Code already applied.");
					}
					return (false, "Invalid PromoCode.");
				}
				return (false, "Invalid User.");
			}
			catch (Exception Ex)
			{
				_logger.LogError(Ex.Message, Ex);
				return (false, "Something went wrong. Please try again later.");
			}
		}

		public (bool success, string error) GetPaymentStatus(ClaimsPrincipal user)
        {
            try
            {
                var accountNo = _tokenService.GetAccountNo(user);
                if (accountNo != null)
                {
                   // if (!_context.PaymentEntities.Where(s => s.EmailAddress.ToLower() == accountNo.Email.ToLower()).Any())
                     //   return (true, "");
                    return (false, "");
                }
                return (false, "Invalid user.");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (false, "Something went wrong. Please try again.");
            }
        }

		 public (bool success, string error) CancelStripeSubscription(ClaimsPrincipal user)
		{
			try
			{
				var userDetail = _tokenService.GetAccountNo(user);
				if (userDetail != null)
				{
					var payments = _context.PaymentEntities.Where(x => x.EmailAddress == userDetail.Email).FirstOrDefault();
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

        public (bool success, string error) CardPayment(CheckoutRequest checkout)
        {
            try
            {
                //Guid uId = new Guid(checkout.UserId);
                UserEntity accountNo = null; // _context.Users.Where(x => x.Id == uId).FirstOrDefault();
                if(accountNo != null)
                { 
                        var paymentResponse = _stripe.StripPayments(checkout, accountNo);
                if (paymentResponse.charge != null)
                {
                    _context.PaymentEntities.Add(new PaymentEntity()
                    {
                        EmailAddress = accountNo.Email,
                        CCNumber = checkout.CardNumber,
                        CreatedDate = DateTime.UtcNow,
                        PaymentAmount = checkout.Amount.ToString(),
                        PaymentId = Guid.NewGuid(),
                        SubscriptionId = "plan_GoYoNBwX2tAFiQ",
                        ProviderId = paymentResponse.charge.Id,
                        ProviderName = "STRIPE"
                    });
                    _context.SaveChanges();
                    return (true, "");
                }
                return (false, "Error in processing payments." + "Error :- " + paymentResponse.error);

                }
                return (false, "Invalid User");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (false, Ex.Message);
            }
        }
    }
}
