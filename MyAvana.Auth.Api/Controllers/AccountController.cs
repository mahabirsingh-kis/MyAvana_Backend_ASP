using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyAvana.Auth.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyAvana.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _account;
        private readonly HttpClient _httpClient;
        private readonly AvanaContext _avanacontext;
        public AccountController(IAccountService account, AvanaContext avanacontext)
        {
            _account = account;
            _avanacontext = avanacontext;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5002/");
        }
        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] Signup signup)
        {
            var result = _account.SignUp(signup);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] Authentication authentication)
        {
            var result = _account.SignIn(authentication);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("activateuser")]
        public IActionResult ActivateUser([FromBody] CodeVerify codeVerify)
        {
            var result = _account.ActivateUser(codeVerify);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpGet("resendcode")]
        public IActionResult ResendCode(string email)
        {
            var result = _account.ResendCode(email);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("forgetpassword")]
        public IActionResult ForgetPass(string email)
        {
            var result = _account.ForgetPass(email);
            if (result.success) return Ok(result.result);
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        [HttpPost("setpass")]
        public async Task<IActionResult> SetPassAsync([FromBody] SetPassword setPassword)
        {
            var result = await _account.SetPass(setPassword);
            if (result.success) return Ok(new JsonResult("") { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        [HttpGet("GetAccountNo")]
        public IActionResult GetAccountNo(string email)
        {
            var result = _account.GetAccountNo(email);
            if (result.success) return Ok(result.user);
            return BadRequest(new JsonResult(result.error));
        }

        [HttpPost("WebSignup")]
        public IActionResult WebSignup([FromBody] Signup signup)
        {
            var result = _account.WebSignup(signup);
            if (result.userId != null) return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("UserDetails")]
        public IActionResult UserDetails(string deviceId)
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;

                var response = _httpClient.GetAsync("/api/Account/GetAccountNo?email=" + email).GetAwaiter().GetResult();
                string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                UserEntity entity = JsonConvert.DeserializeObject<UserEntity>(content);
                if (entity != null)
                {
                        bool result = _account.updateDeviceId(entity, deviceId);
                        if (result)
                            entity.DeviceId = deviceId;
                    return Ok(new JsonResult(entity) { StatusCode = (int)HttpStatusCode.OK });
                }
                    
                return BadRequest(new JsonResult("User not found") { StatusCode = (int)HttpStatusCode.BadRequest });
            }
            catch (Exception Ex)
            {
                return BadRequest(new JsonResult("Authorization token is not valid") { StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }

        [HttpPost("saveAIResult")]
        public IActionResult saveAIResult(UserEntity userDetails)
         {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = handler.ReadToken(token) as JwtSecurityToken;
                string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;

                var response = _httpClient.GetAsync("/api/Account/GetAccountNo?email=" + email).GetAwaiter().GetResult();
                string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                UserEntity entity = JsonConvert.DeserializeObject<UserEntity>(content);
                if (entity != null)
                {
                    bool result = _account.saveAIResult(entity, userDetails.AIResult);
                    if (result)
                        entity.AIResult = userDetails.AIResult;
                    return Ok(new JsonResult(entity) { StatusCode = (int)HttpStatusCode.OK });
                }

                return BadRequest(new JsonResult("User not found") { StatusCode = (int)HttpStatusCode.BadRequest });
            }
            catch (Exception Ex)
            {
                return BadRequest(new JsonResult("Authorization token is not valid") { StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
    }
}