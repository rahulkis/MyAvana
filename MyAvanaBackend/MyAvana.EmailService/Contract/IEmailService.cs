using Microsoft.AspNetCore.Http;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.ViewModels;

namespace MyAvanaApi.Contract
{
    public interface IEmailService
    {
        (bool Succeeded, string Error) SendEmail(string TemplateCode, EmailInformation emailInformation);
        (bool Succeeded, string Error) SendAttachmentEmail(CustomerMessageModel customerMessageModel, IFormFile File);
    }
}
