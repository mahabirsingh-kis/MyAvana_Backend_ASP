using MyAvanaApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.Payments.Api.Contract
{
    public interface IPaymentServices
    {
        (bool success, string error) Checkout(CheckoutRequest checkout, ClaimsPrincipal user);

        (bool success, string error) GetPaymentStatus(ClaimsPrincipal user);
        (bool success, string error) SaveAppleResponse(AppleRequest appleRequest, ClaimsPrincipal user);
		(bool success, string error) SavePromoCodeSubscription(PromoCodeSubscription appleRequest, ClaimsPrincipal user);
		(bool success, string error) CancelStripeSubscription(ClaimsPrincipal user);
        (bool success, string error) CardPayment(CheckoutRequest checkout);
    }
}
