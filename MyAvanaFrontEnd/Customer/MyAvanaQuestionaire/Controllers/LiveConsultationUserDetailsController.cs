using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyAvanaQuestionaire.Models;
using MyAvanaQuestionaire.Utility;
using MyAvanaQuestionaireModel;
using Newtonsoft.Json;

namespace MyAvanaQuestionaire.Controllers
{
    public class LiveConsultationUserDetailsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<AppSettingsModel> config;
        private Uri BaseEndpoint;
        private readonly AppSettingsModel apiSettings;
        public LiveConsultationUserDetailsController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = "http://localhost:5004/api/"; // app.Value.WebApiBaseUrl https://api.myavana.com/;
        }
        private Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri(BaseEndpoint, relativePath);
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }
        private void addHeaders()
        {
            _httpClient.DefaultRequestHeaders.Remove("userIP");
            _httpClient.DefaultRequestHeaders.Add("userIP", "192.168.1.1");
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
        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        [HttpPost]
        public async Task<IActionResult> SaveConsultationDetails(Models.LiveConsultationUserDetails LiveConsultationUser)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = BaseEndpoint;
                    
                  
                        var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "LiveConsultationUserDetails/SaveConsultationDetails"));
                        addHeaders();
                        var response = await client.PostAsync(requestUrl.ToString(), CreateHttpContent<Models.LiveConsultationUserDetails>(LiveConsultationUser));

                        if (response.StatusCode.ToString() == "OK")
                            return Content("1");
                        else
                            return Content("0");
                    
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

    }
}