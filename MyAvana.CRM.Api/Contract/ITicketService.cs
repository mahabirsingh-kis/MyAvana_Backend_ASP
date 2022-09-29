using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;

namespace MyAvana.CRM.Api.Contract
{
    public interface ITicketService
    {
        (bool succeeded, string Error) CreateTicket(ClaimsPrincipal user, SupportTicket supportTicket);
        (bool succeeded, string Error) CreateUser(ClaimsPrincipal user);
        (JsonResult result, bool succeeded, string Error) GetMytickets(ClaimsPrincipal user);
        (JsonResult result, bool succeeded, string Error) GetAllTicket(ClaimsPrincipal user);
        (bool succeeded, string Error) ReplyTicket(ClaimsPrincipal user, string id, SupportTicket supportTicket);
        (bool succeeded, string Error) CloseTicket(ClaimsPrincipal user, string id);
        (JsonResult result, bool succeeded, string Error) GetSingleTicket(string ticketId);
        (JsonResult result, bool succeeded, string Error) GetUserSupportID(ClaimsPrincipal user);

        (bool success, string error) saveNotes(string message, System.Security.Claims.ClaimsPrincipal user);
    }
}
