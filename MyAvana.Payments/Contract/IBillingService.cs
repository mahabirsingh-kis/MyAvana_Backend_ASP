using MyAvana.Models.ViewModels;

namespace MyAvana.Payments.Api.Contract
{
    public interface IBillingService
    {
        (bool success, string error) AddBillingAddress(BillingAddress billingAddress, System.Security.Claims.ClaimsPrincipal user);
    }
}