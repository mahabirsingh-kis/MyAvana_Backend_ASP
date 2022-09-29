using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class WebService : IWebLogin
    {
        private readonly AvanaContext _context;
        public WebService(AvanaContext avanaContext)
        {
            _context = avanaContext;
        }
        public WebLogin Login(WebLogin webLogin)
        {
            try
            {
                WebLogin objWeb = _context.WebLogins.Where(x => x.UserEmail == webLogin.UserEmail && x.Password == webLogin.Password && x.IsActive == true).FirstOrDefault();
                if (objWeb != null)
                {
                    webLogin.UserId = objWeb.UserId;
                    webLogin.UserEmail = objWeb.UserEmail;
                    webLogin.Password = objWeb.Password;
                    webLogin.IsActive = objWeb.IsActive;

                    return objWeb;
                }
                return null;
            }
            catch (Exception ex)
            {
                WebLogin objWeb = new WebLogin();
                objWeb.UserEmail = ex.Message;
                return objWeb;
            }
        }

        public List<WebLogin> GetUsers()
        {
            try
            {
                List<WebLogin> webLogins = _context.WebLogins.Where(x => x.IsActive == true).Select(x => new WebLogin
                {
                    UserId = x.UserId,
                    UserEmail = x.UserEmail,
                    Password = x.Password,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    IsActive = x.IsActive,
                    UserType = x.UserType
                }).OrderByDescending(x => x.CreatedOn).ToList();

                return webLogins;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public WebLogin GetUserByid(WebLogin webLogin)
        {
            try
            {
                WebLogin getUser = _context.WebLogins.Where(x => x.UserId == webLogin.UserId).FirstOrDefault();
                return getUser;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "@#$abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        public WebLogin AddNewUser(WebLogin webLogin)
        {
            try
            {
                if (webLogin.UserId != 0)
                {
                    var objuser = _context.WebLogins.Where(x => x.UserId == webLogin.UserId).FirstOrDefault();
                    objuser.UserEmail = webLogin.UserEmail;
                }
                else
                {
                    var password = CreateRandomPassword(9);
                    _context.WebLogins.Add(new WebLogin()
                    {
                        UserEmail = webLogin.UserEmail,
                        Password = password,
                        CreatedBy = "Admin",
                        IsActive = true,
                        CreatedOn = DateTime.UtcNow,
                        UserType = webLogin.UserType

                    });
                }
                _context.SaveChanges();
                return webLogin;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteUser(WebLogin webLogin)
        {
            try
            {
                var objUser = _context.WebLogins.FirstOrDefault(x => x.UserId == webLogin.UserId);
                {
                    if (objUser != null)
                    {
                        objUser.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                return false;
            }
        }
    }
}
