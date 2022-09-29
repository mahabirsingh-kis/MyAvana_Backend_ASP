using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.CRM.Api.Services;
using MyAvana.Models.Entities;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllers
{
    public class LiveConsultationUserDetailsController : Controller
    {
        private readonly ILiveConsultationUserService _UserService;
        private readonly IBaseBusiness _baseBusiness;
        private readonly IHostingEnvironment _environment;
        public LiveConsultationUserDetailsController(ILiveConsultationUserService LiveConsultationUserDetails, IBaseBusiness baseBusiness, IHostingEnvironment hostingEnvironment)
        {
            _baseBusiness = baseBusiness;
            _UserService = LiveConsultationUserDetails;
            _environment = hostingEnvironment;
        }
        [HttpPost("SaveConsultationDetails")]
        public JObject SaveConsultationDetails(LiveConsultationUserDetails LiveConsultationUserDetails)
        {
            LiveConsultationUserDetails result = _UserService.SaveConsultationDetails(LiveConsultationUserDetails);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }
    }
}