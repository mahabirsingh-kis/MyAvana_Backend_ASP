using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class ToolsController : ControllerBase
    {
        private readonly IToolsService _toolsService;
        private readonly IBaseBusiness _baseBusiness;
        public ToolsController(IToolsService toolsService, IBaseBusiness baseBusiness)
        {
            _toolsService = toolsService;
            _baseBusiness = baseBusiness;
        }
        [HttpGet("GetTools")]
        public JObject GetTools()
        {
            List<ToolsModel> result = _toolsService.GetTools();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("SaveTools")]
        public JObject SaveTools(ToolsModel toolsEntity)
        {
            ToolsModel result = _toolsService.SaveTools(toolsEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("GetToolsById")]
        public JObject GetToolsById(ToolsModel toolsEntity)
        {
            var result = _toolsService.GetToolsById(toolsEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost]
        [Route("DeleteTool")]
        public JObject DeleteTool(ToolsModel toolsEntity)
        {
            bool result = _toolsService.DeleteTool(toolsEntity);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", toolsEntity);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

    }
}