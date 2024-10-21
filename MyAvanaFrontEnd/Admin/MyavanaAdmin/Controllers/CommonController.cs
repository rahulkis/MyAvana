using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using MyavanaAdminModels;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MyavanaAdmin.Controllers
{
    public class CommonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public static void SendEmailToCustomer(CustomerMessageModel customerMessageModel, EmailTemplate emailTemplateModel, IFormFile File)
        {
            string error = "";
            try
            {
                if (customerMessageModel.emailBody.IndexOf("#Content#") > 0)
                {
                    customerMessageModel.emailBody = customerMessageModel.emailBody.Replace("#Content#", customerMessageModel.Message);
                }

                if (customerMessageModel.emailBody.IndexOf("#User#") > 0)
                {
                    customerMessageModel.emailBody = customerMessageModel.emailBody.Replace("#User#", customerMessageModel.UserName);
                }


                SmtpClient smtp = new SmtpClient
                {
                    Host = emailTemplateModel.HostName,
                    Port = emailTemplateModel.HostPort,
                    EnableSsl = true,//result.EnableSSL,

                    Credentials = new System.Net.NetworkCredential(emailTemplateModel.SMTPUsername, emailTemplateModel.SMTPPassword),
                };
                MailMessage message = new MailMessage();

                message.From = new MailAddress(emailTemplateModel.SenderEmail, emailTemplateModel.SenderName);
                message.IsBodyHtml = true;
                message.Subject = customerMessageModel.Subject;
                message.To.Add(customerMessageModel.EmailAddress);
                message.Body = customerMessageModel.emailBody;
                if (customerMessageModel.AttachmentFile != null)
                {
                    if (File.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            File.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            Attachment att = new Attachment(new MemoryStream(fileBytes), File.FileName);
                            message.Attachments.Add(att);
                        }
                    }
                }

                smtp.Send(message);
                error = "Success";
            }
            catch (Exception Ex)
            {
                error = Ex.ToString();
            }
        }
    }
}