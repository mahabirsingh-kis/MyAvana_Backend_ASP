using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IIndicatorService
    {
        List<IndicatorModel> GetIndicators();
        IndicatorModel SaveIndicator(IndicatorModel IndicatorEntity);
        IndicatorModel GetIndicatorById(IndicatorModel IndicatorModel);
        bool DeleteIndicator(IndicatorModel IndicatorModel);
    }
}
