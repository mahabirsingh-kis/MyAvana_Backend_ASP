using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllersr
{
	[Route("api/[controller]")]
	[ApiController]
	public class RegimensController : ControllerBase
	{
		private readonly IRegimenService _regimenService;
		private readonly IBaseBusiness _baseBusiness;
		private readonly IHostingEnvironment _environment;
		public RegimensController(IRegimenService regimenService, IBaseBusiness baseBusiness)
		{
			_regimenService = regimenService;
			_baseBusiness = baseBusiness;
		}


		[HttpPost("SaveRegimens")]
		public JObject SaveRegimens([FromForm] RegimensModel regimensModel)
		{
			if (Request.HasFormContentType)
			{
				if (regimensModel.Step1Photo != null)
				{
					string fileName = regimensModel.Step1Photo.FileName;
					const string UPLOAD_FOLDER = "Regimens";
					if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER)))
					{
						Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
					}
					var path = Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_FOLDER, fileName);

					using (var stream = new FileStream(path, FileMode.Create))
					{
						regimensModel.Step1Photo.CopyToAsync(stream);
					}
					regimensModel.Step1PhotoName = regimensModel.Step1Photo.FileName;
				}
				if (regimensModel.Step2Photo != null)
				{
					string fileName = regimensModel.Step2Photo.FileName;
					const string UPLOAD_FOLDER = "Regimens";
					if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER)))
					{
						Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
					}
					var path = Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_FOLDER, fileName);

					using (var stream = new FileStream(path, FileMode.Create))
					{
						regimensModel.Step2Photo.CopyToAsync(stream);
					}
					regimensModel.Step2PhotoName = regimensModel.Step2Photo.FileName;
				}
				if (regimensModel.Step3Photo != null)
				{
					string fileName = regimensModel.Step3Photo.FileName;
					const string UPLOAD_FOLDER = "Regimens";
					if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER)))
					{
						Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
					}
					var path = Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_FOLDER, fileName);

					using (var stream = new FileStream(path, FileMode.Create))
					{
						regimensModel.Step3Photo.CopyToAsync(stream);
					}
					regimensModel.Step3PhotoName = regimensModel.Step3Photo.FileName;
				}
				if (regimensModel.Step4Photo != null)
				{
					string fileName = regimensModel.Step4Photo.FileName;
					const string UPLOAD_FOLDER = "Regimens";
					if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER)))
					{
						Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
					}
					var path = Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_FOLDER, fileName);

					using (var stream = new FileStream(path, FileMode.Create))
					{
						regimensModel.Step4Photo.CopyToAsync(stream);
					}
					regimensModel.Step4PhotoName = regimensModel.Step4Photo.FileName;
				}
				if (regimensModel.Step5Photo != null)
				{
					string fileName = regimensModel.Step5Photo.FileName;
					const string UPLOAD_FOLDER = "Regimens";
					if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER)))
					{
						Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
					}
					var path = Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_FOLDER, fileName);

					using (var stream = new FileStream(path, FileMode.Create))
					{
						regimensModel.Step5Photo.CopyToAsync(stream);
					}
					regimensModel.Step5PhotoName = regimensModel.Step5Photo.FileName;
				}
			}
			RegimensModel result = _regimenService.SaveRegimens(regimensModel);
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
		}

		[HttpGet("GetRegimens")]
		public JObject GetRegimens()
		{
			List<RegimensModel> result = _regimenService.GetRegimens();
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
		}

		[HttpPost("GetRegimensById")]
		public JObject GetRegimensById(RegimensModel regimens)
		{
			RegimensModel result = _regimenService.GetRegimensById(regimens);
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

		}

		[HttpPost]
		[Route("DeleteRegimens")]
		public JObject DeleteRegimens(Regimens regimens)
		{
			bool result = _regimenService.DeleteRegimens(regimens);
			if (result)
				return _baseBusiness.AddDataOnJson("Success", "1", regimens);
			else
				return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
		}
	}
}