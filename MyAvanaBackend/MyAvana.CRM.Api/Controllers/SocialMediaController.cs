﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAvana.CRM.Api.Contract;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using Newtonsoft.Json.Linq;

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialMediaController : ControllerBase
    {
        private readonly ISocialMediaService _mediaService;
        private readonly IBaseBusiness _baseBusiness;
        private readonly IHostingEnvironment _environment;
        public SocialMediaController(ISocialMediaService mediaService, IBaseBusiness baseBusiness)
        {
            _mediaService = mediaService;
            _baseBusiness = baseBusiness;
        }
        [HttpGet("GetTvLinks")]
        public IActionResult GetTvLinks(string settingName, string subSettingName, int userId)
        {
            var result = _mediaService.GetTvLinks(settingName, subSettingName,userId);
            if (result.success) return result.result;
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetVideoCategories")]
        public IActionResult GetVideoCategories()
        {
            var result = _mediaService.GetVideoCategories();
            if (result.success) return result.result;
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetTvLinks2")]
        public IActionResult GetTvLinks2(string settingName, string subSettingName)
        {
            var result = _mediaService.GetTvLinks2(settingName, subSettingName);
            if (result.success) return result.result;
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpGet("GetTvLinksCategories")]
        public IActionResult GetTvLinksCategories(string settingName, string subSettingName)
        {
            var result = _mediaService.GetTvLinksCategories(settingName, subSettingName);
            if (result.success) return result.result;
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }

        [HttpPost("SaveMediaLinks")]
        public JObject SaveMediaLinks(MediaLinkEntityModel mediaLinkEntity, [FromQuery] int userId)
        {
            MediaLinkEntity result = _mediaService.SaveMediaLink(mediaLinkEntity,userId);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("GetMediaById")]
        public JObject GetMediaById(MediaLinkEntity mediaLinkEntity)
        {
            MediaLinkEntityModel result = _mediaService.GetMediaById(mediaLinkEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost]
        [Route("DeleteMediaLink")]
        public JObject DeleteMediaLink(MediaLinkEntity mediaLink)
        {
            bool result = _mediaService.DeleteMediaLink(mediaLink);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", mediaLink);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpPost("AddEducationTip")]
        public JObject AddEducationTip(EducationTip educationTip)
        {
            EducationTip result = _mediaService.AddEducationTip(educationTip);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpPost("GetEducationTipById")]
        public JObject GetEducationTipById(EducationTip educationTip)
        {
            EducationTip result = _mediaService.GetEducationTipById(educationTip);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }

        [HttpGet("GetEducationTips")]
        public JObject GetEducationTips(int start, int length)
        {
            List<EducationTipModel> result = _mediaService.GetEducationTips(start, length);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);

        }

        [HttpGet("GetEducationTipForMobile")]
        public JObject GetEducationTipForMobile()
        {
            EducationTipAndVideo result = _mediaService.GetEducationTipForMobile();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);

        }
    }
}