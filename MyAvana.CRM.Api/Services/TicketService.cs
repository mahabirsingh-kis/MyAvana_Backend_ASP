using HubSpot.NET.Api.Contact.Dto;
using HubSpot.NET.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Logger.Contract;
using MyAvana.Models.ViewModels;
using MyAvana.Models.ViewModels.Zendesk;
using MyAvanaApi.Models.Entities;
using Newtonsoft.Json;
using MyAvana.Framework.TokenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZendeskApi_v2;
using ZendeskApi_v2.Models.Constants;
using ZendeskApi_v2.Models.HelpCenter.Comments;
using ZendeskApi_v2.Models.Shared;

namespace MyAvana.CRM.Api.Services
{
    public class TicketService : ITicketService
    {
        private readonly ILogger _logger;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly AvanaContext _context;
        private readonly Dictionary<int, string> priority;
        private readonly HttpClient _client;
        private readonly HubSpotApi _hubSpotApi;
        public TicketService(ILogger logger,ITokenService tokenService,IConfiguration configuration, AvanaContext context,IHttpClientFactory httpClient)
        {
            _logger = logger;
            _tokenService = tokenService;
            _configuration = configuration;
            priority = new Dictionary<int, string>();
            _context = context;
            _client = httpClient.CreateClient();
            _hubSpotApi = new HubSpotApi(_configuration.GetSection("Hubspot:Key").Value);
            priority.Add(1, "urgent");
            priority.Add(2, "high");
            priority.Add(3, "normal");
            priority.Add(4, "low");
        }

        public (bool succeeded, string Error) CreateTicket(ClaimsPrincipal claim, SupportTicket supportTicket)
        {
            try
            {
                var usr = _tokenService.GetAccountNo(claim);
                if (usr != null)
                {
                    ZendeskApi api = new ZendeskApi(_configuration.GetSection("ZenDesk:SubDomainAddress").Value, _configuration.GetSection("ZenDesk:EmailAddress").Value, _configuration.GetSection("ZenDesk:Password").Value);
                    var _usr = _context.Users.Where(p => p.AccountNo == usr.AccountNo).FirstOrDefault();
                    if (_usr.TicketUserId <= 0)
                    {
                        var userResult = CreateUser(claim);
                    }

                    var user = _context.Users.Where(p => p.AccountNo == usr.AccountNo).FirstOrDefault();
                    if (user.TicketUserId > 0)
                    {
                        var ticket = new ZendeskApi_v2.Models.Tickets.Ticket()
                        {
                            Subject = supportTicket.Subject,
                            RequesterId = usr.TicketUserId,
                            Requester = new ZendeskApi_v2.Models.Tickets.Requester { Email = usr.Email, Name = usr.FirstName + " " + usr.LastName },
                            Comment = new ZendeskApi_v2.Models.Tickets.Comment()
                            {
                                CreatedAt = DateTime.Now,
                                Public = true,
                                Body = supportTicket.Message
                            }
                        };
                        ticket.Priority = (priority.ContainsKey(supportTicket.Priority) ? priority[supportTicket.Priority] : priority[4]);
                        
                        if (supportTicket.HasAttachment)
                        {
                            var res = api.Attachments.UploadAttachmentAsync(new ZenFile()
                            {
                                ContentType = supportTicket.contentType,
                                FileName = supportTicket.fileName,
                                FileData = Convert.FromBase64String(supportTicket.fileData)
                            });

                        }
                        var t1 = api.Tickets.CreateTicketAsync(ticket).GetAwaiter().GetResult();

                        if (t1.Ticket != null)
                        {
                            if (t1.Ticket.Id > 0)
                            {
                                _context.UsersTicketsEntities.Add(new Models.Entities.UsersTicketsEntity() {
                                    TicketId = t1.Ticket.Id.Value,
                                    CreatedAt = DateTime.UtcNow.ToString(),
                                    Description = t1.Ticket.Description,
                                    Priority =t1.Ticket.Priority,
                                    Subject=t1.Ticket.Subject,
                                    UserId=t1.Ticket.RequesterId.Value.ToString(),
                                    Status = t1.Ticket.Status
                                });
                                _context.SaveChanges();
                                return (true, "");
                            }
                        }
                        else
                        {
                            return (false, "Cannot create the ticket.");

                        }

                    }
                    return (false, "Cannot create the ticket.");
                }
                return (false, "Invalid user.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Error in Ticket Service", Ex);
                return (false, Ex.Message);
            }
        }

        public (bool succeeded, string Error) CreateUser(ClaimsPrincipal claim)
        {
            try
            {
                var usr = _tokenService.GetAccountNo(claim);
                if (usr != null)
                {
                    long ticketId = 0;
                    ZendeskApi api = new ZendeskApi(_configuration.GetSection("ZenDesk:SubDomainAddress").Value, _configuration.GetSection("ZenDesk:EmailAddress").Value, _configuration.GetSection("ZenDesk:Password").Value);
                    var user = new ZendeskApi_v2.Models.Users.User()
                    {
                        Active = true,
                        CreatedAt = DateTime.Now,
                        Email = usr.Email,
                        Name = usr.FirstName,
                        Role = "end-user",
                    };
                    var data = api.Users;
                    var activeuser = api.Users.SearchByEmail(usr.Email);
                    if (activeuser.Count <= 0)
                    {
                        ticketId = (long)api.Users.CreateUser(user).User.Id;
                    }
                    else
                    {
                        ticketId = (long)activeuser.Users.FirstOrDefault().Id;
                    }

                    if (ticketId > 0)
                    {
                        var userupdate = _context.Users.Where(p => p.AccountNo == usr.AccountNo).FirstOrDefault();
                        userupdate.TicketUserId = ticketId;
                        _context.SaveChanges();
                        return (true, "");
                    }
                    else
                        return (false, "Cannot create ticket user.");
                }
                return (false, "Invalid user id.");

            }
            catch (Exception Ex)
            {
                _logger.LogError("Error in Ticket Service", Ex);
                return (false, Ex.Message);
            }
        }

        public (JsonResult result, bool succeeded, string Error) GetAllTicket(ClaimsPrincipal claim)
        {
            try
            {
                ZendeskApi api = new ZendeskApi(_configuration.GetSection("ZenDesk:SubDomainAddress").Value, _configuration.GetSection("ZenDesk:EmailAddress").Value, _configuration.GetSection("ZenDesk:Password").Value);
                var tickets = api.Tickets.GetAllTicketsAsync();
                var tik1 = api.Tickets.GetAllTicketMetricsAsync();

                if (tickets.Result.Count > 0)
                    return (new JsonResult(tickets.Result.Tickets) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                else
                    return (new JsonResult(""), false, "Cannot find any ticket");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Error in Ticket Service", Ex);
                return (new JsonResult(""), false, Ex.Message);
            }
        }

        public (JsonResult result, bool succeeded, string Error) GetMytickets(ClaimsPrincipal claim)
        {
            try
            {
                var usr = _tokenService.GetAccountNo(claim);
                if (usr != null)
                {
                    _logger.LogError(usr.TicketUserId.ToString().Trim());
                    //var tickets = GetTickets(usr.TicketUserId);
                    ZendeskApi api = new ZendeskApi(_configuration.GetSection("ZenDesk:SubDomainAddress").Value, _configuration.GetSection("ZenDesk:EmailAddress").Value, _configuration.GetSection("ZenDesk:Password").Value);
                    var tickets = api.Tickets.GetTicketsByUserID(usr.TicketUserId, 100, 1);
                    //_logger.LogError(usr.TicketUserId.ToString().Trim());

                    //var tickets = _context.UsersTicketsEntities.Where(s => s.UserId.Trim() == usr.TicketUserId.ToString().Trim()).ToList();
                    //_logger.LogError(tickets.Count.ToString());
                    if (tickets.Count > 0)
                        return (new JsonResult(tickets) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                    else
                        return (new JsonResult(""), false, "Cannot find any ticket");
                }
                return (new JsonResult(""), false, "Invalid user.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Error in Ticket Service"+Ex.Message, Ex);
                return (new JsonResult(""), false, Ex.Message);
            }
        }

        private List<MyAvana.Models.ViewModels.Zendesk.Ticket> GetTickets(long ticketUserId)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://myavana.zendesk.com/api/v2/tickets.json"))
                    {
                        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes("customercare@myavana.com:Victory2019!"));
                        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                        var response = httpClient.SendAsync(request).GetAwaiter().GetResult();

                        var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        _logger.LogError(responseString);
                        var model = JsonConvert.DeserializeObject<TicketResponse>(responseString);
                        return model.tickets.Where(s => s.requester_id.ToString() == ticketUserId.ToString()).ToList();
                    }
                }
                //var url = _configuration.GetSection("ZenDesk:SubDomainAddress").Value + "/api/v2/tickets.json";
                //using (var httpClient = new HttpClient())
                //{
                //    using (var request = new HttpRequestMessage(new HttpMethod("GET"), url))
                //    {
                //        var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(_configuration.GetSection("ZenDesk:EmailAddress").Value +":"+_configuration.GetSection("ZenDesk:Password").Value));
                //        request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");
                //        var response = httpClient.SendAsync(request).GetAwaiter().GetResult();
                //        var responseString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                       
                    
                //    }
                //}                
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message);
                return new List<Models.ViewModels.Zendesk.Ticket>();
            }
        }

        public (bool succeeded, string Error) ReplyTicket(ClaimsPrincipal claim, string id, SupportTicket supportTicket)
        {
            try
            {
                var usr = _tokenService.GetAccountNo(claim);
                if (usr != null)
                {
                    ZendeskApi api = new ZendeskApi(_configuration.GetSection("ZenDesk:SubDomainAddress").Value, _configuration.GetSection("ZenDesk:EmailAddress").Value, _configuration.GetSection("ZenDesk:Password").Value);
                    var tickets = api.Tickets.GetTicket(Convert.ToInt64(id)).Ticket;
                    if (tickets.Id > 0)
                    {
                        if (supportTicket.HasAttachment)
                        {
                            var res = api.Attachments.UploadAttachmentAsync(new ZenFile()
                            {
                                ContentType = supportTicket.contentType,
                                FileName = supportTicket.fileName,
                                FileData = Convert.FromBase64String(supportTicket.fileData)
                            });

                            ZendeskApi_v2.Models.Tickets.Comment _comment = new ZendeskApi_v2.Models.Tickets.Comment()
                            {
                                Body = supportTicket.Message,
                                Public = true,
                                Uploads = new List<string>() { res.Result.Token },
                                CreatedAt = DateTime.Now,
                                AuthorId = usr.TicketUserId
                            };
                            tickets.Comment = _comment;
                            var updateResponse = api.Tickets.UpdateTicket(tickets, tickets.Comment);
                        }
                        else
                        {
                            var updateResponse = api.Tickets.UpdateTicket
                            (
                                tickets,
                                new ZendeskApi_v2.Models.Tickets.Comment()
                                {
                                    Body = supportTicket.Message,
                                    Public = true,
                                    Uploads = new List<string>(),
                                    CreatedAt = DateTime.Now,
                                    AuthorId = usr.TicketUserId
                                }
                            );
                        }


                        return (true, "");
                    }
                    return (false, "Invalid Ticket Id.");
                }
                return (false, "Invalid user.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Error in Ticket Service", Ex);
                return (false, Ex.Message);
            }
        }
        public (bool succeeded, string Error) CloseTicket(ClaimsPrincipal claim, string id)
        {
            try
            {
                var usr = _tokenService.GetAccountNo(claim);
                if (usr != null)
                {
                    ZendeskApi api = new ZendeskApi(_configuration.GetSection("ZenDesk:SubDomainAddress").Value, _configuration.GetSection("ZenDesk:EmailAddress").Value, _configuration.GetSection("ZenDesk:Password").Value);
                    var tickets = api.Tickets.GetTicket(Convert.ToInt64(id)).Ticket;
                    if (tickets.Id > 0)
                    {
                        tickets.Status = TicketStatus.Closed;
                        var resp2 = api.Tickets.UpdateTicket(tickets, new ZendeskApi_v2.Models.Tickets.Comment { Body = "Closing Ticket", AuthorId = usr.TicketUserId });
                        return (true, "");
                    }
                    return (false, "Invalid Ticket Id.");
                }
                return (false, "Invalid user.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Error in Ticket Service", Ex);
                return (false, Ex.Message);
            }
        }

        public (JsonResult result, bool succeeded, string Error) GetSingleTicket(string ticketId)
        {
            try
            {
                ZendeskApi api = new ZendeskApi(_configuration.GetSection("ZenDesk:SubDomainAddress").Value, _configuration.GetSection("ZenDesk:EmailAddress").Value, _configuration.GetSection("ZenDesk:Password").Value);
                var tickets = api.Tickets.GetTicket(Convert.ToInt64(ticketId));
                if (tickets.Ticket != null)
                {
                    var comments = api.Tickets.GetTicketComments(Convert.ToInt64(tickets.Ticket.Id)).Comments;
                    if (comments.Count > 0)
                    {
                        return (new JsonResult(new Dictionary<string, object>
                        {
                            { "Ticket", tickets.Ticket },
                            { "Comments", comments}
                        })
                        { StatusCode = (int)HttpStatusCode.OK }, true, "");
                    }
                    else
                    {
                        return (new JsonResult(""), false, "No Comments to Show!!");
                    }

                }
                return (new JsonResult(""), false, "Cannot get the tickets");


            }
            catch (Exception Ex)
            {
                _logger.LogError("Error in Ticket Service", Ex);
                return (new JsonResult(""), false, Ex.Message);
            }
        }

        public (JsonResult result, bool succeeded, string Error) GetUserSupportID(ClaimsPrincipal claim)
        {
            try
            {
                var usr = _tokenService.GetAccountNo(claim);
                if (usr != null)
                {
                    string result = string.IsNullOrEmpty(usr.TicketUserId.ToString()) ? "" : ((usr.TicketUserId > 0) ? usr.TicketUserId.ToString() : "");

                    return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                }
                return (new JsonResult(""), false, "Invalid user.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Error in Ticket Service", Ex);
                return (new JsonResult(""), false, Ex.Message);
            }
        }
        public (bool success, string error) saveNotes(string message, ClaimsPrincipal user)
        {
            try
            {
                var accountNo = _tokenService.GetAccountNo(user);
                if (accountNo != null)
                {
                    if (string.IsNullOrEmpty(accountNo.HubSpotContactId))
                    {
                        CreateContact(accountNo);
                        _context.SaveChanges();
                    }
                    var response = _hubSpotApi.Engagement.Create(new HubSpot.NET.Api.Engagement.Dto.EngagementHubSpotModel()
                    {
                        Associations = new HubSpot.NET.Api.Engagement.Dto.EngagementHubSpotAssociationsModel() { ContactIds = new List<long>() { (Convert.ToInt64(accountNo.HubSpotContactId)) } },
                        Engagement = new HubSpot.NET.Api.Engagement.Dto.EngagementHubSpotEngagementModel() { Type = "NOTE" },
                        Metadata = new { body = message }
                    });
                    return (true, "");

                }
                return (false, "Invalid user.");
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message, Ex);
                return (false, "Something went wrong. Please try again later.");
            }
        }
        private void CreateContact(UserEntity entity)
        {
            try
            {

                var contact = _hubSpotApi.Contact.CreateOrUpdate(new ContactHubSpotModel()
                {
                    Email = entity.Email,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Phone = entity.PhoneNumber
                });
                entity.HubSpotContactId = contact.Id.ToString();
                
            }
            catch (HubSpot.NET.Core.HubSpotException Ex)
            {
                if (!string.IsNullOrEmpty(Ex.RawJsonResponse))
                {
                    var response = JsonConvert.DeserializeObject<MyAvanaApi.Models.ViewModels.HubSpotException>(Ex.RawJsonResponse);
                    if (response.message == "Contact already exists")
                    {
                        entity.HubSpotContactId = response.identityProfile.vid.ToString();
                    }
                }
                _logger.LogError(Ex.Message, Ex);
                
            }
        }
    }
}
