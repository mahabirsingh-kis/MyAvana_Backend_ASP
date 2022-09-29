using MyAvanaApi.Models.ViewModels;

namespace MyAvanaApi.Contract
{
    public interface IEmailService
    {
        (bool Succeeded, string Error) SendEmail(string TemplateCode, EmailInformation emailInformation);
    }
}
