using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvana.Logger.Contract;
using MyAvana.Models.ViewModels;
using MyAvana.Payments.Api.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyAvana.Payments.Api.Services
{
    public class BillingService : IBillingService
    {
        private readonly ILogger _logger;
        private readonly HttpClient _client;
        private readonly AvanaContext _context;
        private readonly ITokenService _tokenService;
        public BillingService(ILogger logger,AvanaContext avanaContext,ITokenService tokenService)
        {
            _logger = logger;
            _context = avanaContext;
            _tokenService = tokenService;
        }
        public (bool success, string error) AddBillingAddress(BillingAddress billingAddress, System.Security.Claims.ClaimsPrincipal user)
        {
            try
            {
                var result = _tokenService.GetAccountNo(user);
                if (result != null)
                {
                    result.Address = billingAddress.Address;
                    result.City = billingAddress.City;
                    result.State = billingAddress.State;
                    result.Country = billingAddress.Country;
                    
                    _context.SaveChanges();
                    return (true, "");
                }
                return (false, "Invalid user.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Error with billing service", Ex);
                return (false, "Something went wrong. Please try again.");
            }
        }
    }
}
