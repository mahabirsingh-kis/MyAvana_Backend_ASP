using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
	public interface IRegimenService
	{
		RegimensModel SaveRegimens(RegimensModel regimensModel);
		List<RegimensModel> GetRegimens();
		bool DeleteRegimens(Regimens regimens);
		RegimensModel GetRegimensById(RegimensModel regimens);
	}
}
