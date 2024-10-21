using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class HairAnalysisStatusService : IHairAnalysisStatusService
    {
        private readonly AvanaContext _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IEmailService _emailService;
        private readonly Logger.Contract.ILogger _logger;
        private readonly INotificationService _notificationService;
        public HairAnalysisStatusService(AvanaContext avanaContext, UserManager<UserEntity> userManager, IEmailService emailService, Logger.Contract.ILogger logger, INotificationService notificationService)
        {
            _context = avanaContext;
            _userManager = userManager;
            _emailService = emailService;
            _notificationService = notificationService;
            _logger = logger;
        }

        public List<HairAnalysisStatusHistoryList> GetHairAnalysisStatusHistoryList(Guid userId)
        {
            try
            {
                var statusHistory = (from m in _context.HairAnalysisStatusHistory.Include(x => x.NewHairAnalysisStatus)
                                     .Include(x => x.OldHairAnalysisStatus)
                                     .Include(x => x.UpdatedBy)
                                     where m.CustomerId == userId
                                     select new HairAnalysisStatusHistoryList()
                                     {
                                         StatusName = m.NewHairAnalysisStatus.StatusName,
                                         OldStatusName = m.OldHairAnalysisStatus.StatusName,
                                         UpdatedByUser = m.UpdatedBy.UserEmail,
                                         CreatedOn = m.CreatedOn
                                     }).ToList();
                return statusHistory;
            }

            catch (Exception ex)
            {
                _logger.LogError("Method: GetHairAnalysisStatusHistoryList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public List<HairAnalysisStatusModel> GetHairAnalysisStatusList()
        {
            try
            {

                List<HairAnalysisStatusModel> status = (from m in _context.HairAnalysisStatus
                                                        select new HairAnalysisStatusModel()
                                                        {
                                                            HairAnalysisStatusId = m.HairAnalysisStatusId,
                                                            StatusName = m.StatusName

                                                        }).ToList();
                return status;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetHairAnalysisStatus, Error: " + ex.Message, ex);
                return null;
            }
        }

        public List<StatusTrackerModel> GetStatusTrackerList()
        {
            try
            {

                List<StatusTrackerModel> statusTrackers = (from m in _context.StatusTracker
                                                           join st in _context.UserEntity on m.CustomerId equals st.Id
                                                           join hr in _context.HairAnalysisStatus on m.HairAnalysisStatusId equals hr.HairAnalysisStatusId
                                                           select new StatusTrackerModel()
                                                           {
                                                               StatusTrackerId = m.StatusTrackerId,
                                                               CustomerName = st.FirstName + " " + st.LastName,
                                                               HairAnalysisStatus = hr.StatusName,
                                                               CustomerId = m.CustomerId.ToString(),
                                                               CustomerEmail = st.Email,
                                                               KitSerialNumber = m.KitSerialNumber,
                                                           }).ToList();
                return statusTrackers;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetStatusTrackerList, Error: " + ex.Message, ex);
                return null;
            }
        }
        public StatusTrackerModel ChangeHairAnalysisStatus(StatusTrackerModel trackerModel)
        {

            var tracker = _context.StatusTracker.Where(x => x.StatusTrackerId == trackerModel.StatusTrackerId).FirstOrDefault();

            try
            {
                if (tracker != null)
                {
                    HairAnalysisStatusHistory hairAnalysisStatusHistory = new HairAnalysisStatusHistory();
                    hairAnalysisStatusHistory.OldHairAnalysisStatusId = tracker.HairAnalysisStatusId;

                    tracker.HairAnalysisStatusId = trackerModel.HairAnalysisStatusId;
                    tracker.LastModifiedBy = trackerModel.LastModifiedBy;
                    tracker.LastUpdatedOn = trackerModel.LastUpdatedOn;
                    _context.Update(tracker);


                    hairAnalysisStatusHistory.NewHairAnalysisStatusId = trackerModel.HairAnalysisStatusId;
                    hairAnalysisStatusHistory.IsActive = true;
                    hairAnalysisStatusHistory.CreatedOn = DateTime.Now;
                    hairAnalysisStatusHistory.CustomerId = tracker.CustomerId;
                    hairAnalysisStatusHistory.UpdatedByUserId = !string.IsNullOrEmpty(trackerModel.LastModifiedBy) ? Convert.ToInt32(trackerModel.LastModifiedBy) : 0;

                    _context.HairAnalysisStatusHistory.Add(hairAnalysisStatusHistory);
                    _context.SaveChanges();

                    // Send email to Customer
                    UserEntity user = _context.Users.Where(x => x.Id == tracker.CustomerId).FirstOrDefault();
                    string statusName = _context.HairAnalysisStatus.Where(x => x.HairAnalysisStatusId == trackerModel.HairAnalysisStatusId).Select(x => x.StatusName).FirstOrDefault();
                    EmailInformation emailInformation = new EmailInformation
                    {
                        Email = user.Email,
                        Name = user.FirstName + " " + user.LastName,
                        Code = statusName
                    };

                    _emailService.SendEmail("HAIRANALYSISSTATUSUPDATE", emailInformation);
                    
                    if (!string.IsNullOrEmpty(user.DeviceId))
                    {
                        var bodyText = "Your Hair Analysis Status has just been updated to " + statusName;
                        NotificationModel notificationModel = new NotificationModel();
                        notificationModel.Title = "Hair Analysis Status Update";
                        notificationModel.Body = bodyText;
                        notificationModel.DeviceId = user.DeviceId;
                        var result = _notificationService.SendNotification(notificationModel);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError("Method: ChangeHairAnalysisStatus, StatusTrackerId:" + trackerModel.StatusTrackerId + ", Error: " + ex.Message, ex);
            }

            return trackerModel;
        }
        public (bool success, string error) SaveHairAnalysisStatus(StatusTracker tracker)
        {
            try
            {
                var statusTrackerModel = _context.StatusTracker.FirstOrDefault(x => x.CustomerId == tracker.CustomerId);
                if (statusTrackerModel != null)
                {
                    statusTrackerModel.HairAnalysisStatusId = tracker.HairAnalysisStatusId;
                    statusTrackerModel.LastUpdatedOn = DateTime.Now;
                    statusTrackerModel.LastModifiedBy = tracker.LastModifiedBy;
                    // statusTrackerModel.KitSerialNumber = tracker.KitSerialNumber;

                }
                else
                {
                    StatusTracker obj = new StatusTracker();
                    obj.CustomerId = tracker.CustomerId;
                    obj.RegistrationDate = DateTime.Now;
                    obj.KitSerialNumber = tracker.KitSerialNumber;
                    obj.HairAnalysisStatusId = 1;
                    _context.Add(obj);
                }
                _context.SaveChanges();
                return (true, "");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: SaveHairAnalysisStatus, UserId:" + tracker.CustomerId + ", Error: " + Ex.Message, Ex);
                return (false, "Something went wrong. Please try again.");
            }
        }
        public StatusTracker AddToStatusTracker(StatusTracker trackerEntity)
        {

            try
            {
                    StatusTracker obj = new StatusTracker();
                    obj.CustomerId = trackerEntity.CustomerId;
                    //obj.RegistrationDate = DateTime.Now;
                    obj.HairAnalysisStatusId = trackerEntity.HairAnalysisStatusId;
                    obj.LastModifiedBy = trackerEntity.LastModifiedBy;
                    obj.LastUpdatedOn = trackerEntity.LastUpdatedOn;
                    _context.Add(obj);
                    

                HairAnalysisStatusHistory hairAnalysisStatusHistory = new HairAnalysisStatusHistory();
                
                    hairAnalysisStatusHistory.NewHairAnalysisStatusId = trackerEntity.HairAnalysisStatusId;
                    hairAnalysisStatusHistory.IsActive = true;
                    hairAnalysisStatusHistory.CreatedOn = DateTime.Now;
                    hairAnalysisStatusHistory.CustomerId = trackerEntity.CustomerId;
                    hairAnalysisStatusHistory.UpdatedByUserId = !string.IsNullOrEmpty(trackerEntity.LastModifiedBy) ? Convert.ToInt32(trackerEntity.LastModifiedBy) : 0;

                    _context.HairAnalysisStatusHistory.Add(hairAnalysisStatusHistory);
                    _context.SaveChanges();

                    // Send email to Customer
                    UserEntity user = _context.Users.Where(x => x.Id == trackerEntity.CustomerId).FirstOrDefault();
                    string statusName = _context.HairAnalysisStatus.Where(x => x.HairAnalysisStatusId == trackerEntity.HairAnalysisStatusId).Select(x => x.StatusName).FirstOrDefault();
                    EmailInformation emailInformation = new EmailInformation
                    {
                        Email = user.Email,
                        Name = user.FirstName + " " + user.LastName,
                        Code = statusName
                    };

                    _emailService.SendEmail("HAIRANALYSISSTATUSUPDATE", emailInformation);

                    if (!string.IsNullOrEmpty(user.DeviceId))
                    {
                        var bodyText = "Your Hair Analysis Status has just been updated to " + statusName;
                        NotificationModel notificationModel = new NotificationModel();
                        notificationModel.Title = "Hair Analysis Status Update";
                        notificationModel.Body = bodyText;
                        notificationModel.DeviceId = user.DeviceId;
                        var result = _notificationService.SendNotification(notificationModel);
                    }
              

            }
            catch (Exception ex)
            {
                _logger.LogError("Method: AddToStatusTracker, StatusTrackerId:" + trackerEntity.StatusTrackerId + ", Error: " + ex.Message, ex);
            }

            return trackerEntity;
        }
    }
}
