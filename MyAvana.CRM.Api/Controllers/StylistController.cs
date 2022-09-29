using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StylistController : ControllerBase
    {
        private readonly IStylistService _stylistService;
        private readonly IBaseBusiness _baseBusiness;
        public StylistController(IStylistService stylistService, IBaseBusiness baseBusiness)
        {
            _baseBusiness = baseBusiness;
            _stylistService = stylistService;
        }

        [HttpPost("AddUpdateStylist")]
        public JObject AddUpdateStylist(StylistModel stylist)
        {
            StylistModel result = _stylistService.AddUpdateStylist(stylist);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetStylistSpecialty")]
        public IActionResult GetProductsType()
        {   
            List<StylistSpecialty> result = _stylistService.GetStylistSpecialty();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("GetStylishAdmin")]
        public JObject GetStylishAdmin(StylishAdminModel stylishAdminModel)
        {
            var result = _stylistService.GetStylishAdmin(stylishAdminModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetStylistList")]
        public JObject GetStylistList()
        {
            List<StylistListModel> result =  _stylistService.GetStylistList();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost]
        [Route("DeleteStylist")]
        public JObject DeleteStylist(StylistModel stylist)
        {
            bool result = _stylistService.DeleteStylist(stylist);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", stylist);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

		[HttpPost("AddStylistList")]
		public JObject AddStylistList(IEnumerable<StylistListModel> stylistData)
		{

			var result = _stylistService.AddStylistList(stylistData);
			if (result != null)
				return _baseBusiness.AddDataOnJson("Success", "1", result);
			else
				return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
		}

	}
}
