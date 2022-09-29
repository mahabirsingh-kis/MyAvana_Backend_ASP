using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndicatorController : ControllerBase
    {
        private readonly IIndicatorService _indicatorService;
        private readonly IBaseBusiness _baseBusiness;
        public IndicatorController(IIndicatorService indicatorService, IBaseBusiness baseBusiness)
        {
            _indicatorService = indicatorService;
            _baseBusiness = baseBusiness;
        }
        [HttpGet("GetIndicators")]
        public JObject GetIndicators()
        {
            List<IndicatorModel> result = _indicatorService.GetIndicators();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("SaveIndicator")]
        public JObject SaveIndicator(IndicatorModel indicatorEntity)
        {
            IndicatorModel result = _indicatorService.SaveIndicator(indicatorEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("GetIndicatorById")]
        public JObject GetIndicatorById(IndicatorModel indicatorEntity)
        {
            var result = _indicatorService.GetIndicatorById(indicatorEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost]
        [Route("DeleteIndicator")]
        public JObject DeleteIndicator(IndicatorModel indicatorEntity)
        {
            bool result = _indicatorService.DeleteIndicator(indicatorEntity);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", indicatorEntity);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
    }
}