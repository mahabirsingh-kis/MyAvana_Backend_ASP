using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveSchedulesController : Controller
    {
        private readonly ILiveSchedules _liveSchedules;

        private readonly IBaseBusiness _baseBusiness;
        private readonly IHostingEnvironment _environment;
        public LiveSchedulesController(ILiveSchedules liveSchedules, IBaseBusiness baseBusiness)
        {
            _liveSchedules = liveSchedules;
            _baseBusiness = baseBusiness;
        }

        [HttpGet("GetTime")]
        public IActionResult GetTime()
        {
            TimeZoneLiveSchedule result = _liveSchedules.GetTime();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });

        }
        [HttpPost("SaveConsultationDetails")]
        public JObject SaveConsultationDetails(LiveConsultationUserDetails LiveConsultationUserDetails)
        {
            LiveConsultationUserDetails result = _liveSchedules.SaveConsultationDetails(LiveConsultationUserDetails);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }
        [HttpPost("FetchConsultationDetails")]
        public JObject FetchConsultationDetails(LiveConsultationUserDetails LiveConsultationUserDetails)
        {
            GetCustomerScheduleDetails result = _liveSchedules.FetchConsultationDetails(LiveConsultationUserDetails);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }
        [HttpPost("JoinLiveConsultation")]
        public JObject JoinLiveConsultation(LiveConsultationModel liveConsultationModel)
        {
            var userDetails = liveConsultationModel.AspNetUserId;
            //var userName = await _userManager.FindByIdAsync(liveConsultationModel.AspNetUserId);
            //if (userName != null)
            // {
            if (liveConsultationModel.UpdateCustomer)
            {
                // liveConsultationModel.CustomerId = liveConsultationModel.AspNetUserId;
                //liveConsultationModel.CustomerName = userName.FirstName;
            }
            else
            {
                // liveConsultationModel.AdminId = liveConsultationModel.AspNetUserId;
                //liveConsultationModel.AdminName = userName.FirstName;
            }
            // }
            var result = _liveSchedules.JoinLiveConsultation(liveConsultationModel);
            if (result != null)
            {
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            }
            return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
        [HttpGet("GetConsultationList")]
        public IActionResult GetConsultationList()
        {
            List<GetCustomerScheduleDetails> result = _liveSchedules.GetConsultationList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("ChangeIsApproved")]
        public JObject ChangeIsApproved(LiveConsultationUserDetails LiveConsultationUserDetails)
        {
            bool result = _liveSchedules.ChangeIsApproved(LiveConsultationUserDetails);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", LiveConsultationUserDetails);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);

        }
        [HttpPost("CheckIsOtherParticipantReady")]
        public JObject CheckIsOtherParticipantReady(LiveConsultationModel liveConsultationModel)
        {
            var result = _liveSchedules.CheckIsOtherParticipantReady(liveConsultationModel);
            if (result != null)
            {
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            }
            return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
        [HttpPost("UpdateLiveConsultationInformation")]
        public JObject UpdateLiveConsultationInformation(LiveConsultationModel liveConsultationModel)
        {
            var result = _liveSchedules.UpdateLiveConsultationInformation(liveConsultationModel);
            if (result != null)
            {
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            }
            return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
        //[HttpPost("GetUserDetailsById")]
        //public JObject GetUserDetailsById(GetCustomerScheduleDetails Customerid)
        //{
        //    GetCustomerScheduleDetails result = _liveSchedules.GetUserDetailsById(Customerid);
        //    if (result != null)
        //        return _baseBusiness.AddDataOnJson("Success", "1", result);
        //    else
        //        return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        //}
    }
}
