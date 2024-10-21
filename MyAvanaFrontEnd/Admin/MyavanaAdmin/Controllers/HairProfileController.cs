using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyavanaAdmin.Factory;
using MyavanaAdminModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyavanaAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookies")]
    public class HairProfileController : Controller
    {
        public IActionResult HairProfiles()
        {
            return View();
        }

        public async Task<IActionResult> CustomerHairProfile(string id, string hairProfileId)
        {
            HairProfileCustomerModel hairProfileModel = new HairProfileCustomerModel();
            hairProfileModel.UserId = id;
            if (!string.IsNullOrEmpty(hairProfileId))
            {
                hairProfileModel.HairProfileId = Convert.ToInt32(hairProfileId);
            }
            if (hairProfileModel != null)
            {
                hairProfileModel.LoginUserId = Request.Cookies["UserId"];
                var response = await MyavanaAdminApiClientFactory.Instance.GetHairProfileCustomerExceptTab2(hairProfileModel);
                //if (!String.IsNullOrEmpty(response.Data.AIResult))
                //{
                //	var result = JsonConvert.DeserializeObject<dynamic>(response.Data.AIResult.ToString());
                //	var pn = (object)result["item2"];
                //	response.Data.AIResultDecoded = (JObject)pn;
                //}
                if (response.Data != null && !String.IsNullOrEmpty(response.Data.AIResultNew))
                {
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
                    //if (result1 is String)
                    //{
                    //    response.Data.AIResultDecoded = null;
                    //}
                    //else
                    //{
                    //    var pn = (object)result1["item2"];
                    //    if (pn == null)
                    //    {
                    //        pn = (object)result1["Item2"];
                    //    }
                    //    response.Data.AIResultDecoded = (JObject)pn;
                    //}

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
                return View(response.Data);
            }
            return Content("0");
        }

        [AllowAnonymous]
        public ActionResult MediaVideo(string video)
        {
            ViewBag.video = video;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateHairProfileSalon(SalonHairProfileModel salonHairProfileModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.UpdateHairProfileSalon(salonHairProfileModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        public PartialViewResult GetHHCP(string id, string userId)
        {
            HairProfileCustomerModel hairProfileModel = new HairProfileCustomerModel();
            hairProfileModel.UserId = userId;
            hairProfileModel.HairProfileId = Convert.ToInt32(id);

            if (hairProfileModel != null)
            {
                hairProfileModel.LoginUserId = Request.Cookies["UserId"];
                var response = MyavanaAdminApiClientFactory.Instance.GetHairProfileCustomerExceptTab2(hairProfileModel).GetAwaiter().GetResult();
                if (response.Data != null && !String.IsNullOrEmpty(response.Data.AIResultNew))
                {
                    var r1 = (Newtonsoft.Json.Linq.JObject)JObject.Parse(JsonConvert.DeserializeObject<dynamic>(response.Data.AIResultNew.ToString()));
                    //var rn = (object)r1["score"];
                    //response.Data.HairTextureLabelAIResult = (string)r1["hairTextureLabel"];
                    //response.Data.HairTypeLabelAIResult = (string)r1["hairTypeLabel"];
                    //response.Data.LabelAIResult = (string)r1["label"];
                    //response.Data.AIResultNewDecoded = (JObject)rn;
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
            }
            return null;

        }

        [HttpPost]
        public async Task<IActionResult> CreateHHCPHairKitUser(CreateHHCPModel createHHCPModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.CreateHHCPHairKitUser(createHHCPModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        public async Task<IActionResult> SaveSalonNotes(SalonNotesModel salonNotesModel)
        {
            salonNotesModel.LoginUserId = Request.Cookies["UserId"];
            var response = await MyavanaAdminApiClientFactory.Instance.SaveSalonNotes(salonNotesModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SaveImageFromMobile([FromBody] IFormFile file)
        {
            return Content("1");
        }

        public class TLImagesModel
        {
            public string Name { get; set; }
            public IFormFile File { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> EnableDisableProfileView(EnableDisableProfileModel enableDisableProfileModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.EnableDisableProfileView(enableDisableProfileModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        #region Partial Views

        public PartialViewResult GetHHCPStrands(string id, string userId)
        {
            HairProfileCustomerModel hairProfileModel = new HairProfileCustomerModel();
            hairProfileModel.UserId = userId;
            hairProfileModel.HairProfileId = Convert.ToInt32(id);

            if (hairProfileModel != null)
            {
                hairProfileModel.LoginUserId = Request.Cookies["UserId"];
                var response = MyavanaAdminApiClientFactory.Instance.GetHairProfileCustomerTab2(hairProfileModel).GetAwaiter().GetResult();
       
                return PartialView("_hairAnalysis", response.Data);
            }
            return null;

        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetHairStrandUploadNotificationList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            
            var res = await MyavanaAdminApiClientFactory.Instance.GetHairStrandUploadNotificationList();
            IEnumerable<HairStrandUploadNotificationModel> filterProducts = res.ToList();


            try
            {
                IEnumerable<HairStrandUploadNotificationModel> codes = filterProducts.Select(e => new HairStrandUploadNotificationModel
                {
                  UserName=e.UserName,
                  SalonName=e.SalonName,
                  IsRead=e.IsRead,
                  Id=e.Id,
                  CreatedDate=e.CreatedOn.ToString("yyyy-MM-dd-hh-mm-ss"),
                  UserId=e.UserId,
                  HairProfileId=e.HairProfileId,
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, res.Count));

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateNotificationAsRead(HairStrandUploadNotificationModel notification)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.UpdateNotificationAsRead(notification);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpGet]
        public async Task<IActionResult> GetHairDiarySubmitNotificationList([DataTablesRequest] DataTablesRequest dataRequest)
        {

            var res = await MyavanaAdminApiClientFactory.Instance.GetHairDiarySubmitNotificationList();
            IEnumerable<DailyRoutineTrackerNotificationModel> filterProducts = res.ToList();


            try
            {
                IEnumerable<DailyRoutineTrackerNotificationModel> codes = filterProducts.Select(e => new DailyRoutineTrackerNotificationModel
                {
                    Id = e.Id,
                    CreatedDate = e.TrackTime.ToString("yyyy-MM-dd"),
                    IsRead = e.IsRead,
                    UserId = e.UserId,
                    CreatedOn = e.CreatedOn,
                    IsActive = e.IsActive,
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, res.Count));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNotificationHairDiaryAsRead(DailyRoutineTrackerNotificationModel notification)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.UpdateNotificationHairDiaryAsRead(notification);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
    }
}