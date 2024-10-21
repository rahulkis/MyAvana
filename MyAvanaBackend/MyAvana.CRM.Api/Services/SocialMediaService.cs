using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Logger.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class SocialMediaService : ISocialMediaService
    {
        private readonly ILogger _logger;
        private readonly AvanaContext _context;
        public SocialMediaService(ILogger logger, AvanaContext avanaContext)
        {
            _logger = logger;
            _context = avanaContext;
        }
        public (JsonResult result, bool success, string error) GetTvLinks(string settingName, string subSettingName, int userId)
        {
            try
            {
                var userType = _context.WebLogins.FirstOrDefault(x => x.UserId == userId).UserTypeId;
                if (userType == (int)UserTypeEnum.B2B)
                {
                    var salonIds = _context.SalonsOwners.Where(x => x.UserId == userId && x.IsActive == true).Select(x => x.SalonId).ToArray();
                    var videoData = _context.MediaLinkEntities.Join(
                    _context.SalonVideos,
                    MediaLinkEntities => MediaLinkEntities.MediaLinkEntityId,
                    SalonVideos => SalonVideos.MediaLinkEntityId,
                    (MediaLinkEntities, SalonVideos) => new
                    {
                        MediaLinkEntityId = MediaLinkEntities.MediaLinkEntityId,
                        SalonId = SalonVideos.SalonId,
                        IsActive = SalonVideos.IsActive,
 ShowOnMobile = MediaLinkEntities.ShowOnMobile
                    }).Where(u => salonIds.Any(x => x == u.SalonId) && u.IsActive == true).ToList();

                    var userVideoIds = videoData.Select(x => x.MediaLinkEntityId).Distinct().ToArray();
                    //var result = _context.MediaLinkEntities.Where(x => x.IsActive == true && userVideoIds.Any(u => u == x.MediaLinkEntityId)).OrderByDescending(x => x.CreatedOn).ToList();

                    var result = (from ml in _context.MediaLinkEntities
                                  join cat in _context.VideoCategories
                                  on ml.VideoCategoryId equals cat.Id
                                  select new
                                  {
                                      CreatedOn = ml.CreatedOn,
                                      UserId = ml.Id,
                                      IsActive = ml.IsActive,
                                      VideoId = ml.VideoId,
                                      Title = ml.Title,
                                      ImageLink = ml.ImageLink,
                                      Description = ml.Description,
                                      Header = ml.Header,
                                      IsFeatured = ml.IsFeatured,
                                      MediaLinkEntityId = ml.MediaLinkEntityId,
                                      VideoCategoryId = ml.VideoCategoryId,
                                      VideoCategory = cat.Description,
                                      ShowOnMobile = ml.ShowOnMobile
                                  }).Where(ml => ml.IsActive == true && userVideoIds.Any(u => u == ml.MediaLinkEntityId)).ToList().OrderByDescending(ml => ml.CreatedOn);
                    if (result != null)
                        return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                    _logger.LogError("Method: GetTvLinks, UserId:" + userId + ", Error: No settings found.");
                    return (new JsonResult(""), false, "No settings found.");
                }
                else
                {
                    //var result = _context.MediaLinkEntities.Include(x=>x.VideoCategory).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();

                    var result = (from ml in _context.MediaLinkEntities
                                  join cat in _context.VideoCategories
                                  on ml.VideoCategoryId equals cat.Id
                                  select new
                                  {
                                      CreatedOn = ml.CreatedOn,
                                      Id = ml.Id,
                                      IsActive = ml.IsActive,
                                      VideoId = ml.VideoId,
                                      Title = ml.Title,
                                      ImageLink = ml.ImageLink,
                                      Description = ml.Description,
                                      Header = ml.Header,
                                      IsFeatured = ml.IsFeatured,
                                      MediaLinkEntityId = ml.MediaLinkEntityId,
                                      VideoCategoryId = ml.VideoCategoryId,
                                      VideoCategory = cat.Description,
                                      ShowOnMobile = ml.ShowOnMobile
                                  }).Where(ml=>ml.IsActive == true).ToList().OrderByDescending(ml => ml.CreatedOn);

                    if (result != null)
                        return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                    _logger.LogError("Method: GetTvLinks, UserId:" + userId + ", Error: No settings found.");
                    return (new JsonResult(""), false, "No settings found.");
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetTvLinks, UserId:" + userId + ", Error: " + Ex.Message, Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again.");
            }
        }

        public (JsonResult result, bool success, string error) GetVideoCategories()
        {
            try
            {
                var result = _context.VideoCategories.Where(x => x.IsActive == true).ToList();
                if (result != null)
                    return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                _logger.LogError("Method: GetVideoCategories, Error: No settings found.");
                return (new JsonResult(""), false, "No settings found.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetVideoCategories, Error: " + Ex.Message, Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again.");
            }
        }
        public (JsonResult result, bool success, string error) GetTvLinks2(string settingName, string subSettingName)
        {
            try
            {
                var result = _context.MediaLinkEntities.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();
                if (result != null)
                {
                    //foreach (var res in result)
                    //{
                    //    if (res.VideoId.Contains("instagram"))
                    //    {
                    //        res.ImageLink = "http://admin.myavana.com/images/instagram.jpg";
                    //    }
                    //}
                    return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                }
                _logger.LogError("Method: GetTvLinks2, Error: No settings found.");
                return (new JsonResult(""), false, "No settings found.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetTvLinks2, Error: " + Ex.Message, Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again.");
            }
        }

        public (JsonResult result, bool success, string error) GetTvLinksCategories(string settingName, string subSettingName)
        {
            try
            {
                //var result = _context.MediaLinkEntities.Include(x => x.VideoCategory).Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).ToList();
                var result = (from ml in _context.MediaLinkEntities
                              join cat in _context.VideoCategories
                              on ml.VideoCategoryId equals cat.Id
                              where cat.IsActive == true
                              select new
                              {
                                  CategoryId = cat.Id,
                                  Category = cat.Description,
                                  CreatedOn = ml.CreatedOn,
                                  UserId = ml.Id,
                                  IsActive = ml.IsActive,
                                  VideoId = ml.VideoId,
                                  Title = ml.Title,
                                  ImageLink = ml.ImageLink,
                                  Description = ml.Description,
                                  Header = ml.Header,
                                  IsFeatured = ml.IsFeatured,
                              }).ToList().OrderByDescending(ml => ml.CreatedOn);
                List<MediaLinkCategory> mediaLinkCategories = new List<MediaLinkCategory>();
                List<string> categories = new List<string>();
                if (result != null)
                {
                    foreach (var res in result)
                    {
                        if (!categories.Contains(res.Category))
                        {
                            categories.Add(res.Category);
                            MediaLinkCategory mediaLinkCategory = new MediaLinkCategory();
                            mediaLinkCategory.Category = res.Category;
                            var mediaLink = result.Where(x => x.CategoryId == res.CategoryId).ToList();
                            List<MediaLinkEntity> mediaEntities = new List<MediaLinkEntity>();
                            foreach (var media in mediaLink)
                            {
                                MediaLinkEntity mediaEntity = new MediaLinkEntity();
                                mediaEntity.CreatedOn = media.CreatedOn;
                                mediaEntity.IsActive = media.IsActive;
                                mediaEntity.VideoId = media.VideoId;
                                mediaEntity.Title = media.Title;
                                mediaEntity.ImageLink = media.ImageLink;
                                mediaEntity.Description = media.Description;
                                mediaEntity.Header = media.Header;
                                mediaEntity.IsFeatured = media.IsFeatured;
                                mediaEntities.Add(mediaEntity);
                                if (media.VideoId.Contains("instagram") && String.IsNullOrEmpty(media.ImageLink))
                                {
                                    mediaEntity.ImageLink = "http://admin.myavana.com/images/instagram.jpg";
                                }
                            }
                            mediaLinkCategory.MediaEntity = mediaEntities;
                            mediaLinkCategories.Add(mediaLinkCategory);

                        }
                    }
                    return (new JsonResult(mediaLinkCategories) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                }
                _logger.LogError("Method: GetTvLinksCategories, Error: No settings found.");
                return (new JsonResult(""), false, "No settings found.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetTvLinksCategories, Error: " + Ex.Message, Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again.");
            }
        }
        public MediaLinkEntityModel GetMediaById(MediaLinkEntity mediaLinkEntity)
        {
            try
            {
                MediaLinkEntityModel mediaLinkEntityModel = _context.MediaLinkEntities.Where(x => x.Id == mediaLinkEntity.Id).Select(x => new MediaLinkEntityModel
                {
                    Id = x.Id,
                    VideoId = x.VideoId,
                    Title = x.Title,
                    Description = x.Description,
                    Header = x.Header,
                    IsFeatured = x.IsFeatured,
                    ImageLink = x.ImageLink,
                    VideoCategoryId = x.VideoCategoryId,
                    IsActive = x.IsActive,
                    CreatedOn = x.CreatedOn,
                    ShowOnMobile = x.ShowOnMobile,
                    userSalons = _context.SalonVideos.Where(s => s.MediaLinkEntityId == x.MediaLinkEntityId && s.IsActive == true).Select(s => new UserSalonOwnerModel
                    {
                        SalonId = s.SalonId,
                        SalonName = _context.Salons.Where(p => p.SalonId == s.SalonId).Select(p => p.SalonName).FirstOrDefault()
                    }).ToList(),
                    SelectedHairChallenges = _context.HairChallengeVideoMapping.Where(a=>a.MediaLinkEntityId == x.MediaLinkEntityId).Select(a=>a.HairChallengeId).ToList(),
                    SelectedHairGoals = _context.HairGoalVideoMapping.Where(a => a.MediaLinkEntityId == x.MediaLinkEntityId).Select(a => a.HairGoalId).ToList(),
                    MediaLinkEntityId = x.MediaLinkEntityId
                }).FirstOrDefault();
                return mediaLinkEntityModel;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetMediaById, MediaLinkEntityId:" + mediaLinkEntity.Id + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public MediaLinkEntity SaveMediaLink(MediaLinkEntityModel mediaLinkEntity, int userId)
        {
            try
            {
                List<HairChallenges> listHairChallenges = JsonConvert.DeserializeObject<List<HairChallenges>>(mediaLinkEntity.HairChallenges);
                List<HairGoal> listHairGoals = JsonConvert.DeserializeObject<List<HairGoal>>(mediaLinkEntity.HairGoals);

                var isNewUser = false;
                var mediaLinkEntityObj = new MediaLinkEntity();

                if (mediaLinkEntity.ShowOnMobile == true)
                {
                    var checkShowOnMobile = _context.MediaLinkEntities.Where(x => x.ShowOnMobile == true).FirstOrDefault();
                    if (checkShowOnMobile != null)
                    {
                        checkShowOnMobile.ShowOnMobile = false;
                    }
                    _context.SaveChanges();
                }

                if (mediaLinkEntity.Id != Guid.Empty)
                {
                    var data = _context.MediaLinkEntities.Where(x => x.Id == mediaLinkEntity.Id).FirstOrDefault();
                    data.VideoId = mediaLinkEntity.VideoId;
                    data.Title = mediaLinkEntity.Title;
                    data.Description = mediaLinkEntity.Description;
                    data.Header = mediaLinkEntity.Header;
                    data.IsFeatured = mediaLinkEntity.IsFeatured;
                    data.ImageLink = mediaLinkEntity.ImageLink;
                    data.ShowOnMobile = mediaLinkEntity.ShowOnMobile;

                    //update HairChallengeVideoMapping table
                    foreach (var spec in listHairChallenges)
                    {
                        var hairchallengevideo = _context.HairChallengeVideoMapping
                                                         .FirstOrDefault(x => x.MediaLinkEntityId == data.MediaLinkEntityId
                                                                           && x.HairChallengeId == spec.HairChallengeId);

                        if (hairchallengevideo != null)
                        {
                            hairchallengevideo.HairChallengeId = spec.HairChallengeId; 
                            _context.HairChallengeVideoMapping.Update(hairchallengevideo);
                        }
                        else
                        {
                            var newHairChallengeVideo = new HairChallengeVideoMapping
                            {
                                MediaLinkEntityId = data.MediaLinkEntityId,
                                HairChallengeId = spec.HairChallengeId,
                                IsActive = true,
                                CreatedOn = DateTime.Now
                            };
                            _context.HairChallengeVideoMapping.Add(newHairChallengeVideo);
                        }
                    }
                    //delete record
                    var selectedHairChallengeIds = listHairChallenges.Select(x => x.HairChallengeId).ToList();
                    var hairChallengesToDelete = _context.HairChallengeVideoMapping.Where(x => !selectedHairChallengeIds.Contains(x.HairChallengeId) && x.MediaLinkEntityId == data.MediaLinkEntityId).ToList();
                    if (hairChallengesToDelete.Any())
                    {
                        _context.HairChallengeVideoMapping.RemoveRange(hairChallengesToDelete);
                    }

                    //update HairGoalVideoMapping table
                    foreach (var spec in listHairGoals)
                    {
                        var hairgoalsvideo = _context.HairGoalVideoMapping
                                                     .FirstOrDefault(x => x.MediaLinkEntityId == data.MediaLinkEntityId
                                                                       && x.HairGoalId == spec.HairGoalId);

                        if (hairgoalsvideo != null)
                        {
                            hairgoalsvideo.HairGoalId = spec.HairGoalId;  
                            _context.HairGoalVideoMapping.Update(hairgoalsvideo);
                        }
                        else
                        {
                            var newHairGoalVideo = new HairGoalVideoMapping
                            {
                                MediaLinkEntityId = data.MediaLinkEntityId,
                                HairGoalId = spec.HairGoalId,
                                IsActive = true,
                                CreatedOn = DateTime.Now
                            };
                            _context.HairGoalVideoMapping.Add(newHairGoalVideo);
                        }
                    }
                    //delete record
                    var selectedHairGoalsIds = listHairGoals.Select(x => x.HairGoalId).ToList();
                    var hairGoalsToDelete = _context.HairGoalVideoMapping.Where(x => !selectedHairGoalsIds.Contains(x.HairGoalId) && x.MediaLinkEntityId== data.MediaLinkEntityId).ToList();
                    if (hairGoalsToDelete.Any())
                    {
                        _context.HairGoalVideoMapping.RemoveRange(hairGoalsToDelete);
                    }

                    var salonVideos = _context.SalonVideos.Where(s => s.IsActive == true && s.MediaLinkEntityId == data.MediaLinkEntityId).ToList();
                    salonVideos.ForEach(x => x.IsActive = false);
                    _context.SalonVideos.UpdateRange(salonVideos);
                    if (mediaLinkEntity.userSalons != null && mediaLinkEntity.userSalons.Count() > 0)
                    {
                        var salonIds = mediaLinkEntity.userSalons.Select(x => x.SalonId);
                        foreach (var userSalon in salonIds)
                        {
                            SalonVideo obj = new SalonVideo();
                            obj.SalonId = userSalon;
                            obj.IsActive = true;
                            obj.MediaLinkEntityId = data.MediaLinkEntityId;
                            _context.SalonVideos.Add(obj);
                        }
                    }
                }
                else
                {
                    isNewUser = true;
                    mediaLinkEntityObj.Id = Guid.NewGuid();
                    mediaLinkEntityObj.VideoId = mediaLinkEntity.VideoId;
                    mediaLinkEntityObj.Title = mediaLinkEntity.Title;
                    mediaLinkEntityObj.Description = mediaLinkEntity.Description;
                    mediaLinkEntityObj.Header = mediaLinkEntity.Header;
                    mediaLinkEntityObj.IsFeatured = mediaLinkEntity.IsFeatured;
                    mediaLinkEntityObj.ImageLink = mediaLinkEntity.ImageLink;
                    mediaLinkEntityObj.VideoCategoryId = mediaLinkEntity.VideoCategoryId;
                    mediaLinkEntityObj.IsActive = true;
                    mediaLinkEntityObj.CreatedOn = DateTime.UtcNow;
                    mediaLinkEntityObj.ShowOnMobile = mediaLinkEntity.ShowOnMobile;
                    _context.MediaLinkEntities.Add(mediaLinkEntityObj);
                    _context.SaveChanges();

                    //insert hairChallengeVideoMapping table
                    foreach (var spec in listHairChallenges)
                    {
                        HairChallengeVideoMapping hairChallengeVideoMapping = new HairChallengeVideoMapping();
                        hairChallengeVideoMapping.MediaLinkEntityId = mediaLinkEntityObj.MediaLinkEntityId;
                        hairChallengeVideoMapping.HairChallengeId = spec.HairChallengeId;
                        hairChallengeVideoMapping.IsActive = true;
                        hairChallengeVideoMapping.CreatedOn = DateTime.Now;
                        _context.HairChallengeVideoMapping.Add(hairChallengeVideoMapping);
                    }

                    //insert hairGoalVideoMapping table
                    foreach (var spec in listHairGoals)
                    {

                        HairGoalVideoMapping hairGoalVideoMapping = new HairGoalVideoMapping();
                        hairGoalVideoMapping.MediaLinkEntityId = mediaLinkEntityObj.MediaLinkEntityId;
                        hairGoalVideoMapping.HairGoalId = spec.HairGoalId;
                        hairGoalVideoMapping.IsActive = true;
                        hairGoalVideoMapping.CreatedOn = DateTime.Now;
                        _context.HairGoalVideoMapping.Add(hairGoalVideoMapping);
                    }
                }
                _context.SaveChanges();

                if (isNewUser)
                {
                    var listSalonVideo = new List<SalonVideo>();
                    var userType = _context.WebLogins.FirstOrDefault(x => x.UserId == userId).UserTypeId;
                    if (userType == (int)UserTypeEnum.B2B)
                    {
                        var salonIds = _context.SalonsOwners.Where(x => x.UserId == userId && x.IsActive == true).Select(x => x.SalonId).ToArray();
                        foreach (var item in salonIds)
                        {
                            listSalonVideo.Add(new SalonVideo { MediaLinkEntityId = mediaLinkEntityObj.MediaLinkEntityId, SalonId = item, IsActive = true });
                        }
                        _context.SalonVideos.AddRange(listSalonVideo);
                        _context.SaveChanges();
                    }
                    else
                    {
                        if (mediaLinkEntity?.userSalons?.Count > 0)
                        {
                            foreach (var item in mediaLinkEntity.userSalons)
                            {
                                listSalonVideo.Add(new SalonVideo { MediaLinkEntityId = mediaLinkEntityObj.MediaLinkEntityId, SalonId = item.SalonId, IsActive = true });
                            }
                            _context.SalonVideos.AddRange(listSalonVideo);
                            _context.SaveChanges();
                        }
                    }
                }
                return mediaLinkEntityObj;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveMediaLink, MediaLinkEntityId:" + mediaLinkEntity.Id + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool DeleteMediaLink(MediaLinkEntity mediaLink)
        {
            try
            {
                var objCode = _context.MediaLinkEntities.FirstOrDefault(x => x.Id == mediaLink.Id);
                {
                    if (objCode != null)
                    {
                        objCode.IsActive = false;
                    }
                    mediaLink.MediaLinkEntityId = _context.MediaLinkEntities.Where(x => x.Id == mediaLink.Id).Select(a => a.MediaLinkEntityId).SingleOrDefault();

                    //Isactive false for challenge
                    var challenges = _context.HairChallengeVideoMapping.Where(x => x.MediaLinkEntityId == mediaLink.MediaLinkEntityId).ToList();
                    foreach (var data in challenges)
                    {
                        data.IsActive = false;
                    }

                    //Isactive false for goals
                    var goals = _context.HairGoalVideoMapping.Where(x => x.MediaLinkEntityId == mediaLink.MediaLinkEntityId).ToList();
                    foreach (var data in goals)
                    {
                        data.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteMediaLink, MediaLinkEntityId:" + mediaLink.Id + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public EducationTip AddEducationTip(EducationTip educationTip)
        {
            try
            {
                if (educationTip.ShowOnMobile == true)
                {
                    var checkShowOnMobile = _context.EducationTips.Where(x => x.ShowOnMobile == true).FirstOrDefault();
                    if (checkShowOnMobile != null)
                    {
                        checkShowOnMobile.ShowOnMobile = false;
                        _context.SaveChanges();
                    }
                }
                if (educationTip.EducationTipsId != 0)
                {
                    var objeducationTip = _context.EducationTips.Where(x => x.EducationTipsId == educationTip.EducationTipsId).FirstOrDefault();
                    objeducationTip.Description = educationTip.Description;
                    objeducationTip.IsActive = true;
                    objeducationTip.ShowOnMobile = educationTip.ShowOnMobile;
                }
                else
                {
                    _context.EducationTips.Add(new EducationTip()
                    {
                        Description = educationTip.Description,
                        ShowOnMobile = educationTip.ShowOnMobile,
                        IsActive = true
                    });
                }
                _context.SaveChanges();
                return educationTip;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: AddEducationTip, EducationTipsId:" + educationTip.EducationTipsId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }
        public EducationTip GetEducationTipById(EducationTip educationTip)
        {
            try
            {
                EducationTip educationTipModel = _context.EducationTips.Where(x => x.EducationTipsId == educationTip.EducationTipsId).FirstOrDefault();
                return educationTipModel;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetEducationTipById, EducationTipsId:" + educationTip.EducationTipsId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }
        public EducationTipAndVideo GetEducationTipForMobile()
        {
            try
            {
                EducationTipAndVideo educationTipModel = _context.EducationTips.Where(x => x.ShowOnMobile == true).Select(x => new EducationTipAndVideo
                {
                    EducationTipsId = x.EducationTipsId,
                    Description = x.Description,
                    ShowOnMobile = x.ShowOnMobile
                }).FirstOrDefault();

                EducationTipAndVideo educationVideoModel = _context.MediaLinkEntities.Where(x => x.ShowOnMobile == true).Select(x => new EducationTipAndVideo
                {
                    MediaLinkEntityId = x.MediaLinkEntityId,
                    ImageLink = x.ImageLink,
                    Title = x.Title,
                    VideoDescription = x.Description,
                    VideoId = x.VideoId
                }).FirstOrDefault();

                EducationTipAndVideo educationTipAndVideoModel = new EducationTipAndVideo();
                educationTipAndVideoModel.EducationTipsId = educationTipModel.EducationTipsId;
                educationTipAndVideoModel.Description = educationTipModel.Description;
                educationTipAndVideoModel.ShowOnMobile = educationTipModel.ShowOnMobile;
                educationTipAndVideoModel.MediaLinkEntityId = educationVideoModel.MediaLinkEntityId;
                educationTipAndVideoModel.VideoDescription = educationVideoModel.VideoDescription;
                educationTipAndVideoModel.ImageLink = educationVideoModel.ImageLink;
                educationTipAndVideoModel.VideoId = educationVideoModel.VideoId;
                educationTipAndVideoModel.Title = educationVideoModel.Title;

                return educationTipAndVideoModel;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetEducationTipForMobile, Error: " + Ex.Message, Ex);
                return null;
            }
        }
        public List<EducationTipModel> GetEducationTips(int start, int length)
        {
            try
            {
                List<EducationTipModel> educationTipList = _context.EducationTips.Where(x => x.IsActive == true).Skip(start).Take(length).Select(x => new EducationTipModel
                {
                    EducationTipsId = x.EducationTipsId,
                    Description = x.Description,
                    ShowOnMobile = x.ShowOnMobile,
                    TotalRecords = _context.EducationTips.Where(p => p.IsActive == true).Count()
                }).ToList();

                return educationTipList;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetEducationTips, Error: " + Ex.Message, Ex);
                return null;
            }
        }
    }
}
