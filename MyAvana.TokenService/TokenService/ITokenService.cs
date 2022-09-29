using MyAvanaApi.Models.Entities;
using System.Security.Claims;

namespace MyAvana.Framework.TokenService
{
    public interface ITokenService
    {
        UserEntity GetAccountNo(ClaimsPrincipal claims);
    }
}
