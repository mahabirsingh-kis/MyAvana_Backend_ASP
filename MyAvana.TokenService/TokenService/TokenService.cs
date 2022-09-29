using MyAvanaApi.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;

namespace MyAvana.Framework.TokenService
{
    public class TokenService : ITokenService
    {
        private const string ACCOUNT_API_PATH = "/api/Account/GetAccountNo?email=";
        private readonly HttpClient _httpClient;
        public TokenService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5002/");
        }
        public UserEntity GetAccountNo(ClaimsPrincipal claims)
        {
            Claim claim = claims.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            if (claim.Value != "")
            {
                var response = _httpClient.GetAsync(ACCOUNT_API_PATH + claim.Value).GetAwaiter().GetResult();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return JsonConvert.DeserializeObject<UserEntity>(content);
                }
            }
            return null;
        }
    }
}
