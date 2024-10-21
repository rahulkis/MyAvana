using Microsoft.Extensions.Configuration;
using MyAvana.DAL.Auth;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Contract;
using MyAvanaApi.Models.ViewModels;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    [DisallowConcurrentExecution]
    public class PushNotificationJob : IJob
    {
        private readonly AvanaContext _context;
        private readonly IConfiguration configuration;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        public PushNotificationJob(AvanaContext avanaContext, IConfiguration configuration, INotificationService notificationService, IEmailService emailService)
        {
            _context = avanaContext;
            this.configuration = configuration;
            _notificationService = notificationService;
            _emailService = emailService;
        }
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var consultationList = _context.LiveConsultationUserDetails.Where(x => x.Date.ToShortDateString() == DateTime.Now.ToShortDateString() && x.Status == 1).ToList();

                EmailInformation emailInformation = new EmailInformation
                {
                    Email = "manish@mailinator.com",
                    Name = "",
                    Code = "1"
                };

                _emailService.SendEmail("RESETPWD", emailInformation);
                foreach (var item in consultationList)
                {
                    DateTime date1 = item.Date;
                    DateTime date2 = DateTime.Now;
                    TimeSpan ts = date1 - date2;
                    var diff = ts.TotalMinutes;
                    if (diff > 0 && diff < 5)
                    {
                        emailInformation.Code = "2";
                        _emailService.SendEmail("RESETPWD", emailInformation);
                        var user = _context.Users.Where(x => x.Email.ToLower() == item.UserEmail.ToLower()).FirstOrDefault();
                        if (!string.IsNullOrEmpty(user.DeviceId))
                        {
                            var bodyText = "Reminder! Your consultation is going to start in next 5 minutes";
                            NotificationModel notificationModel = new NotificationModel();
                            notificationModel.Title = "Consultation Reminder";
                            notificationModel.Body = bodyText;
                            notificationModel.DeviceId = user.DeviceId;
                            var result = _notificationService.SendNotification(notificationModel);
                        }
                    }
                    else
                    {
                        emailInformation.Code = "3";
                        _emailService.SendEmail("RESETPWD", emailInformation);
                    }
                }

                //}
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                EmailInformation emailInformation = new EmailInformation
                {
                    Email = "manish@mailinator.com",
                    Name = "",
                    Code = "4"
                };
                _emailService.SendEmail("RESETPWD", emailInformation);
                throw;
            }
        }
    }
}
