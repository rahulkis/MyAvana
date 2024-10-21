using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MyAvana.DAL.Auth;
using MyAvana.Logger.Contract;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Contract;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace MyAvanaApi.Services
{
    public class EmailService : IEmailService
    {
        public readonly AvanaContext _context;
        public readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _adminStagingUrl;
        public EmailService(AvanaContext context, ILogger logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _adminStagingUrl = configuration["AdminStagingUrl"];
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
            if (result.TemplateCode == "HHCPUPDT" || result.TemplateCode== "HAIRANALYSISSTATUSUPDATE" || result.TemplateCode == "HAIRAIPURCHASE")
            {
                body = _context.GenericSettings.Where(s => s.SettingName == "EMAILTEMPLATE02").Select(s => s.DefaultTextMax).FirstOrDefault();
            }
            else if(result.TemplateCode == "REG" || result.TemplateCode == "FGTPASS" || result.TemplateCode == "CONSCH" || result.TemplateCode == "REGHAIRKITUSER" ||result.TemplateCode == "FGTADMINPASS" || result.TemplateCode == "REGISTERUSER" 
            || result.TemplateCode == "ASSIGNCUST" || result.TemplateCode == "SHAREHHCP")
            {
                body = _context.GenericSettings.Where(s => s.SettingName == "EMAILTEMPLATE04").Select(s => s.DefaultTextMax).FirstOrDefault();
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
                    if (result.TemplateCode == "CONREQ" || result.TemplateCode == "CUSTJOIN" || result.TemplateCode == "GROUPREQUEST")
                    {
                        body = body.Replace("#User#", emailInformation.Name);
                        emailInformation.Email = "karen@myavana.com,support@myavana.com";
                    }
                    else if(result.TemplateCode == "CONASSIGN" || result.TemplateCode == "CONCANADM" || result.TemplateCode == "CONCOMPADM")
                    {
                        body = body.Replace("#User#", "Admin,");
                    }
                    else if(result.TemplateCode == "REGUSER" || result.TemplateCode == "FGTADMINPASS")
                    {
                        body = body.Replace("#User#", emailInformation.Name);
                    }
                    else if (result.TemplateCode == "COMPQA" || result.TemplateCode == "POSTREPORT")
                    {
                        body = body.Replace("#User#", "Admin,");
                        emailInformation.Email = "support@myavana.com,support3@mailinator.com";
                    }
                    else if(result.TemplateCode == "ASSIGNCUST")
                    {
                        body = body.Replace("#User#", emailInformation.GroupName);
                    }
                    else
                    {
                        body = body.Replace("#User#", userInfo.FirstName + " " + userInfo.LastName);

                    }
                } 
                if (body.IndexOf("#ImageSrc#") > 0)
                {
                    if (result.TemplateCode == "FGTPASS")
                    {
                        body = body.Replace("#ImageSrc#", _adminStagingUrl + "EmailTemplate/images/img3.png");
                    }
                    else if (result.TemplateCode == "CONSCH")
                    {
                        body = body.Replace("#ImageSrc#", _adminStagingUrl + "EmailTemplate/images/img.png");
                    }
                    else
                    {
                        body = body.Replace("#ImageSrc#", _adminStagingUrl + "EmailTemplate/images/img2.png");
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
                if (body.IndexOf("#username#") > 0)
                {
                    body = body.Replace("#username#", emailInformation.Email);
                }
                if (body.IndexOf("#password#") > 0)
                {
                    body = body.Replace("#password#", emailInformation.Code);
                }
                if (body.IndexOf("#usertype#") > 0)
                {
                    body = body.Replace("#usertype#", emailInformation.Name);
                }
                if (body.IndexOf("#useremail#") > 0)
                {
                    body = body.Replace("#useremail#", emailInformation.Code);
                }
                if (body.IndexOf("#post#") > 0)
                {
                    body = body.Replace("#post#", emailInformation.Code);
                }
                if (body.IndexOf("#group#") > 0)
                {
                    body = body.Replace("#group#", emailInformation.GroupName);
                }
                if (body.IndexOf("#customer#") > 0)
                {
                    body = body.Replace("#customer#", emailInformation.userEmail);
                }
                if (body.IndexOf("#plan#") > 0)
                {
                    body = body.Replace("#plan#", emailInformation.Code);
                }
                if (body.IndexOf("#loginuser#") > 0)
                {
                    body = body.Replace("#loginuser#", emailInformation.Name);
                }
                if (body.IndexOf("#assignUserName#") > 0 || body.IndexOf("#assignUserEmail#") > 0)
                {
                    body = body.Replace("#assignUserName#", emailInformation.Name);
                    body = body.Replace("#assignUserEmail#", emailInformation.userEmail);
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

        public (bool Succeeded, string Error) SendAttachmentEmail(CustomerMessageModel customerMessageModel, IFormFile File)
        {
            bool flag = false;
            string error = "";
            string body = "";
            try
            {
                EmailTemplate emailTemplateModel = _context.EmailTemplates.Where(p => p.TemplateCode == "CUSTMSG").FirstOrDefault();
                body = _context.GenericSettings.Where(s => s.SettingName == "EMAILTEMPLATE01").Select(s => s.DefaultTextMax).FirstOrDefault();

                if (body.IndexOf("#Content#") > 0)
                {
                    body = body.Replace("#Content#", customerMessageModel.Message);
                }

                if (body.IndexOf("#User#") > 0)
                {
                    body = body.Replace("#User#", customerMessageModel.UserName);
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
                message.Body = body;
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
                return (true, error);
            }
            catch (Exception Ex)
            {
                _logger.LogError(Ex.Message);
                return (flag, Ex.Message);
            }
        }
    }
}
