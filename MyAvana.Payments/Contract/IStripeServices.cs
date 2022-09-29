using MyAvanaApi.Models.ViewModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.Payments.Api.Contract
{
    public interface IStripeServices
    {
        (Subscription charge, string error) StripPayments(CheckoutRequest checkout, MyAvanaApi.Models.Entities.UserEntity accountNo);
        bool CreateSubscription();
		(Subscription subscriptionDetails, bool success, string error) CancelStripeSubscription(string ProviderId);
	}
}
