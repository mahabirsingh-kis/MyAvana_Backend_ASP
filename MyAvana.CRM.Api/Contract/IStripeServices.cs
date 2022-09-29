using MyAvanaApi.Models.ViewModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IStripeServices
    {
		(Subscription subscriptionDetails, bool success, string error) CancelStripeSubscription(string ProviderId);
	}
}
