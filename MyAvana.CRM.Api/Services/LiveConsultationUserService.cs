using System;
using System.Collections.Generic;
using System.Linq;
using MyAvana.CRM.Api.Contract;
using System.Threading.Tasks;
using MyAvana.Models.Entities;
using MyAvana.DAL.Auth;

namespace MyAvana.CRM.Api.Services
{
    public class LiveConsultationUserService : ILiveConsultationUserService
    {
        private readonly AvanaContext _context;
        public LiveConsultationUserService(AvanaContext avanaContext)
        {
            _context = avanaContext;
        }
        public LiveConsultationUserDetails SaveConsultationDetails(LiveConsultationUserDetails LiveConsultationUserDetails)
        {
            var objConsUser = _context.LiveConsultationUserDetails;
            //objConsUser. = LiveConsultationUserDetails.;
            //objConsUser. = LiveConsultationUserDetails.;
            //objConsUser. = LiveConsultationUserDetails.;
            //objConsUser. = LiveConsultationUserDetails.;
            return LiveConsultationUserDetails;
        }
    }
}
