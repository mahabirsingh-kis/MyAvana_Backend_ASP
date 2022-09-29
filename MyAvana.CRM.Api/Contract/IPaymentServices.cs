using MyAvanaApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IPaymentServices
    {
		(bool success, string error) CancelStripeSubscription(string email);
	}
}
