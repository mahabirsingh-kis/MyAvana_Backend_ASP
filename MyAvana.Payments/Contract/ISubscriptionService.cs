using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.Payments.Api.Contract
{
    public interface ISubscriptionService
    {
        (JsonResult result, bool success, string error) GetActiveSubscription();
        (bool success, string error) StripeWebhook(Stripe.Event stripeEvent);
        (JsonResult result, bool success, string error) GetMySubscription(ClaimsPrincipal user);
    }
}
