using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebLoginController : ControllerBase
    {
        private readonly IWebLogin _webService;
        private readonly IBaseBusiness _baseBusiness;
        public WebLoginController(IWebLogin webService, IBaseBusiness baseBusiness)
        {
            _webService = webService;
            _baseBusiness = baseBusiness;
        }

        [HttpPost("Login")]
        public JObject Login(WebLogin webLogin)
        {
            WebLogin result = _webService.Login(webLogin);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetUsers")]
        public JObject GetUsers()
        {
            List<WebLogin> result = _webService.GetUsers();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost]
        [Route("AddNewUser")]
        public JObject AddNewUser(WebLogin webLogin)
        {
            WebLogin result = _webService.AddNewUser(webLogin);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", webLogin);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost("GetUserByid")]
        public JObject GetUserByid(WebLogin webLogin)
        {
            WebLogin result = _webService.GetUserByid(webLogin);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost]
        [Route("DeleteUser")]
        public JObject DeleteUser(WebLogin webLogin)
        {
            bool result = _webService.DeleteUser(webLogin);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", webLogin);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
    }
}
