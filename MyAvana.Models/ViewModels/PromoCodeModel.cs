using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyAvanaApi.Models.ViewModels
{
	public class PromoCodeModel
	{
		public string PromoCode { get; set; }
		public DateTime? ExpireDate { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string StripePlanId { get; set; }
		public bool IsActive { get; set; }

	}

	public class PromoCodeValidationModel
	{
		public string PromoCode { get; set; }
		public DateTime ExpireDate { get; set; }
	}
	public class PromoCodeSubscription
	{
		public string PromoCode { get; set; }
		public string Token { get; set; }
	}
	
}
