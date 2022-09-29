using Microsoft.EntityFrameworkCore;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
	public class RegimenService : IRegimenService
	{
		private readonly AvanaContext _context;
		public RegimenService(AvanaContext avanaContext)
		{
			_context = avanaContext;
		}

		public RegimensModel SaveRegimens(RegimensModel regimensModel)
		{
			if (regimensModel.RegimensId != 0)
			{
				var objRegimens = _context.Regimens.Include(x => x.RegimenSteps).Where(x => x.RegimensId == regimensModel.RegimensId).FirstOrDefault();

				objRegimens.Name = regimensModel.Name;
				objRegimens.Title = regimensModel.Title;
				objRegimens.Description = regimensModel.Description;
				objRegimens.RegimenSteps.Step1Instruction = regimensModel.Step1Instruction == null ? objRegimens.RegimenSteps.Step1Instruction : regimensModel.Step1Instruction;
				objRegimens.RegimenSteps.Step1Photo = regimensModel.Step1PhotoName == null ? objRegimens.RegimenSteps.Step1Photo : regimensModel.Step1PhotoName;
				objRegimens.RegimenSteps.Step2Instruction = regimensModel.Step2Instruction == null ? objRegimens.RegimenSteps.Step2Instruction : regimensModel.Step2Instruction;
				objRegimens.RegimenSteps.Step2Photo = regimensModel.Step2PhotoName == null ? objRegimens.RegimenSteps.Step2Photo : regimensModel.Step2PhotoName;

				objRegimens.RegimenSteps.Step3Instruction = regimensModel.Step3Instruction == null ? objRegimens.RegimenSteps.Step3Instruction : regimensModel.Step3Instruction;
				objRegimens.RegimenSteps.Step3Photo = regimensModel.Step3PhotoName == null ? objRegimens.RegimenSteps.Step3Photo : regimensModel.Step3PhotoName;

				objRegimens.RegimenSteps.Step4Instruction = regimensModel.Step4Instruction == null ? objRegimens.RegimenSteps.Step4Instruction : regimensModel.Step4Instruction;
				objRegimens.RegimenSteps.Step4Photo = regimensModel.Step4PhotoName == null ? objRegimens.RegimenSteps.Step4Photo : regimensModel.Step4PhotoName;

				objRegimens.RegimenSteps.Step5Instruction = regimensModel.Step5Instruction == null ? objRegimens.RegimenSteps.Step5Instruction : regimensModel.Step5Instruction;
				objRegimens.RegimenSteps.Step5Photo = regimensModel.Step5PhotoName == null ? objRegimens.RegimenSteps.Step5Photo : regimensModel.Step5PhotoName;

				objRegimens.RegimenSteps.Step6Instruction = regimensModel.Step6Instruction == null ? objRegimens.RegimenSteps.Step6Instruction : regimensModel.Step6Instruction;
				objRegimens.RegimenSteps.Step6Photo = regimensModel.Step6PhotoName == null ? objRegimens.RegimenSteps.Step6Photo : regimensModel.Step6PhotoName;

				objRegimens.RegimenSteps.Step7Instruction = regimensModel.Step7Instruction == null ? objRegimens.RegimenSteps.Step7Instruction : regimensModel.Step7Instruction;
				objRegimens.RegimenSteps.Step7Photo = regimensModel.Step7PhotoName == null ? objRegimens.RegimenSteps.Step7Photo : regimensModel.Step7PhotoName;

				objRegimens.RegimenSteps.Step8Instruction = regimensModel.Step8Instruction == null ? objRegimens.RegimenSteps.Step8Instruction : regimensModel.Step8Instruction;
				objRegimens.RegimenSteps.Step8Photo = regimensModel.Step8PhotoName == null ? objRegimens.RegimenSteps.Step8Photo : regimensModel.Step8PhotoName;

				objRegimens.RegimenSteps.Step9Instruction = regimensModel.Step9Instruction == null ? objRegimens.RegimenSteps.Step9Instruction : regimensModel.Step9Instruction;
				objRegimens.RegimenSteps.Step9Photo = regimensModel.Step9PhotoName == null ? objRegimens.RegimenSteps.Step9Photo : regimensModel.Step9PhotoName;

				objRegimens.RegimenSteps.Step10Instruction = regimensModel.Step10Instruction == null ? objRegimens.RegimenSteps.Step10Instruction : regimensModel.Step10Instruction;
				objRegimens.RegimenSteps.Step10Photo = regimensModel.Step10PhotoName == null ? objRegimens.RegimenSteps.Step10Photo : regimensModel.Step10PhotoName;

				objRegimens.RegimenSteps.Step11Instruction = regimensModel.Step11Instruction == null ? objRegimens.RegimenSteps.Step11Instruction : regimensModel.Step11Instruction;
				objRegimens.RegimenSteps.Step11Photo = regimensModel.Step11PhotoName == null ? objRegimens.RegimenSteps.Step11Photo : regimensModel.Step11PhotoName;

				objRegimens.RegimenSteps.Step12Instruction = regimensModel.Step12Instruction == null ? objRegimens.RegimenSteps.Step12Instruction : regimensModel.Step12Instruction;
				objRegimens.RegimenSteps.Step12Photo = regimensModel.Step12PhotoName == null ? objRegimens.RegimenSteps.Step12Photo : regimensModel.Step12PhotoName;

				objRegimens.RegimenSteps.Step13Instruction = regimensModel.Step13Instruction == null ? objRegimens.RegimenSteps.Step13Instruction : regimensModel.Step13Instruction;
				objRegimens.RegimenSteps.Step13Photo = regimensModel.Step13PhotoName == null ? objRegimens.RegimenSteps.Step13Photo : regimensModel.Step13PhotoName;

				objRegimens.RegimenSteps.Step14Instruction = regimensModel.Step14Instruction == null ? objRegimens.RegimenSteps.Step14Instruction : regimensModel.Step14Instruction;
				objRegimens.RegimenSteps.Step14Photo = regimensModel.Step14PhotoName == null ? objRegimens.RegimenSteps.Step14Photo : regimensModel.Step15PhotoName;

				objRegimens.RegimenSteps.Step15Instruction = regimensModel.Step15Instruction == null ? objRegimens.RegimenSteps.Step15Instruction : regimensModel.Step15Instruction;
				objRegimens.RegimenSteps.Step15Photo = regimensModel.Step15PhotoName == null ? objRegimens.RegimenSteps.Step15Photo : regimensModel.Step15PhotoName;

				objRegimens.RegimenSteps.Step16Instruction = regimensModel.Step16Instruction == null ? objRegimens.RegimenSteps.Step16Instruction : regimensModel.Step16Instruction;
				objRegimens.RegimenSteps.Step16Photo = regimensModel.Step16PhotoName == null ? objRegimens.RegimenSteps.Step16Photo : regimensModel.Step16PhotoName;

				objRegimens.RegimenSteps.Step17Instruction = regimensModel.Step17Instruction == null ? objRegimens.RegimenSteps.Step17Instruction : regimensModel.Step17Instruction;
				objRegimens.RegimenSteps.Step17Photo = regimensModel.Step17PhotoName == null ? objRegimens.RegimenSteps.Step17Photo : regimensModel.Step17PhotoName;

				objRegimens.RegimenSteps.Step18Instruction = regimensModel.Step18Instruction == null ? objRegimens.RegimenSteps.Step18Instruction : regimensModel.Step18Instruction;
				objRegimens.RegimenSteps.Step18Photo = regimensModel.Step18PhotoName == null ? objRegimens.RegimenSteps.Step18Photo : regimensModel.Step18PhotoName;

				objRegimens.RegimenSteps.Step19Instruction = regimensModel.Step19Instruction == null ? objRegimens.RegimenSteps.Step19Instruction : regimensModel.Step19Instruction;
				objRegimens.RegimenSteps.Step19Photo = regimensModel.Step19PhotoName == null ? objRegimens.RegimenSteps.Step19Photo : regimensModel.Step19PhotoName;

				objRegimens.RegimenSteps.Step20Instruction = regimensModel.Step20Instruction == null ? objRegimens.RegimenSteps.Step20Instruction : regimensModel.Step20Instruction;
				objRegimens.RegimenSteps.Step20Photo = regimensModel.Step20PhotoName == null ? objRegimens.RegimenSteps.Step20Photo : regimensModel.Step20PhotoName;
				_context.SaveChanges();
			}
			else
			{
				RegimenSteps regimenSteps = new RegimenSteps();
				regimenSteps.Step1Instruction = regimensModel.Step1Instruction;
				regimenSteps.Step1Photo = regimensModel.Step1PhotoName;
				regimenSteps.Step2Instruction = regimensModel.Step2Instruction;
				regimenSteps.Step2Photo = regimensModel.Step2PhotoName;
				regimenSteps.Step3Instruction = regimensModel.Step3Instruction;
				regimenSteps.Step3Photo = regimensModel.Step3PhotoName;
				regimenSteps.Step4Instruction = regimensModel.Step4Instruction;
				regimenSteps.Step4Photo = regimensModel.Step4PhotoName;
				regimenSteps.Step5Instruction = regimensModel.Step5Instruction;
				regimenSteps.Step5Photo = regimensModel.Step5PhotoName;

				regimenSteps.Step6Instruction = regimensModel.Step6Instruction;
				regimenSteps.Step6Photo = regimensModel.Step6PhotoName;
				regimenSteps.Step7Instruction = regimensModel.Step7Instruction;
				regimenSteps.Step7Photo = regimensModel.Step7PhotoName;
				regimenSteps.Step8Instruction = regimensModel.Step8Instruction;
				regimenSteps.Step8Photo = regimensModel.Step8PhotoName;
				regimenSteps.Step9Instruction = regimensModel.Step9Instruction;
				regimenSteps.Step9Photo = regimensModel.Step9PhotoName;
				regimenSteps.Step10Instruction = regimensModel.Step10Instruction;
				regimenSteps.Step10Photo = regimensModel.Step10PhotoName;
				regimenSteps.Step11Instruction = regimensModel.Step11Instruction;
				regimenSteps.Step11Photo = regimensModel.Step11PhotoName;
				regimenSteps.Step12Instruction = regimensModel.Step12Instruction;
				regimenSteps.Step12Photo = regimensModel.Step12PhotoName;
				regimenSteps.Step13Instruction = regimensModel.Step13Instruction;
				regimenSteps.Step13Photo = regimensModel.Step13PhotoName;
				regimenSteps.Step14Instruction = regimensModel.Step14Instruction;
				regimenSteps.Step14Photo = regimensModel.Step14PhotoName;
				regimenSteps.Step15Instruction = regimensModel.Step15Instruction;
				regimenSteps.Step15Photo = regimensModel.Step15PhotoName;
				regimenSteps.Step16Instruction = regimensModel.Step16Instruction;
				regimenSteps.Step16Photo = regimensModel.Step16PhotoName;
				regimenSteps.Step17Instruction = regimensModel.Step17Instruction;
				regimenSteps.Step17Photo = regimensModel.Step17PhotoName;
				regimenSteps.Step18Instruction = regimensModel.Step18Instruction;
				regimenSteps.Step18Photo = regimensModel.Step18PhotoName;
				regimenSteps.Step19Instruction = regimensModel.Step19Instruction;
				regimenSteps.Step19Photo = regimensModel.Step19PhotoName;
				regimenSteps.Step20Instruction = regimensModel.Step20Instruction;
				regimenSteps.Step20Photo = regimensModel.Step20PhotoName;
				regimenSteps.IsActive = true;
				regimenSteps.CreatedOn = DateTime.Now;
				_context.Add(regimenSteps);
				_context.SaveChanges();

				Regimens regimens = new Regimens();
				regimens.Name = regimensModel.Name;
				regimens.Title = regimensModel.Title;
				regimens.Description = regimensModel.Description;
				regimens.IsActive = true;
				regimens.CreatedOn = DateTime.Now;
				regimens.RegimenStepsId = regimenSteps.RegimenStepsId;

				_context.Add(regimens);
				_context.SaveChanges();
			}
			return regimensModel;
		}

		public List<RegimensModel> GetRegimens()
		{
			List<RegimensModel> regimensModels = new List<RegimensModel>();
			regimensModels = _context.Regimens.Include(x => x.RegimenSteps).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn)
				.Select(e => new RegimensModel()
				{
					RegimensId = e.RegimensId,
					Name = e.Name,
					Title = e.Title,
					Description = e.Description,
					RegimenStepsId = e.RegimenSteps.RegimenStepsId,
					Step1Instruction = e.RegimenSteps.Step1Instruction,
					Step1PhotoName = e.RegimenSteps.Step1Photo,
					Step2Instruction = e.RegimenSteps.Step2Instruction,
					Step2PhotoName = e.RegimenSteps.Step2Photo,
					Step3Instruction = e.RegimenSteps.Step3Instruction,
					Step3PhotoName = e.RegimenSteps.Step3Photo,
					Step4Instruction = e.RegimenSteps.Step4Instruction,
					Step4PhotoName = e.RegimenSteps.Step4Photo,
					Step5Instruction = e.RegimenSteps.Step5Instruction,
					Step5PhotoName = e.RegimenSteps.Step5Photo,
					Step6Instruction = e.RegimenSteps.Step6Instruction,
					Step6PhotoName = e.RegimenSteps.Step6Photo,
					Step7Instruction = e.RegimenSteps.Step7Instruction,
					Step7PhotoName = e.RegimenSteps.Step7Photo,
					Step8Instruction = e.RegimenSteps.Step8Instruction,
					Step8PhotoName = e.RegimenSteps.Step8Photo,
					Step9Instruction = e.RegimenSteps.Step9Instruction,
					Step9PhotoName = e.RegimenSteps.Step9Photo,
					Step10Instruction = e.RegimenSteps.Step10Instruction,
					Step10PhotoName = e.RegimenSteps.Step10Photo,
					Step11Instruction = e.RegimenSteps.Step11Instruction,
					Step11PhotoName = e.RegimenSteps.Step11Photo,
					Step12Instruction = e.RegimenSteps.Step12Instruction,
					Step12PhotoName = e.RegimenSteps.Step12Photo,
					Step13Instruction = e.RegimenSteps.Step13Instruction,
					Step13PhotoName = e.RegimenSteps.Step13Photo,
					Step14Instruction = e.RegimenSteps.Step14Instruction,
					Step14PhotoName = e.RegimenSteps.Step14Photo,
					Step15Instruction = e.RegimenSteps.Step15Instruction,
					Step15PhotoName = e.RegimenSteps.Step15Photo,
					Step16Instruction = e.RegimenSteps.Step16Instruction,
					Step16PhotoName = e.RegimenSteps.Step16Photo,
					Step17Instruction = e.RegimenSteps.Step17Instruction,
					Step17PhotoName = e.RegimenSteps.Step17Photo,
					Step18Instruction = e.RegimenSteps.Step18Instruction,
					Step18PhotoName = e.RegimenSteps.Step18Photo,
					Step19Instruction = e.RegimenSteps.Step19Instruction,
					Step19PhotoName = e.RegimenSteps.Step19Photo,
					Step20Instruction = e.RegimenSteps.Step20Instruction,
					Step20PhotoName = e.RegimenSteps.Step20Photo,
					CreatedOn = e.CreatedOn,
					IsActive = e.IsActive

				}).ToList();
			return regimensModels;
		}

		public bool DeleteRegimens(Regimens regimens)
		{
			try
			{
				var objRegimens = _context.Regimens.Include(x => x.RegimenSteps).FirstOrDefault(x => x.RegimensId == regimens.RegimensId);
				{
					if (objRegimens != null)
					{
						objRegimens.IsActive = false;
						objRegimens.RegimenSteps.IsActive = false;
					}
				}
				_context.SaveChanges();
				return true;
			}

			catch (Exception Ex)
			{
				return false;
			}
		}

		public RegimensModel GetRegimensById(RegimensModel regimens)
		{
			try
			{
				var regimensModel = _context.Regimens.Include(x => x.RegimenSteps).Where(x => x.RegimensId == regimens.RegimensId).FirstOrDefault();
				if (regimensModel != null)
				{
					regimens.RegimensId = regimensModel.RegimensId;
					regimens.RegimenStepsId = regimensModel.RegimenStepsId;
					regimens.Name = regimensModel.Name;
					regimens.Title = regimensModel.Title;
					regimens.Description = regimensModel.Description;
					regimens.Step1Instruction = regimensModel.RegimenSteps.Step1Instruction;
					regimens.Step1PhotoName = regimensModel.RegimenSteps.Step1Photo;
					regimens.Step2Instruction = regimensModel.RegimenSteps.Step2Instruction;
					regimens.Step2PhotoName = regimensModel.RegimenSteps.Step2Photo;
					regimens.Step3Instruction = regimensModel.RegimenSteps.Step3Instruction;
					regimens.Step3PhotoName = regimensModel.RegimenSteps.Step3Photo;
					regimens.Step4Instruction = regimensModel.RegimenSteps.Step4Instruction;
					regimens.Step4PhotoName = regimensModel.RegimenSteps.Step4Photo;
					regimens.Step5Instruction = regimensModel.RegimenSteps.Step5Instruction;
					regimens.Step5PhotoName = regimensModel.RegimenSteps.Step5Photo;
					regimens.Step6Instruction = regimensModel.RegimenSteps.Step6Instruction;
					regimens.Step6PhotoName = regimensModel.RegimenSteps.Step6Photo;
					regimens.Step7Instruction = regimensModel.RegimenSteps.Step7Instruction;
					regimens.Step7PhotoName = regimensModel.RegimenSteps.Step7Photo;
					regimens.Step8Instruction = regimensModel.RegimenSteps.Step8Instruction;
					regimens.Step8PhotoName = regimensModel.RegimenSteps.Step8Photo;
					regimens.Step9Instruction = regimensModel.RegimenSteps.Step9Instruction;
					regimens.Step9PhotoName = regimensModel.RegimenSteps.Step9Photo;
					regimens.Step10Instruction = regimensModel.RegimenSteps.Step10Instruction;
					regimens.Step10PhotoName = regimensModel.RegimenSteps.Step10Photo;
					regimens.Step11Instruction = regimensModel.RegimenSteps.Step11Instruction;
					regimens.Step11PhotoName = regimensModel.RegimenSteps.Step11Photo;
					regimens.Step12Instruction = regimensModel.RegimenSteps.Step12Instruction;
					regimens.Step12PhotoName = regimensModel.RegimenSteps.Step12Photo;
					regimens.Step13Instruction = regimensModel.RegimenSteps.Step13Instruction;
					regimens.Step13PhotoName = regimensModel.RegimenSteps.Step13Photo;
					regimens.Step14Instruction = regimensModel.RegimenSteps.Step14Instruction;
					regimens.Step14PhotoName = regimensModel.RegimenSteps.Step14Photo;
					regimens.Step15Instruction = regimensModel.RegimenSteps.Step15Instruction;
					regimens.Step15PhotoName = regimensModel.RegimenSteps.Step15Photo;
					regimens.Step16Instruction = regimensModel.RegimenSteps.Step16Instruction;
					regimens.Step16PhotoName = regimensModel.RegimenSteps.Step16Photo;
					regimens.Step17Instruction = regimensModel.RegimenSteps.Step17Instruction;
					regimens.Step17PhotoName = regimensModel.RegimenSteps.Step17Photo;
					regimens.Step18Instruction = regimensModel.RegimenSteps.Step18Instruction;
					regimens.Step18PhotoName = regimensModel.RegimenSteps.Step18Photo;
					regimens.Step19Instruction = regimensModel.RegimenSteps.Step19Instruction;
					regimens.Step19PhotoName = regimensModel.RegimenSteps.Step19Photo;
					regimens.Step20Instruction = regimensModel.RegimenSteps.Step20Instruction;
					regimens.Step20PhotoName = regimensModel.RegimenSteps.Step20Photo;
				}
				return regimens;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
	}
}
