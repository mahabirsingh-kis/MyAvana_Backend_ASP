using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface ILiveConsultationUserService
    {
        LiveConsultationUserDetails SaveConsultationDetails(LiveConsultationUserDetails LiveConsultationUserDetails);
    }
}
