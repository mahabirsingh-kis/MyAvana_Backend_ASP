using MyAvana.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IWebLogin
    {
        WebLogin Login(WebLogin webLogin);
        //bool AddNewUser(WebLogin webLogin);
        List<WebLogin> GetUsers();
        WebLogin AddNewUser(WebLogin webLogin);
        WebLogin GetUserByid(WebLogin webLogin);
        bool DeleteUser(WebLogin webLogin);
        //bool DeleteUser(WebLogin webLogin);
    }
}
