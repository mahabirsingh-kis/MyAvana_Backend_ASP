using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HubSpot.NET.Core.Requests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using MyAvana.CRM.Api.Contract;
using MyAvana.Framework.TokenService;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnaireController : ControllerBase
    {
        private readonly IQuestionaire _questionaireService;
        private readonly IBaseBusiness _baseBusiness;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserEntity> _userManager;
        private IHttpContextAccessor _httpContextAccessor;
        public QuestionnaireController(IQuestionaire questionaireService, IBaseBusiness baseBusiness, IHostingEnvironment environment,
            ITokenService tokenService, UserManager<UserEntity> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _questionaireService = questionaireService;
            _baseBusiness = baseBusiness;
            _tokenService = tokenService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            var x = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier); //get in the constructor

        }

        [HttpPost("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser()
        {
            var res = Request.HasFormContentType;
            string token = Request.Form["Token"];

            try
            {
                var stream = token;
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

                string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;

                var result = await _questionaireService.AuthenticateUser(email);
                if (result.success)
                {
                    string id = (((Microsoft.AspNetCore.Identity.IdentityUser<System.Guid>)result.result.Value).Id).ToString();
                    return Ok(result.result);

                }
                return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [HttpPost("SaveSurvey")]
        public JObject SaveSurvey(IEnumerable<Questionaire> questionaires)
        {
            IEnumerable<Questionaire> result = _questionaireService.SaveSurvey(questionaires);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        } 
        
        [HttpPost("SaveSurveyAdmin")]
        public JObject SaveSurveyAdmin(IEnumerable<Questionaire> questionaires)
        {
            IEnumerable<Questionaire> result = _questionaireService.SaveSurveyAdmin(questionaires);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }


        [HttpGet("GetQuestionnaireAdmin")]
        public async Task<JObject> GetQuestionnaire()
        {
            List<QuestionAnswerModel> result = await _questionaireService.GetQuestionnaire();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetQuestionnaireAbsenceUserList")]
        public async Task<JObject> GetQuestionnaireAbsenceUserList()
        {
            List<QuestionAnswerModel> result = await _questionaireService.GetQuestionnaireAbsenceUserList();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetQuestionnaireCustomer")]
        public async Task<JObject> GetQuestionnaireCustomer(string id)
        {
            QuestionAnswerModel result = await _questionaireService.GetQuestionnaireCustomer(id);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetQuestionnaireCustomerList")]
        public IActionResult GetQuestionnaireCustomerList()
        {
            List<QuestionnaireCustomerList> result = _questionaireService.GetQuestionnaireCustomerList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetCustomerMessagesList")]
        public IActionResult GetCustomerMessagesList(string id)
        {
            List<CustomerMessageList> result = _questionaireService.GetCustomerMessagesList(new Guid(id));
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost]
        [Route("DeleteQuest")]
        public JObject DeleteQuest(QuestModel quest)
        {
            bool result = _questionaireService.DeleteQuest(quest);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", quest);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpGet("GetFilledSurvey")]
        public JObject GetFilledSurvey()
        {
            List<Questionaire> result = _questionaireService.GetFilledSurvey(HttpContext.User);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetQuestionsForGraph")]
        public JObject GetQuestionsForGraph()
        {
            List<QuestionGraph> result = _questionaireService.GetQuestionsForGraph();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost]
        [Route("SaveCustomerMessage")]
        public JObject SaveCustomerMessage(CustomerMessageModel customerMessageModel)
        {

            CustomerMessageModel result = _questionaireService.SaveCustomerMessage(customerMessageModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);

            return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpGet("GetCustomerEmailTemplate")]
        public JObject GetCustomerEmailTemplate()
        {
            EmailTemplate result = _questionaireService.GetCustomerEmailTemplate();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
    }
}
