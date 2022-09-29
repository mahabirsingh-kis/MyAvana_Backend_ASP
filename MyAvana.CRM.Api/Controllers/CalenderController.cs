using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalenderController : ControllerBase
    {
        IBaseBusiness _baseBusiness;
        ICalenderService _calenderService;
        public CalenderController(IBaseBusiness baseBusiness, ICalenderService calenderService)
        {
            _baseBusiness = baseBusiness;
            _calenderService = calenderService;
        }

        [HttpPost("saveuserdailyroutine")]
        [Authorize(AuthenticationSchemes = "TestKey")]
        public JObject SaveUserDailyRoutine([FromBody]DailyRoutineTracker dailyRoutineTracker)
        {
            bool result = _calenderService.SaveUserDailyRoutine(dailyRoutineTracker, HttpContext.User);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", result);

        }

        [HttpPost("savehairstyle")]
        [Authorize(AuthenticationSchemes = "TestKey")]
        public JObject SaveHairStyle([FromBody]DailyRoutineTracker trackingDetails)
        {
            bool result = _calenderService.SaveHairStyle(trackingDetails, HttpContext.User);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", result);

        }

        [HttpGet("getdailyroutine")]
        [Authorize(AuthenticationSchemes = "TestKey")]
        public JObject GetDailyRoutine(string date)
        {
            DailyRoutineTracker dailyRoutineTracker = new DailyRoutineTracker();
            dailyRoutineTracker.TrackTime = Convert.ToDateTime(date);
            DailyRoutineContent result = _calenderService.GetDailyRoutine(dailyRoutineTracker, HttpContext.User);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Hairstyle for this User doesn't exist.", "0", result);

        }

        [HttpGet("differenthaircareparts")]
        [Authorize(AuthenticationSchemes = "TestKey")]
        public JObject DifferentHairCareParts(string selectedDate)
        {
            HairCareParts result = _calenderService.DifferentHairCareParts(selectedDate, HttpContext.User);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Hairstyle for this User doesn't exist.", "0", result);

        }

        [HttpPost("Saveuserroutinehaircare")]
        [Authorize(AuthenticationSchemes = "TestKey")]
        public JObject SaveUserRoutineHairCare([FromBody]IEnumerable<UserRoutineHairCare> userRoutineHairCare)
        {
            bool result = _calenderService.SaveUserRoutineHairCare(userRoutineHairCare, HttpContext.User);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", result);

        }

        [HttpPost("AddSelectedRoutineItems")]
        public JObject AddSelectedRoutineItems([FromBody]UserRoutineHairCareModel userRoutineHairCare)
        {
            var (succeeded, error) = _calenderService.SaveUserHairCareItem(userRoutineHairCare);
            if (succeeded)
                return _baseBusiness.AddDataOnJson("Success", "1", userRoutineHairCare);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("routinecompleted")]
        [Authorize(AuthenticationSchemes = "TestKey")]
        public JObject RoutineCompleted([FromBody]DailyRoutineTracker dailyRoutine)
        {
            bool succeeded = _calenderService.RoutineCompleted(dailyRoutine, HttpContext.User);
            if (succeeded)
                return _baseBusiness.AddDataOnJson("Success", "1", dailyRoutine);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("AddProfileImage")]
        public JObject AddProfileImage([FromBody]DailyRoutineTracker userRoutineHairCare)
        {
            var (succeeded, error) = _calenderService.AddProfileImage(userRoutineHairCare);
            if (succeeded)
                return _baseBusiness.AddDataOnJson("Success", "1", userRoutineHairCare);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }
    }
}