using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;
using Twilio.Rest.Video.V1;
using Twilio;
using Twilio.Jwt.AccessToken;

namespace MyavanaAdmin.Controllers
{
    public class LiveConsultationController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        public IConfiguration configuration;
        public LiveConsultationController(IOptions<AppSettingsModel> app, IConfiguration _configuration)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
            configuration = _configuration;
        }
        public IActionResult LiveSchedule()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangeIsApproved(LiveConsultationUserDetails Isapprovedval)
        {
        
           var response = await MyavanaAdminApiClientFactory.Instance.ChangeIsApproved(Isapprovedval);

            if (response.message == "Success")
               return Content("1");
            else
                return Content("0");

        }
        public async Task<IActionResult> JoinConsultation(int id)
        {
            LiveConsultationUserDetails user = new LiveConsultationUserDetails();
            user.UserEmail = "admin@mailinator.com";
            user.LiveConsultationUserDetailsId = id;
            //var response = await MyavanaAdminApiClientFactory.Instance.FetchConsultationDetails(user);
            // user = response.Data;
            return View(user);
        }

        [HttpPost]
        public async Task<JsonResult> JoinLiveConsultation(LiveConsultationModel liveConsultationModel)
        {
            //var claimsIdentity1 = (ClaimsIdentity)User.Identity;
            //string userId = (claimsIdentity1.Claims).Select(x => x.Value).FirstOrDefault();
            //liveConsultationModel.AspNetUserId = HttpContext.Session.GetString("id");
            liveConsultationModel.CustomerName = System.Guid.NewGuid().ToString();
            var response = await MyavanaAdminApiClientFactory.Instance.JoinLiveConsultation(liveConsultationModel);

            if (response != null)
            {
                liveConsultationModel = response;
                if (response.IsCustomerJoined && response.IsAdminJoined && response.IsLeft != true && response.AlreadyJoined != true)
                {
                    liveConsultationModel = createTwilioRoomAndTokens(response);
                    liveConsultationModel = await MyavanaAdminApiClientFactory.Instance.UpdateLiveConsultationInformation(liveConsultationModel);
                }
                return Json(liveConsultationModel);
            }
            else
            {
                return Json("failure");
            }
        }

        [HttpPost]
        public async Task<JsonResult> CheckIsOtherParticipantReady(LiveConsultationModel liveConsultationModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.CheckIsOtherParticipantReady(liveConsultationModel);

            if (response != null)
            {
                if (response.IsCustomerJoined && response.IsAdminJoined && !string.IsNullOrEmpty(response.CustomerToken) && !string.IsNullOrEmpty(response.AdminToken))
                {
                    return Json(response);
                }
                else
                    return Json("failure");
            }
            else
            {
                return Json("failure");
            }
        }

        [HttpPost]
        public async Task<JsonResult> CheckIsCandiateReady(LiveConsultationModel liveConsultationModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.CheckIsOtherParticipantReady(liveConsultationModel);

            if (response != null)
            {
                if (response.IsCustomerJoined && response.IsAdminJoined && !string.IsNullOrEmpty(response.CustomerToken) && !string.IsNullOrEmpty(response.AdminToken))
                {
                    return Json(response);
                }
                else
                    return Json("failure");
            }
            else
            {
                return Json("failure");
            }
        }

        [HttpPost]
        public async Task<JsonResult> CompleteTwilioRoom(LiveConsultationModel liveConsultationModel)
        {
            //var accountSid = "ACde37412e9194ee6594a7949010704464";
            //var authToken = "71929af3706dea6b7155e8f23b5df97e";
            //var accountSid = configuration.GetSection("AccountSid").Value; // "ACda441ab5c0c9d8737554e0b02f646d69";
            //var authToken = configuration.GetSection("AuthToken").Value; //"e75cdf050ba22284ce142bd2d6203ab4";
            var accountSid = "ACc1ce2b511c22f19ddb487aa4ef2f5d0c";
            var authToken = "2673b39855a14d9637c2d06c5d91ac55";
            TwilioClient.Init(accountSid, authToken);

            var room = RoomResource.Update(
                status: RoomResource.RoomStatusEnum.Completed,
                pathSid: liveConsultationModel.TwilioRoomSid
            );
            //liveInterviewModel.IsCompleted = true;

            var response = await MyavanaAdminApiClientFactory.Instance.UpdateLiveConsultationInformation(liveConsultationModel);
            return Json("success");

        }
        public LiveConsultationModel createTwilioRoomAndTokens(LiveConsultationModel liveConsultationModel)
        {
            try
            {
                //var accountSid = configuration.GetSection("AccountSid").Value; // "ACda441ab5c0c9d8737554e0b02f646d69";
                //var authToken = configuration.GetSection("AuthToken").Value; //"e75cdf050ba22284ce142bd2d6203ab4";
                // var apiKey = configuration.GetSection("ApiKey").Value; //"SKf3044071240b141ccc05b44764d3546e";
                // var apiSecret = configuration.GetSection("ApiSecret").Value; //"GaNZIeOLzUvagGU3BrUL2bOw3iBdhSsp";

                var accountSid = "ACc1ce2b511c22f19ddb487aa4ef2f5d0c";
                var authToken = "2673b39855a14d9637c2d06c5d91ac55";
                var apiKey = "SK1937453fa219d186a5409f2619a97825";
                var apiSecret = "M15mZTFbqCu20b1BTw2GTI2UT7fBs853";

                var identity = System.Guid.NewGuid().ToString();
                //var identity = liveConsultationModel.CustomerName;
                TwilioClient.Init(accountSid, authToken);
                var room = RoomResource.Create(
                    recordParticipantsOnConnect: true,
                    type: RoomResource.RoomTypeEnum.GroupSmall,
                    uniqueName: "LiveRoom_" + System.Guid.NewGuid()
                );
                liveConsultationModel.TwilioRoomName = room.UniqueName;
                liveConsultationModel.TwilioRoomName = room.UniqueName;
                liveConsultationModel.TwilioRoomSid = room.Sid;
                var grant = new VideoGrant();
                grant.Room = liveConsultationModel.TwilioRoomSid;

                var grants = new HashSet<IGrant> { grant };

                // Create an Access Token generator
                var token = new Token(accountSid, apiKey, apiSecret, identity: liveConsultationModel.CustomerName, grants: grants);
                liveConsultationModel.CustomerToken = token.ToJwt().ToString();
                token = new Token(accountSid, apiKey, apiSecret, identity: "Candace", grants: grants);
                liveConsultationModel.AdminToken = token.ToJwt().ToString();

                return liveConsultationModel;
            }
            catch (Exception ex)
            {
                //CommonController.SendEmail(ex.InnerException, "twilio");
                return liveConsultationModel;
            }

        }

    }
}