using MyAvana.DAL.Auth;
using MyAvana.Logger.Contract;
using MyAvanaApi.Contract;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Linq;
using System.Net.Mail;

namespace MyAvanaApi.Services
{
    public class EmailService : IEmailService
    {
        public readonly AvanaContext _context;
        public readonly ILogger _logger;
        public EmailService(AvanaContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
        public (bool Succeeded, string Error) SendEmail(string TemplateCode, EmailInformation emailInformation)
        {

            bool flag = false;
            string error = "";
            try
            {
                if (TemplateCode != "")
                {
                    var result = _context.EmailTemplates.Where(s => s.TemplateCode == TemplateCode).First<EmailTemplate>();
                    if (result.TemplateCode != null)
                    {
                        error = SendEmail(result, emailInformation);
                        if (error == "Success") { flag = true; }
                    }

                }
                return (flag, error);
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message);
                return (flag, Ex.Message);
            }
        }

        private string SendEmail(EmailTemplate result, EmailInformation emailInformation)
        {
            string error = "";
            string body = "";
            if (result.TemplateCode == "HHCPUPDT")
            {
                body = _context.GenericSettings.Where(s => s.SettingName == "EMAILTEMPLATE02").Select(s => s.DefaultTextMax).FirstOrDefault();
            }
            else
            {
                body = _context.GenericSettings.Where(s => s.SettingName == "EMAILTEMPLATE").Select(s => s.DefaultTextMax).FirstOrDefault();
            }
            try
            {
                var userInfo = _context.Users.Where(p => p.Email == emailInformation.Email).FirstOrDefault();
                if (body.IndexOf("#User#") > 0)
                {
                    if (result.TemplateCode == "CONREQ" || result.TemplateCode == "CUSTJOIN")
                    {
                        body = body.Replace("#User#", emailInformation.Name + ",");
                        emailInformation.Email = "karen@myavana.com,support@myavana.com";
                    }
                    else
                    {
                        body = body.Replace("#User#", userInfo.FirstName + " " + userInfo.LastName + ",");

                    }
                }
                if (body.IndexOf("#Content#") > 0)
                {
                    body = body.Replace("#Content#", result.Body);
                }
                if (body.IndexOf("#Code#") > 0)
                {
                    body = body.Replace("#Code#", emailInformation.Code);
                }

                SmtpClient smtp = new SmtpClient
                {
                    Host = result.HostName,
                    Port = result.HostPort,
                    EnableSsl = true,//result.EnableSSL,

                    Credentials = new System.Net.NetworkCredential(result.SMTPUsername, result.SMTPPassword),
                };
                MailMessage message = new MailMessage(result.SenderEmail, emailInformation.Email, result.Subject, body);

                message.From = new MailAddress(result.SenderEmail, result.SenderName);
                message.IsBodyHtml = true;
                smtp.Send(message);
                error = "Success";
            }
            catch (Exception Ex)
            {
                error = Ex.ToString();
                _logger.LogError(Ex.Message);
            }
            return error;
        }
    }
}
