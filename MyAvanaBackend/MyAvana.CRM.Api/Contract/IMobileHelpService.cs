using Microsoft.AspNetCore.Mvc;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IMobileHelpService
    {
        MobileHelpFAQ SaveMobileHelpFAQ(MobileHelpFAQ mobileHelpFAQEntity);
        List<MobileHelpFaqModel> GetMobileHelpFaqList();
        MobileHelpFaqModel GetMobileHelpFaqById(MobileHelpFaqModel mobileHelpFAQ);
        bool DeleteMobileHelpFaq(MobileHelpFAQ mobileHelpFAQ);
        (JsonResult result, bool success, string error) GetMobileHelpTopicList();
    }
}
