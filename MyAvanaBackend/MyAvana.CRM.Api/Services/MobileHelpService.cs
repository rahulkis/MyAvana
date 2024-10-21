using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Logger.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class MobileHelpService : IMobileHelpService
    {
        private readonly ILogger _logger;
        private readonly AvanaContext _context;

        public MobileHelpService(ILogger logger, AvanaContext avanaContext)
        {
            _logger = logger;
            _context = avanaContext;
        }

        public MobileHelpFAQ SaveMobileHelpFAQ(MobileHelpFAQ mobileHelpFAQEntity)
        {
            try
            {
                MobileHelpFAQ mobileHelpFAQ = _context.MobileHelpFAQ.Where(x => x.MobileHelpFAQId == mobileHelpFAQEntity.MobileHelpFAQId).FirstOrDefault();
                if(mobileHelpFAQ != null)
                {
                    mobileHelpFAQ.Title = mobileHelpFAQEntity.Title;
                    mobileHelpFAQ.Description = mobileHelpFAQEntity.Description;
                    mobileHelpFAQ.Videolink = mobileHelpFAQEntity.Videolink;
                    mobileHelpFAQ.ImageLink = mobileHelpFAQEntity.ImageLink;
                    mobileHelpFAQ.VideoThumbnail = mobileHelpFAQEntity.VideoThumbnail;
                    mobileHelpFAQ.MobileHelpTopicId = mobileHelpFAQEntity.MobileHelpTopicId;
                    mobileHelpFAQ.LastModifiedOn = DateTime.Now;
                }
                else
                {
                    MobileHelpFAQ mobileHelp = new MobileHelpFAQ();
                    mobileHelp.Title = mobileHelpFAQEntity.Title;
                    mobileHelp.Description = mobileHelpFAQEntity.Description;
                    mobileHelp.Videolink = mobileHelpFAQEntity.Videolink;
                    mobileHelp.ImageLink = mobileHelpFAQEntity.ImageLink;
                    mobileHelp.VideoThumbnail = mobileHelpFAQEntity.VideoThumbnail;
                    mobileHelp.MobileHelpTopicId = mobileHelpFAQEntity.MobileHelpTopicId;
                    mobileHelp.CreatedBy = mobileHelpFAQEntity.CreatedBy;
                    mobileHelp.IsActive = true;
                    mobileHelp.CreatedOn = DateTime.Now;
                    _context.Add(mobileHelp);
                }
                _context.SaveChanges();
                return mobileHelpFAQEntity;
            }
            catch(Exception ex)
            {
                _logger.LogError("Method: SaveMobileHelpFAQ, MobileHelpFAQId:" + mobileHelpFAQEntity.MobileHelpFAQId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public List<MobileHelpFaqModel> GetMobileHelpFaqList()
        {
            try
            {
                List<MobileHelpFaqModel> FAQs = _context.MobileHelpFAQ.Where(x => x.IsActive == true).OrderByDescending(x => x.CreatedOn).
                    Select(x => new MobileHelpFaqModel
                    {
                        MobileHelpFAQId = x.MobileHelpFAQId,
                        Title = x.Title,
                        Description = x.Description,
                        ImageLink = x.ImageLink,
                        Videolink = x.Videolink,
                        VideoThumbnail = x.VideoThumbnail,                     
                        IsActive = x.IsActive,
                        CreatedOn = x.CreatedOn,
                        MobileHelpTopicId=x.MobileHelpTopicId,
                        MobileHelpTopicDescription =x.MobileHelpTopic.Description
                       
                    }).ToList();
               

                return FAQs;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetMobileHelpFaqList, Error: " + ex.Message, ex);
                return null;
            }
        }
        public MobileHelpFaqModel GetMobileHelpFaqById(MobileHelpFaqModel mobileHelpFAQ)
        {
            try
            {
                MobileHelpFaqModel MobileHelpFaqEntity = _context.MobileHelpFAQ.Where(x => x.MobileHelpFAQId == mobileHelpFAQ.MobileHelpFAQId).
                    Select(x => new MobileHelpFaqModel
                    {
                        MobileHelpFAQId = x.MobileHelpFAQId,
                        Title = x.Title,
                        Description = x.Description,
                        ImageLink = x.ImageLink,
                        Videolink = x.Videolink,
                        VideoThumbnail = x.VideoThumbnail,
                        IsActive = x.IsActive,
                        CreatedOn = x.CreatedOn,
                        MobileHelpTopicId = x.MobileHelpTopicId,
                        MobileHelpTopicDescription = x.MobileHelpTopic.Description
                    }).FirstOrDefault();
                _context.SaveChanges();
                return MobileHelpFaqEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetMobileHelpFaqById, MobileHelpFAQId:" + mobileHelpFAQ.MobileHelpFAQId + ", Error: " + ex.Message, ex);
                return null;
            }
        }

        public bool DeleteMobileHelpFaq(MobileHelpFAQ mobileHelpFAQ)
        {
            try
            {
                var FAQ = _context.MobileHelpFAQ.FirstOrDefault(x => x.MobileHelpFAQId == mobileHelpFAQ.MobileHelpFAQId);
                {
                    if (FAQ != null)
                    {
                        FAQ.IsActive = false;
                    }
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteMobileHelpFaq, MobileHelpFAQId:" + mobileHelpFAQ.MobileHelpFAQId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }
        public (JsonResult result, bool success, string error) GetMobileHelpTopicList()
        {
            try
            {
                var result = _context.MobileHelpTopic.Where(x => x.IsActive == true).ToList();
                if (result != null)
                    return (new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                _logger.LogError("Method: GetMobileHelpTopicList, Error: No Topics found.");
                return (new JsonResult(""), false, "No settings found.");
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetMobileHelpTopicList, Error: " + Ex.Message, Ex);
                return (new JsonResult(""), false, "Something went wrong. Please try again.");
            }
        }
    }
}
