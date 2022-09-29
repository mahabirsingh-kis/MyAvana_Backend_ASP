using MyAvana.Models;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using MyAvana.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface ILiveSchedules
    {
        TimeZoneLiveSchedule GetTime();
        LiveConsultationUserDetails SaveConsultationDetails(LiveConsultationUserDetails liveConsultationUserDetails);
        GetCustomerScheduleDetails FetchConsultationDetails(LiveConsultationUserDetails liveConsultationUserDetails);
        LiveConsultationModel JoinLiveConsultation(LiveConsultationModel liveConsultationUserDetails);
        LiveConsultationModel CheckIsOtherParticipantReady(LiveConsultationModel liveInterviewModel);
        LiveConsultationModel UpdateLiveConsultationInformation(LiveConsultationModel liveInterviewModel);
        List<GetCustomerScheduleDetails> GetConsultationList();
        bool ChangeIsApproved(LiveConsultationUserDetails liveConsultationUserDetails);

    }
}