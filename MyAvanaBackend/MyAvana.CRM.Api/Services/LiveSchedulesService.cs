using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Contract;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAvana.CRM.Api.Services
{
    public class LiveSchedulesService : ILiveSchedules
    {
        private readonly AvanaContext _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;
        private readonly Logger.Contract.ILogger _logger;
        public LiveSchedulesService(AvanaContext avanaContext, UserManager<UserEntity> userManager, IEmailService emailService, INotificationService notificationService, Logger.Contract.ILogger logger)
        {
            _context = avanaContext;
            _userManager = userManager;
            _emailService = emailService;
            _notificationService = notificationService;
            _logger = logger;
        }
        public bool ChangeIsApproved(LiveConsultationUserDetails liveConsultationUserDetails)
        {
            try
            {
                var objCode = _context.LiveConsultationUserDetails.Where(x => x.LiveConsultationUserDetailsId == liveConsultationUserDetails.LiveConsultationUserDetailsId).FirstOrDefault();
                objCode.isApproved = false;

                string accountSid = "ACc1ce2b511c22f19ddb487aa4ef2f5d0c";
                string authToken = "2673b39855a14d9637c2d06c5d91ac55";


                //            if (liveConsultationUserDetails.Status == 1)
                //            {
                //                try
                //                {
                //                    TwilioClient.Init(accountSid, authToken);
                //                    var message = MessageResource.Create(

                //                       body: "Hi!Your LIVE Appointment is approved.",
                //                       from: new Twilio.Types.PhoneNumber("+12702149248"),
                //                       //to: new Twilio.Types.PhoneNumber("+918979195552"),
                //                       to: new Twilio.Types.PhoneNumber("+91" + liveConsultationUserDetails.ContactNo)
                //                   );
                //                    return true;
                //                }
                //                catch (Exception Ex)
                //                {
                //                    return false;
                //                }

                //            }
                //            else if (liveConsultationUserDetails.Status == 2)
                //            {
                //                TwilioClient.Init(accountSid, authToken);
                //                var message = MessageResource.Create(

                //body: "Hi!Your LIVE Appointment is Not Approved.",
                //from: new Twilio.Types.PhoneNumber("+12702149248"),
                ////to: new Twilio.Types.PhoneNumber("+918979195552"),
                //to: new Twilio.Types.PhoneNumber("+91" + liveConsultationUserDetails.ContactNo)
                //); return true;
                //            }
                //            else if (liveConsultationUserDetails.Status == 3)
                //            {
                //                TwilioClient.Init(accountSid, authToken);
                //                var message = MessageResource.Create(

                //body: "Hi!Your LIVE Appointment is Cancelled.",
                //from: new Twilio.Types.PhoneNumber("+12702149248"),
                ////to: new Twilio.Types.PhoneNumber("+918979195552"),
                //to: new Twilio.Types.PhoneNumber("+91" + liveConsultationUserDetails.ContactNo)
                //); return true;
                //            }
                //            else
                //            {
                //            }

                if (liveConsultationUserDetails.Status == 1)
                {
                    objCode.isApproved = true;
                    objCode.assignedTo = liveConsultationUserDetails.assignedTo;
                }
                objCode.Status = liveConsultationUserDetails.Status;
                _context.SaveChanges();

                var user = _context.Users.Where(x => x.Email.ToLower() == objCode.UserEmail.ToLower()).FirstOrDefault();

                if (liveConsultationUserDetails.Status == 0 || liveConsultationUserDetails.Status == 1)
                {
                    try
                    {
                        if(liveConsultationUserDetails.Status == 1)
                        {
                            EmailInformation emailInformation = new EmailInformation
                            {
                                Email = user.Email,
                                Name = user.FirstName + " " + user.LastName,
                            };

                            _emailService.SendEmail("CONAPP", emailInformation);

                            if (!string.IsNullOrEmpty(user.DeviceId))
                            {
                                var bodyText = "Congrats! Your consultation is approved";
                                NotificationModel notificationModel = new NotificationModel();
                                notificationModel.Title = "Consultation Approved";
                                notificationModel.Body = bodyText;
                                notificationModel.DeviceId = user.DeviceId;
                                var result = _notificationService.SendNotification(notificationModel);
                            }
                        }
                        

                        // Send email to assigned person
                        EmailInformation emailInformationassignto = new EmailInformation
                        {
                            Email = liveConsultationUserDetails.assignedTo,
                        };

                        _emailService.SendEmail("CONASSIGN", emailInformationassignto);
                    }
                    catch (Exception Ex)
                    {
                        return false;
                    }
                }
                else if(liveConsultationUserDetails.Status == 3)
                {
                    try
                    {
                        EmailInformation emailInformation = new EmailInformation
                        {
                            Email = user.Email,
                            Name = user.FirstName + " " + user.LastName,

                        };

                        _emailService.SendEmail("CONCANCUST", emailInformation);

                        // Send email to assigned person
                        EmailInformation emailInformationassignto = new EmailInformation
                        {
                            Email = liveConsultationUserDetails.assignedTo,
                        };

                        _emailService.SendEmail("CONCANADM", emailInformationassignto);
                    }
                    catch (Exception Ex)
                    {
                        return false;
                    }
                }
                else if (liveConsultationUserDetails.Status == 4)
                {
                    try
                    {
                        EmailInformation emailInformation = new EmailInformation
                        {
                            Email = user.Email,
                            Name = user.FirstName + " " + user.LastName,

                        };

                        _emailService.SendEmail("CONCOMPCUST", emailInformation);

                        // Send email to assigned person
                        EmailInformation emailInformationassignto = new EmailInformation
                        {
                            Email = liveConsultationUserDetails.assignedTo,
                        };

                        _emailService.SendEmail("CONCOMPADM", emailInformationassignto);
                    }
                    catch (Exception Ex)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: ChangeIsApproved, Error: " + ex.Message, ex);
                return false;
            }

        }


        public List<GetCustomerScheduleDetails> GetConsultationList()
        {
            try
            {
                var LiveConsultationUserDetails = _context.LiveConsultationUserDetails.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();

                var LiveConsultation = (from s in LiveConsultationUserDetails // outer sequence
                                        join st in _context.UserEntity on s.UserEmail equals st.Email

                                        select new GetCustomerScheduleDetails
                                        {
                                            LiveConsultationUserDetailsId = s.LiveConsultationUserDetailsId,
                                            ContactNo = s.ContactNo,
                                            dates = s.Date.GetDateTimeFormats('D'),
                                            FocusAreaDescription = s.FocusAreaDescription,
                                            TimeZone = s.TimeZone,
                                            UserEmail = s.UserEmail,
                                            Time = s.Time,
                                            FirstName = st.FirstName,
                                            LastName = st.LastName,
                                            Status = s.Status,
                                            Date = s.Date,
                                            CustomerType = st.CustomerType,
                                            AssignedTo = s.assignedTo

                                        }).OrderByDescending(x => x.Date).ToList();
                return LiveConsultation;
            }
            catch(Exception Ex)
            {
                _logger.LogError("Method: GetConsultationList, Error: " + Ex.Message, Ex);
                return null;
            }
        }
        public TimeZoneLiveSchedule GetTime()
        {
            try
            {
                TimeZoneLiveSchedule timeScheduleModel = new TimeZoneLiveSchedule();
                timeScheduleModel.Time = _context.ScheduleTimes.Where(x => x.IsActive == true).ToList();
                timeScheduleModel.TimeZones = _context.TimeZones.Where(x => x.IsActive == true).ToList();

                return timeScheduleModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetTime, Error: " + ex.Message, ex);
                return null;
            }
        }



        public LiveConsultationUserDetails SaveConsultationDetails(LiveConsultationUserDetails LiveConsultationUserDetails)
        {
            try
            {
                LiveConsultationUserDetails liveConsultationUserDetails = _context.LiveConsultationUserDetails.Where(x => x.UserEmail == LiveConsultationUserDetails.UserEmail && x.IsActive == true).FirstOrDefault();

                if (liveConsultationUserDetails != null)
                {
                    liveConsultationUserDetails.IsActive = false;
                }
                _context.LiveConsultationUserDetails.Add(new LiveConsultationUserDetails()
                {
                    ContactNo = LiveConsultationUserDetails.ContactNo,
                    Time = LiveConsultationUserDetails.Time,
                    Date = LiveConsultationUserDetails.Date,
                    UserEmail = LiveConsultationUserDetails.UserEmail,
                    FocusAreaDescription = LiveConsultationUserDetails.FocusAreaDescription,
                    IsActive = true,
                    TimeZone = LiveConsultationUserDetails.TimeZone,
                    IsPaid = true,
                    userId = LiveConsultationUserDetails.userId,
                    assignedTo = LiveConsultationUserDetails.assignedTo
                });
                _context.SaveChanges();
                var user = _context.Users.Where(x => x.Email.ToLower() == LiveConsultationUserDetails.UserEmail.ToLower()).FirstOrDefault();
                EmailInformation emailInformation = new EmailInformation
                {
                    Email = user.Email,
                    Name = user.FirstName + " " + user.LastName,

                };

                _emailService.SendEmail("CONSCH", emailInformation);

                // Send email to Admin
                EmailInformation emailInformationadmin = new EmailInformation
                {
                    //Email = "admin@myavana.com",
                    Name = "Admin",

                };
                _emailService.SendEmail("CONREQ", emailInformationadmin);

                return LiveConsultationUserDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveConsultationDetails, Email:" + LiveConsultationUserDetails.UserEmail + ", Error: " + ex.Message, ex);
                return null;
            }



        }
        public GetCustomerScheduleDetails FetchConsultationDetails(LiveConsultationUserDetails LiveConsultationUserDetails)
        {
            try
            {
                // LiveConsultationUserDetails liveConsultationUserDetails = _context.LiveConsultationUserDetails.Where(x => x.UserEmail == LiveConsultationUserDetails.UserEmail && x.IsActive == true).FirstOrDefault();

                var liveConsultationUserDetails = _context.LiveConsultationUserDetails.Where(x => x.UserEmail == LiveConsultationUserDetails.UserEmail && x.IsActive == true && x.Status != 4).ToList();

                var LiveConsultation = (from s in liveConsultationUserDetails // outer sequence
                                        join st in _context.UserEntity on s.UserEmail equals st.Email

                                        select new GetCustomerScheduleDetails
                                        {
                                            LiveConsultationUserDetailsId = s.LiveConsultationUserDetailsId,
                                            ContactNo = s.ContactNo,
                                            FocusAreaDescription = s.FocusAreaDescription,
                                            TimeZone = s.TimeZone,
                                            UserEmail = s.UserEmail,
                                            Time = s.Time,
                                            FirstName = st.FirstName,
                                            LastName = st.LastName,
                                            Status = s.Status,
                                            Date = s.Date,
                                            IsPaid = s.IsPaid

                                        }).LastOrDefault();

                return LiveConsultation;

                // return liveConsultationUserDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: FetchConsultationDetails, Email:" + LiveConsultationUserDetails.UserEmail + ", Error: " + ex.Message, ex);
                return null;
            }

        }
        public LiveConsultationModel JoinLiveConsultation(LiveConsultationModel liveInterviewModel)
        {

            try
            {

                LiveConsultationUserDetails interviewCandidate = _context.LiveConsultationUserDetails.Where(x => x.LiveConsultationUserDetailsId == liveInterviewModel.LiveConsultationUserDetailsId).FirstOrDefault();
                //if (interviewCandidate.InterviewStatusId == 5)
                //{
                //    liveInterviewModel.ExpireInterview = true;
                //}
                //else if (interviewCandidate.InterviewStatusId == 1)
                //{
                //    liveInterviewModel.CompleteInterview = true;
                //}
                //else
                //{
                LiveConsultationCustomer objLiveInterviewInfo = _context.LiveConsultationCustomer.Where(x => x.IsActive == true && x.LiveConsultationUserDetailsId == liveInterviewModel.LiveConsultationUserDetailsId).FirstOrDefault();

                if (objLiveInterviewInfo != null)
                {
                    if (liveInterviewModel.UpdateCustomer)
                    {
                        if (objLiveInterviewInfo.IsCustomerJoined == true && objLiveInterviewInfo.IsLeft == true)
                        {
                            liveInterviewModel.TwilioRoomName = objLiveInterviewInfo.TwilioRoomName;
                            liveInterviewModel.TwilioRoomSid = objLiveInterviewInfo.TwilioRoomSid;
                            liveInterviewModel.CustomerToken = objLiveInterviewInfo.CustomerToken;
                            liveInterviewModel.InformationId = objLiveInterviewInfo.LiveConsultationCustomerId;
                            liveInterviewModel.IsLeft = objLiveInterviewInfo.IsLeft;
                            objLiveInterviewInfo.IsLeft = false;
                        }
                        else if (objLiveInterviewInfo.IsCustomerJoined == true)
                        {
                            liveInterviewModel.AlreadyJoined = true;
                        }
                        else
                        {
                            objLiveInterviewInfo.IsCustomerJoined = true;

                            // Send email to Admin
                            EmailInformation emailInformationadmin = new EmailInformation
                            {
                                //Email = "admin@myavana.com",
                                Name = "Admin",

                            };
                            _emailService.SendEmail("CUSTJOIN", emailInformationadmin);
                        }
                        objLiveInterviewInfo.CustomerJoinDateTime = DateTime.UtcNow;
                        liveInterviewModel.IsAdminJoined = objLiveInterviewInfo.IsAdminJoined;
                        liveInterviewModel.IsCustomerJoined = true;
                        liveInterviewModel.CustomerJoinDatetime = objLiveInterviewInfo.CustomerJoinDateTime;
                        liveInterviewModel.AdminJoinDatetime = objLiveInterviewInfo.AdminJoinDateTime;
                        objLiveInterviewInfo.customerId = liveInterviewModel.CustomerId;

                    }
                    else
                    {
                        if (objLiveInterviewInfo.IsAdminJoined == true)
                        {
                            liveInterviewModel.AlreadyJoined = true;
                        }
                        else
                        {
                            if (objLiveInterviewInfo.IsAdminJoined == true)
                            {
                                liveInterviewModel.AlreadyJoined = true;
                            }
                            else
                            {
                                objLiveInterviewInfo.IsAdminJoined = true;
                                liveInterviewModel.IsAdminJoined = true;
                                objLiveInterviewInfo.AdminJoinDateTime = DateTime.UtcNow;
                                liveInterviewModel.IsCustomerJoined = objLiveInterviewInfo.IsCustomerJoined;
                                liveInterviewModel.CustomerJoinDatetime = objLiveInterviewInfo.CustomerJoinDateTime;
                                //objLiveInterviewInfo.EmployerId = liveInterviewModel.EmployerId;

                                // Send email to Customer
                                var user = _context.Users.Where(x => x.Email.ToLower() == interviewCandidate.UserEmail.ToLower()).FirstOrDefault();
                                EmailInformation emailInformation = new EmailInformation
                                {
                                    Email = user.Email,
                                    Name = user.FirstName + " " + user.LastName,

                                };

                                _emailService.SendEmail("ADMJOIN", emailInformation);
                                if (!string.IsNullOrEmpty(user.DeviceId))
                                {
                                    var bodyText = "Hurry up! Your consultant was joined.";
                                    NotificationModel notificationModel = new NotificationModel();
                                    notificationModel.Title = "Consultant Joined";
                                    notificationModel.Body = bodyText;
                                    notificationModel.DeviceId = user.DeviceId;
                                    var result = _notificationService.SendNotification(notificationModel);
                                }
                            }
                        }
                    }
                    // liveInterviewModel.CustomerName = _context.User.Where(x => x.UserId == objLiveInterviewInfo.CandidateId).FirstOrDefault().FullName;
                    // liveInterviewModel.EmployerName = _context.User.Where(x => x.UserId == objLiveInterviewInfo.EmployerId).FirstOrDefault().FullName;

                }
                else
                {
                    objLiveInterviewInfo = new LiveConsultationCustomer();
                    objLiveInterviewInfo.LiveConsultationUserDetailsId = liveInterviewModel.LiveConsultationUserDetailsId;
                    objLiveInterviewInfo.IsActive = true;
                    objLiveInterviewInfo.IsCompleted = false;
                    objLiveInterviewInfo.CreatedDate = DateTime.UtcNow;
                    if (liveInterviewModel.UpdateCustomer)
                    {
                        var userDetail = _userManager.FindByEmailAsync(liveInterviewModel.UserEmail).GetAwaiter().GetResult();
                        objLiveInterviewInfo.customerId = userDetail.Id.ToString();
                        objLiveInterviewInfo.IsCustomerJoined = true;
                        liveInterviewModel.IsCustomerJoined = true;
                        objLiveInterviewInfo.CustomerJoinDateTime = DateTime.UtcNow;
                        liveInterviewModel.CustomerJoinDatetime = objLiveInterviewInfo.CustomerJoinDateTime;
                        liveInterviewModel.IsAdminJoined = false;

                        // Send email to Admin
                        EmailInformation emailInformationadmin = new EmailInformation
                        {
                            //Email = "admin@myavana.com",
                            Name = "Admin",

                        };
                        _emailService.SendEmail("CUSTJOIN", emailInformationadmin);
                    }
                    else
                    {
                        objLiveInterviewInfo.adminId = liveInterviewModel.AdminId;
                        objLiveInterviewInfo.IsAdminJoined = true;
                        liveInterviewModel.IsAdminJoined = true;
                        objLiveInterviewInfo.AdminJoinDateTime = DateTime.UtcNow;
                        liveInterviewModel.AdminJoinDatetime = objLiveInterviewInfo.AdminJoinDateTime;
                        liveInterviewModel.IsCustomerJoined = false;

                        // Send email to Customer
                        var user = _context.Users.Where(x => x.Email.ToLower() == interviewCandidate.UserEmail.ToLower()).FirstOrDefault();
                        EmailInformation emailInformation = new EmailInformation
                        {
                            Email = user.Email,
                            Name = user.FirstName + " " + user.LastName,

                        };

                        _emailService.SendEmail("ADMJOIN", emailInformation);
                        if (!string.IsNullOrEmpty(user.DeviceId))
                        {
                            var bodyText = "Hurry up! Your consultant was joined.";
                            NotificationModel notificationModel = new NotificationModel();
                            notificationModel.Title = "Consultant Joined";
                            notificationModel.Body = bodyText;
                            notificationModel.DeviceId = user.DeviceId;
                            var result = _notificationService.SendNotification(notificationModel);
                        }
                    }
                    _context.LiveConsultationCustomer.Add(objLiveInterviewInfo);
                }

                _context.SaveChanges();
                liveInterviewModel.InformationId = objLiveInterviewInfo.LiveConsultationCustomerId;
                // }
                return liveInterviewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: JoinLiveConsultation, LiveConsultationUserDetailsId:" + liveInterviewModel.LiveConsultationUserDetailsId + ", Error: " + ex.Message, ex);
                liveInterviewModel.TwilioRoomName += ex.Message;
                return liveInterviewModel;
            }

        }
        public LiveConsultationModel CheckIsOtherParticipantReady(LiveConsultationModel liveInterviewModel)
        {

            try
            {
                LiveConsultationCustomer objLiveInterviewInfo;
                if (liveInterviewModel.UpdateCustomer)
                {
                    objLiveInterviewInfo = _context.LiveConsultationCustomer.Where(x => x.LiveConsultationCustomerId == liveInterviewModel.InformationId && x.IsCustomerJoined == true).FirstOrDefault();
                }
                else
                {
                    objLiveInterviewInfo = _context.LiveConsultationCustomer.Where(x => x.LiveConsultationCustomerId == liveInterviewModel.InformationId && x.IsAdminJoined == true).FirstOrDefault();
                }
                if (objLiveInterviewInfo != null)
                {
                    return new LiveConsultationModel
                    {
                        IsCustomerJoined = objLiveInterviewInfo.IsCustomerJoined,
                        IsAdminJoined = objLiveInterviewInfo.IsAdminJoined,
                        //CandidateName = objLiveInterviewInfo.Candidate.FullName,
                        CustomerJoinDatetime = objLiveInterviewInfo.CustomerJoinDateTime,
                        AdminJoinDatetime = objLiveInterviewInfo.AdminJoinDateTime,
                        //EmployerName = objLiveInterviewInfo.Employer.FullName,
                        CustomerToken = objLiveInterviewInfo.CustomerToken,
                        AdminToken = objLiveInterviewInfo.AdminToken,
                        TwilioRoomName = objLiveInterviewInfo.TwilioRoomName,
                        TwilioRoomSid = objLiveInterviewInfo.TwilioRoomSid
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: CheckIsOtherParticipantReady, Error: " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }
        public LiveConsultationModel UpdateLiveConsultationInformation(LiveConsultationModel liveInterviewModel)
        {
            try
            {
                LiveConsultationCustomer objLiveInterviewInfo = _context.LiveConsultationCustomer.Where(x => x.LiveConsultationCustomerId == liveInterviewModel.InformationId).FirstOrDefault();

                if (objLiveInterviewInfo != null)
                {
                    if (liveInterviewModel.UpdateComposition)
                    {
                        objLiveInterviewInfo.TwilioCompositionSid = liveInterviewModel.TwilioCompositionSid;
                        objLiveInterviewInfo.TwilioCompositionStatus = liveInterviewModel.TwilioCompositionStatus;
                    }
                    else if (liveInterviewModel.CompleteConsultation)
                    {
                        objLiveInterviewInfo.CustomerParticipantId = liveInterviewModel.CustomerParticipantId;
                        objLiveInterviewInfo.AdminParticipantId = liveInterviewModel.AdminParticipantId;
                        objLiveInterviewInfo.IsCompleted = liveInterviewModel.IsCompleted;
                        objLiveInterviewInfo.CompletedDateTime = DateTime.UtcNow;
                        //objLiveInterviewInfo.TwilioCompositionSid = liveInterviewModel.TwilioCompositionSid;
                        //objLiveInterviewInfo.TwilioCompositionStatus = liveInterviewModel.TwilioCompositionStatus;
                        LiveConsultationUserDetails objInterview = _context.LiveConsultationUserDetails.Where(x => x.LiveConsultationUserDetailsId == objLiveInterviewInfo.LiveConsultationUserDetailsId).FirstOrDefault();
                        objInterview.Status = 4;

                    }
                    else if (liveInterviewModel.IsLeft == true)
                    {
                        objLiveInterviewInfo.IsLeft = true;
                        objLiveInterviewInfo.LeftDateTime = DateTime.UtcNow;
                    }
                    else
                    {
                        objLiveInterviewInfo.TwilioRoomName = liveInterviewModel.TwilioRoomName;
                        objLiveInterviewInfo.TwilioRoomSid = liveInterviewModel.TwilioRoomSid;
                        objLiveInterviewInfo.CustomerToken = liveInterviewModel.CustomerToken;
                        objLiveInterviewInfo.AdminToken = liveInterviewModel.AdminToken;
                    }
                    _context.SaveChanges();

                }
                return liveInterviewModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: UpdateLiveConsultationInformation, LiveConsultationCustomerId:" + liveInterviewModel.InformationId + ", Error: " + ex.Message, ex);
                liveInterviewModel.TwilioRoomName += ex.Message;
                return liveInterviewModel;
            }
        }
    }
}
