using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HairProfileController : ControllerBase
    {
        IHairProfileService hairProfileService;
        private readonly IBaseBusiness _baseBusiness;

        public HairProfileController(IHairProfileService _hairProfileService, IBaseBusiness baseBusiness)
        {
            hairProfileService = _hairProfileService;
            _baseBusiness = baseBusiness;
        }

        [HttpGet("GetHairProfile")]
        public IActionResult GetHairProfile(string token)
        {
            //var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            string userId = tokenS.Claims.First(claim => claim.Type == "sub").Value;

            //string decoded = token.Split('.').Take(2).Select(x => Encoding.UTF8.GetString(Convert.FromBase64String(
            //x.PadRight(x.Length + (x.Length % 4), '=')))).Aggregate((s1, s2) => s1 + Environment.NewLine + s2);

            //string str1 = decoded.Substring(decoded.IndexOf("sub") + 6);
            //string userId = str1.Substring(0, str1.IndexOf(",") - 1);

            var result = hairProfileService.GetHairProfile(userId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("Something goes wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpGet("GetHairProfile2")]
        public IActionResult GetHairProfile2(string token)
        {
            //var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            string userId = tokenS.Claims.First(claim => claim.Type == "sub").Value;

            //string decoded = token.Split('.').Take(2).Select(x => Encoding.UTF8.GetString(Convert.FromBase64String(
            //x.PadRight(x.Length + (x.Length % 4), '=')))).Aggregate((s1, s2) => s1 + Environment.NewLine + s2);

            //string str1 = decoded.Substring(decoded.IndexOf("sub") + 6);
            //string userId = str1.Substring(0, str1.IndexOf(",") - 1);

            var result = hairProfileService.GetHairProfile2(userId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("Something goes wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("CollaboratedDetail")]
        public IActionResult CollaboratedDetail(string hairProfileId)
        {

            var result = hairProfileService.CollaboratedDetails(hairProfileId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("Something goes wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("CollaboratedDetailLocal")]
        public IActionResult CollaboratedDetailLocal(string hairProfileId)
        {

            var result = hairProfileService.CollaboratedDetailsLocal(hairProfileId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("Something goes wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("RecommendedRegimens")]
        public IActionResult RecommendedRegimens(int regimenId)
        {

            var result = hairProfileService.RecommendedRegimens(regimenId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("No data to display") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("RecommendedProducts")]
        public IActionResult RecommendedProducts(int productId)
        {

            var result = hairProfileService.RecommendedProducts(productId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("No data to display") { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpGet("RecommendedProducts2")]
        public IActionResult RecommendedProducts2(int productId)
        {

            var result = hairProfileService.RecommendedProducts2(productId);
            if (result != null)
                return Ok(result);
            return BadRequest(new JsonResult("No data to display") { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpPost("SaveProfile")]
        public JObject SaveProfile([FromForm] HairProfile hairProfile)
        {
            HairProfile result = hairProfileService.SaveProfile(hairProfile);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetHairProfileAdmin")]
        public JObject GetHairProfileAdmin(HairProfileAdminModel hairProfileModel)
        {
            var result = hairProfileService.GetHairProfileAdmin(hairProfileModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetQuestionaireAnswer")]
        public JObject GetQuestionaireAnswer(QuestionaireSelectedAnswer hairProfileModel)
        {
            var result = hairProfileService.GetQuestionaireAnswer(hairProfileModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetHairProfileCustomer")]
        public async Task<JObject> GetHairProfileCustomer(HairProfileCustomerModel hairProfileModel)
        {
            var result = await hairProfileService.GetHairProfileCustomer(hairProfileModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost]
        [Route("GetQuestionaireDetails")]
        public JObject GetQuestionaireDetails(QuestionaireModel questionaire)
        {
            QuestionaireModel result = hairProfileService.GetQuestionaireDetails(questionaire);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", questionaire);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost]
        [Route("GetQuestionaireDetailsMobile")]
        public async Task<JObject> GetQuestionaireDetailsMobile()
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            string userId = tokenS.Claims.First(claim => claim.Type == "sub").Value;

            QuestionaireModel result = await hairProfileService.GetQuestionaireDetailsMobile(userId);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpGet]
        [Route("GetHairProfileCustomerList")]
        public JObject GetHairProfileCustomerList()
        {
            List<HairProfileCustomersModel> result = hairProfileService.GetHairProfileCustomerList();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

    }
}
