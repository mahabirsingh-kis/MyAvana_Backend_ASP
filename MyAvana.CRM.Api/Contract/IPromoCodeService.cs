using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
	public interface IPromoCodeService
	{
		bool SavePromoCode(PromoCodeModel codeModel);
		List<PromoCodeModel> GetPromoCodes();
        bool DeletePromoCode(PromoCodeModel codeModel);
    }
}
