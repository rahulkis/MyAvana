using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyAvanaQuestionaire.Factory;
using MyAvanaQuestionaire.Models;
using MyAvanaQuestionaire.Utility;
using MyAvanaQuestionaireModel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using DataTables.AspNetCore.Mvc.Binder;

namespace MyAvanaQuestionaire.Controllers
{
    [Authorize(AuthenticationSchemes = "CustomerCookies")]
    public class HairProfileController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        private readonly IOptions<AppSettingsModel> config;
        private Uri BaseEndpoint;
        public HairProfileController(IOptions<AppSettingsModel> app, IOptions<AppSettingsModel> config)
        {
            this.config = config;
            BaseEndpoint = new Uri(config.Value.WebApiBaseUrl);
            ApplicationSettings.WebApiUrl = config.Value.WebApiBaseUrl;
        }
        public async Task<IActionResult> CustomerHair(int HairProfileId)
        {
            var sharedView = HttpContext.Request.Query["View"].ToString();
            ViewBag.View = sharedView;

            var claimsIdentity1 = (ClaimsIdentity)User.Identity;
            string userId = (claimsIdentity1.Claims).Select(x => x.Value).FirstOrDefault();

            HairProfileCustomerModel hairProfileModel = new HairProfileCustomerModel();
            hairProfileModel.UserId = userId;
            if(HairProfileId!=0)
            {
                hairProfileModel.HairProfileId = HairProfileId;
            }
            QuestionaireModel questionaire = new QuestionaireModel();
            questionaire.Userid = userId;

            var result = await MyavanaCustomerApiClientFactory.Instance.GetQuestionaireDetails(questionaire);
            if (result.Data.IsExist == true)
            {
                ViewBag.IsExist = true;
            }
            else
            {
                ViewBag.IsExist = false;
                //return RedirectToAction("start", "Questionaire", new { id = userId });
            }
            if (result.Data.PaymentId != null)
            {
                HttpContext.Session.SetString("ispaid", "true");
            }
            if (hairProfileModel != null)
            {
                hairProfileModel.IsRequestedFromCustomer = true;
                var response = await MyavanaCustomerApiClientFactory.Instance.GetHairProfileCustomerExceptTab2(hairProfileModel);
                if (response.Data != null && !String.IsNullOrEmpty(response.Data.AIResultNew))
                {
                    var result11 = JsonConvert.DeserializeObject<dynamic>(response.Data.AIResultNew.ToString());
                    var r1 = (Newtonsoft.Json.Linq.JObject)JObject.Parse(JsonConvert.DeserializeObject<dynamic>(response.Data.AIResultNew.ToString()));
                    if (response.Data.IsVersion2 == true)
                    {
                        var rn = (object)r1["type"]["score"];
                        response.Data.HairTextureLabelAIResult = (string)r1["hairTextureLabel"];
                        response.Data.HairTypeLabelAIResult = (string)r1["hairTypeLabel"];
                        response.Data.LabelAIResult = (string)r1["label"];
                        response.Data.AIResultNewDecoded = (JObject)rn;

                        var texture = (object)r1["texture"]["score"];
                        response.Data.AIResultTextureDecoded = (JObject)texture;

                        response.Data.UserAIImage = (string)r1["imageLink"];
                    }
                    else
                    {
                        var rn = (object)r1["score"];
                        response.Data.HairTextureLabelAIResult = (string)r1["hairTextureLabel"];
                        response.Data.HairTypeLabelAIResult = (string)r1["hairTypeLabel"];
                        response.Data.LabelAIResult = (string)r1["label"];
                        response.Data.AIResultNewDecoded = (JObject)rn;
                    }
                    
                }
                else if (response.Data != null && !String.IsNullOrEmpty(response.Data.AIResult))
                {
                    var result1 = JsonConvert.DeserializeObject<dynamic>(response.Data.AIResult.ToString());
                    if(response.Data.IsAIV2Mobile != true)
                    {
                        var pn = (object)result1["item2"];
                        if (pn == null)
                        {
                            pn = (object)result1["Item2"];
                        }
                        response.Data.AIResultDecoded = (JObject)pn;
                    }
                    else
                    {
                        var rn = (object)result1["type"]["score"];
                        response.Data.AIResultDecoded = (JObject)rn;
                        var texture = (object)result1["texture"]["score"];
                        response.Data.AIResultTextureDecoded = (JObject)texture;
                    }
                }

                return View(response.Data);
            }
            return Content("0");
        }

        public async Task<ActionResult> HairAI()
        {
            var claimsIdentity1 = (ClaimsIdentity)User.Identity;
            string userId = (claimsIdentity1.Claims).Select(x => x.Value).FirstOrDefault();

            
            QuestionaireModel questionaire = new QuestionaireModel();
            questionaire.Userid = userId;
            var result = await MyavanaCustomerApiClientFactory.Instance.GetQuestionaireDetails(questionaire);
            if (result.Data.IsExist == true)
            {
                ViewBag.IsExist = true;
            }
            else
            {
                ViewBag.IsExist = false;
                
            }
           
            var passedValue = TempData["ExpiredMessage"];
            ViewBag.ExpiredMessage = result.Data.ExpiredMessage;//passedValue;
            return View("HairAI"); 
        }
        public PartialViewResult GetHHCP(string id)
        {
            var claimsIdentity1 = (ClaimsIdentity)User.Identity;
            string userId = (claimsIdentity1.Claims).Select(x => x.Value).FirstOrDefault();

            HairProfileCustomerModel hairProfileModel = new HairProfileCustomerModel();
            hairProfileModel.UserId = userId;
            hairProfileModel.HairProfileId = Convert.ToInt32(id);

            QuestionaireModel questionaire = new QuestionaireModel();
            questionaire.Userid = userId;

            var result =  MyavanaCustomerApiClientFactory.Instance.GetQuestionaireDetails(questionaire).GetAwaiter().GetResult();
            if (result.Data.IsExist == true)
            {
                ViewBag.IsExist = true;
            }
            else
            {
                ViewBag.IsExist = false;
                //return RedirectToAction("start", "Questionaire", new { id = userId });
            }

            if (hairProfileModel != null)
            {
                var response =  MyavanaCustomerApiClientFactory.Instance.GetHairProfileCustomerExceptTab2(hairProfileModel).GetAwaiter().GetResult();
                if (response.Data != null && !String.IsNullOrEmpty(response.Data.AIResultNew))
                {
                    var result11 = JsonConvert.DeserializeObject<dynamic>(response.Data.AIResultNew.ToString());
                    var r1 = (Newtonsoft.Json.Linq.JObject)JObject.Parse(JsonConvert.DeserializeObject<dynamic>(response.Data.AIResultNew.ToString()));
                    if (response.Data.IsVersion2 == true)
                    {
                        var rn = (object)r1["type"]["score"];
                        response.Data.HairTextureLabelAIResult = (string)r1["hairTextureLabel"];
                        response.Data.HairTypeLabelAIResult = (string)r1["hairTypeLabel"];
                        response.Data.LabelAIResult = (string)r1["label"];
                        response.Data.AIResultNewDecoded = (JObject)rn;

                        var texture = (object)r1["texture"]["score"];
                        response.Data.AIResultTextureDecoded = (JObject)texture;
                        response.Data.UserAIImage = (string)r1["imageLink"];
                    }
                    else
                    {
                        var rn = (object)r1["score"];
                        response.Data.HairTextureLabelAIResult = (string)r1["hairTextureLabel"];
                        response.Data.HairTypeLabelAIResult = (string)r1["hairTypeLabel"];
                        response.Data.LabelAIResult = (string)r1["label"];
                        response.Data.AIResultNewDecoded = (JObject)rn;
                    }

                }
                else if (response.Data != null && !String.IsNullOrEmpty(response.Data.AIResult))
                { 
                    var result1 = JsonConvert.DeserializeObject<dynamic>(response.Data.AIResult.ToString());
                    if (response.Data.IsAIV2Mobile != true)
                    {
                        var pn = (object)result1["item2"];
                        if (pn == null)
                        {
                            pn = (object)result1["Item2"];
                        }
                        response.Data.AIResultDecoded = (JObject)pn;
                    }
                    else
                    {
                        var rn = (object)result1["type"]["score"];
                        response.Data.AIResultDecoded = (JObject)rn;
                        var texture = (object)result1["texture"]["score"];
                        response.Data.AIResultTextureDecoded = (JObject)texture;
                    }
                }
                return PartialView("_customerHair", response.Data);
                //return View(response.Data);
            }
            return null;
            
        }

        #region Partial Views

        public PartialViewResult GetHHCPStrands(string id)
        {
            var claimsIdentity1 = (ClaimsIdentity)User.Identity;
            string userId = (claimsIdentity1.Claims).Select(x => x.Value).FirstOrDefault();
            HairProfileCustomerModel hairProfileModel = new HairProfileCustomerModel();
            hairProfileModel.UserId = userId;
            hairProfileModel.HairProfileId = Convert.ToInt32(id);

            if (hairProfileModel != null)
            {
                var response = MyavanaCustomerApiClientFactory.Instance.GetHairProfileCustomerTab2(hairProfileModel).GetAwaiter().GetResult();

                return PartialView("_hairAnalysis", response.Data);
            }
            return null;
        }
        public IActionResult ManageSharedHHCP()
        {
            Guid userId = new Guid(HttpContext.Session.GetString("id"));
            SharedHHCPModel sharedHHCPModel = new SharedHHCPModel();
            sharedHHCPModel.UserEmail = userId.ToString();
            return View(sharedHHCPModel);
        }
        public IActionResult ViewSharedHHCP()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSharedHHCPList([DataTablesRequest] DataTablesRequest dataRequest)
        {
           Guid userId = new Guid(HttpContext.Session.GetString("id"));
            var res = await MyavanaCustomerApiClientFactory.Instance.GetSharedHHCPList(userId);
            IEnumerable<SharedHHCPModel> filterProducts = res.ToList();


            try
            {
                IEnumerable<SharedHHCPModel> codes = filterProducts.Select(e => new SharedHHCPModel
                {
                    HHCPName = e.HHCPName,
                    HairProfileId = e.HairProfileId,
                    //SharedBy = e.SharedBy,
                    SharedWith = e.SharedWith,
                    //SharedByUser = e.SharedByUser,
                    SharedWithUser = e.SharedWithUser,
                    IsRevoked = e.IsRevoked,
                    SharedOn = e.SharedOn,
                    RevokedOn = e.RevokedOn,
                    Id=e.Id
                }).OrderByDescending(x => x.SharedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, res.Count));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public async Task<IActionResult> ShareHHCP(string email,int hairProfileId)
        {
            try
            {
                var data = HttpContext.User.Identity.Name;
                var sharedBy = new Guid(data);
                var ExistUser = await MyavanaCustomerApiClientFactory.Instance.ShareEmailExist(email, hairProfileId, sharedBy);
                if (ExistUser.message == "This entered user does not exist")
                    return Content("2");

                if (ExistUser.message == "Success")
                    return Content("1");
                else
                    return Content("0");
            }
            catch(Exception e)
            {

            }
            return null;
        }
        #endregion
        [HttpPost]
        public async Task<IActionResult> RevokeAccessHHCP(SharedHHCPModel sharedHHCP)
        {
            var response = await MyavanaCustomerApiClientFactory.Instance.RevokeAccessHHCP(sharedHHCP);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        [HttpGet]
        public async Task<IActionResult> GetSharedWithMeHHCPList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            Guid userId = new Guid(HttpContext.Session.GetString("id"));
            var res = await MyavanaCustomerApiClientFactory.Instance.GetSharedWithMeHHCPList(userId);
            IEnumerable<SharedHHCPModel> filterProducts = res.ToList();


            try
            {
                IEnumerable<SharedHHCPModel> codes = filterProducts.Select(e => new SharedHHCPModel
                {
                    HHCPName = e.HHCPName,
                    HairProfileId = e.HairProfileId,
                    SharedBy = e.SharedBy,
                    SharedWith = e.SharedWith,
                    SharedByUser = e.SharedByUser,
                    //SharedWithUser = e.SharedWithUser,
                    IsRevoked = e.IsRevoked,
                    SharedOn = e.SharedOn,
                    Id = e.Id
                }).OrderByDescending(x => x.SharedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, res.Count));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}