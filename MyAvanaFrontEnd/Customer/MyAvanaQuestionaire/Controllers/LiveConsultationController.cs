using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MyAvanaQuestionaire.Factory;
using MyAvanaQuestionaire.Models;
using MyAvanaQuestionaire.Utility;
using MyAvanaQuestionaireModel;
using Newtonsoft.Json;
using Twilio.Rest.Video.V1.Room;
using Twilio.Base;
using Twilio.Rest.Video.V1;
using Twilio;
using Twilio.Jwt.AccessToken;
using static Twilio.Rest.Video.V1.CompositionResource;
using System.Security.Claims;

namespace MyAvanaQuestionaire.Controllers
{
    public class LiveConsultationController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        private readonly HttpClient _httpClient;
        public IConfiguration configuration;
        private readonly Microsoft.Extensions.Options.IOptions<AppSettingsModel> config;
        private Uri BaseEndpoint;
        public LiveConsultationController(Microsoft.Extensions.Options.IOptions<AppSettingsModel> app, IConfiguration _configuration, Microsoft.Extensions.Options.IOptions<AppSettingsModel> config)
        {
            _httpClient = new HttpClient();
            this.config = config;
            BaseEndpoint = new Uri(config.Value.WebApiBaseUrl);
            ApplicationSettings.WebApiUrl = config.Value.WebApiBaseUrl;
            configuration = _configuration;
        }
        public async Task<IActionResult> LiveSchedule()
        {
            LiveConsultationUserDetails user = new LiveConsultationUserDetails();
            //user.UserEmail = "zeal@mailinator.com";
            user.UserEmail = HttpContext.Session.GetString("email");
            var response = await MyavanaCustomerApiClientFactory.Instance.FetchConsultationDetails(user);
            if(response.Data != null)
            {
                user = response.Data;
                DateTime date1 = user.Date;
                DateTime date2 = DateTime.Now;
                TimeSpan ts = date1 - date2;
                var diff = ts.TotalMinutes;
                if(diff>0 && diff < 3)
                {
                    user.IsValidToJoin = true;
                }
            }
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> SaveConsultationDetails(LiveConsultationUserDetails LiveConsultationUserDetails)
        {
            LiveConsultationUserDetails.userId = HttpContext.Session.GetString("id");
            LiveConsultationUserDetails.assignedTo = "support@test.com";
            var response = await MyavanaCustomerApiClientFactory.Instance.SaveConsultationDetails(LiveConsultationUserDetails);
            return Content("1");
        }

        
        public async Task<IActionResult> CardPayment(CheckoutRequest checkoutRequest)
        {
            //_httpClient.BaseAddress = new Uri("http://localhost:5003/api/");
            _httpClient.BaseAddress = BaseEndpoint;
            //checkoutRequest.SubscriptionId = "plan_GoYoNBwX2tAFiQ";
            checkoutRequest.SubscriptionId = "price_1KGxPoSIwXSxhdLCNtyHuJB5";
            string token = HttpContext.Session.GetString("token");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = _httpClient.PostAsync("Payments/CardPayment", CreateHttpContent<CheckoutRequest>(checkoutRequest)).Result;
            var data = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject(data);
            return Content("1");
        }



       
        [HttpPost]
        public async Task<JsonResult> JoinLiveConsultation(LiveConsultationModel liveConsultationModel)
        {
            //var claimsIdentity1 = (ClaimsIdentity)User.Identity;
            //string userId = (claimsIdentity1.Claims).Select(x => x.Value).FirstOrDefault();
            //liveConsultationModel.AspNetUserId = HttpContext.Session.GetString("id");
            liveConsultationModel.CustomerName = System.Guid.NewGuid().ToString();
            var response = await MyavanaCustomerApiClientFactory.Instance.JoinLiveConsultation(liveConsultationModel);

            if (response != null)
            {
                liveConsultationModel = response;
                if (response.IsCustomerJoined && response.IsAdminJoined && response.IsLeft != true && response.AlreadyJoined != true)
                {
                    liveConsultationModel = createTwilioRoomAndTokens(response);
                    liveConsultationModel = await MyavanaCustomerApiClientFactory.Instance.UpdateLiveConsultationInformation(liveConsultationModel);
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
            var response = await MyavanaCustomerApiClientFactory.Instance.CheckIsOtherParticipantReady(liveConsultationModel);

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
            var accountSid = configuration.GetSection("AccountSid").Value; // "ACda441ab5c0c9d8737554e0b02f646d69";
            var authToken = configuration.GetSection("AuthToken").Value; //"e75cdf050ba22284ce142bd2d6203ab4";
            TwilioClient.Init(accountSid, authToken);

            var room = RoomResource.Update(
                status: RoomResource.RoomStatusEnum.Completed,
                pathSid: liveConsultationModel.TwilioRoomSid
            );
            //liveInterviewModel.IsCompleted = true;

            var response = await MyavanaCustomerApiClientFactory.Instance.UpdateLiveConsultationInformation(liveConsultationModel);
            return Json("success");

        }
        public LiveConsultationModel createTwilioRoomAndTokens(LiveConsultationModel liveConsultationModel)
        {
            try
            {
                var accountSid = configuration.GetSection("AccountSid").Value; // "ACda441ab5c0c9d8737554e0b02f646d69";
                var authToken = configuration.GetSection("AuthToken").Value; //"e75cdf050ba22284ce142bd2d6203ab4";
                var apiKey = configuration.GetSection("ApiKey").Value; //"SKf3044071240b141ccc05b44764d3546e";
                var apiSecret = configuration.GetSection("ApiSecret").Value; //"GaNZIeOLzUvagGU3BrUL2bOw3iBdhSsp";

           
                var identity = liveConsultationModel.CustomerName;
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


        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

    }
}