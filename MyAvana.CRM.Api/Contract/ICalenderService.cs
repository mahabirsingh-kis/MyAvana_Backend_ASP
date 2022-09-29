using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface ICalenderService
    {
        bool SaveUserDailyRoutine(DailyRoutineTracker dailyRoutineTracker, ClaimsPrincipal user);
        bool SaveHairStyle(DailyRoutineTracker trackingDetails, ClaimsPrincipal user);
        DailyRoutineContent GetDailyRoutine(DailyRoutineTracker dailyRoutineTracker, ClaimsPrincipal user);
        HairCareParts DifferentHairCareParts(string selectedDate, ClaimsPrincipal user);
        bool SaveUserRoutineHairCare(IEnumerable<UserRoutineHairCare> userRoutineHairCare, ClaimsPrincipal claims);
        (bool succeeded, string error) SaveUserHairCareItem(UserRoutineHairCareModel userRoutineHairCare);
        bool RoutineCompleted(DailyRoutineTracker dailyRoutineTracker, ClaimsPrincipal user);
        (bool succeeded, string error) AddProfileImage(DailyRoutineTracker userRoutineHairCare);
    }
}
