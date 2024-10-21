using Microsoft.AspNetCore.Mvc;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface ISocialMediaService
    {
        (JsonResult result, bool success, string error) GetTvLinks(string settingName, string subSettingName, int userId); 
         (JsonResult result, bool success, string error) GetTvLinks2(string settingName, string subSettingName); 
        (JsonResult result, bool success, string error) GetTvLinksCategories(string settingName, string subSettingName);
        MediaLinkEntityModel GetMediaById(MediaLinkEntity mediaLinkEntity);
        MediaLinkEntity SaveMediaLink(MediaLinkEntityModel mediaLinkEntity, int userId);
        bool DeleteMediaLink(MediaLinkEntity mediaLink);
        (JsonResult result, bool success, string error) GetVideoCategories();
        EducationTip AddEducationTip(EducationTip educationTip);
        EducationTip GetEducationTipById(EducationTip educationTip);
        List<EducationTipModel> GetEducationTips(int start, int length);
        EducationTipAndVideo GetEducationTipForMobile();
    }
}
