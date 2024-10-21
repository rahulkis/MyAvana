using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
    public class MobileHelpController : ControllerBase
    {
        private readonly IMobileHelpService _mobileHelpService;
        private readonly IBaseBusiness _baseBusiness;

        public MobileHelpController(IMobileHelpService mobileHelpService, IBaseBusiness baseBusiness)
        {
            _mobileHelpService = mobileHelpService;
            _baseBusiness = baseBusiness;
        }

        [HttpPost("SaveMobileHelpFAQ")]
        public JObject SaveMobileHelpFAQ(MobileHelpFAQ mobileHelpFAQEntity)
        {
            MobileHelpFAQ result = _mobileHelpService.SaveMobileHelpFAQ(mobileHelpFAQEntity);
            if(result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpGet("GetMobileHelpFaqList")]
        public JObject GetMobileHelpFaqList()
        {
            List<MobileHelpFaqModel> result = _mobileHelpService.GetMobileHelpFaqList();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }
        [HttpPost("GetMobileHelpFaqById")]
        public JObject GetMobileHelpFaqById(MobileHelpFaqModel mobileHelpFAQEntity)
        {
            var result = _mobileHelpService.GetMobileHelpFaqById(mobileHelpFAQEntity);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);

        }
        [HttpPost]
        [Route("DeleteMobileHelpFaq")]
        public JObject DeleteMobileHelpFaq(MobileHelpFAQ mobileHelpFAQEntity)
        {
            bool result = _mobileHelpService.DeleteMobileHelpFaq(mobileHelpFAQEntity);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", mobileHelpFAQEntity);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }
        [HttpGet("GetMobileHelpTopicList")]
        public IActionResult GetMobileHelpTopicList()
        {
            var result = _mobileHelpService.GetMobileHelpTopicList();
            if (result.success) return result.result;
            return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
        }
    }
}
