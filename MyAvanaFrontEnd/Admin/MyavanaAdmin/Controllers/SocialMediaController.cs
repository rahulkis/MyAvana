using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyavanaAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookies")]
    public class SocialMediaController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        public SocialMediaController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
            ApplicationSettings.SiteUrl = app.Value.SiteUrl;
        }

        public IActionResult EducationalVideos()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewSocialMedia([DataTablesRequest] DataTablesRequest dataRequest)
        {
            var loginUserId = Convert.ToInt32(Request.Cookies["UserId"]);
            IEnumerable<MediaLinkEntityModel> lstVideos = await MyavanaAdminApiClientFactory.Instance.GetEducationalVideos(loginUserId);
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<MediaLinkEntityModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.VideoId);
                            lstVideos =
                                sortDirection == "asc"
                                    ? lstVideos.OrderBy(orderingFunctionString)
                                    : lstVideos.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 1:
                        {
                            orderingFunctionString = (c => c.Description);
                            lstVideos =
                                sortDirection == "asc"
                                    ? lstVideos.OrderBy(orderingFunctionString)
                                    : lstVideos.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 2:
                        {
                            orderingFunctionString = (c => c.Title);
                            lstVideos =
                                sortDirection == "asc"
                                    ? lstVideos.OrderBy(orderingFunctionString)
                                    : lstVideos.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<MediaLinkEntityModel> codes = lstVideos.Select(e => new MediaLinkEntityModel
                {
                    Id = e.Id,
                    VideoId = e.VideoId,
                    Description = e.Description,
                    Title = e.Title,
                    ImageLink = e.ImageLink,
                    Header = e.Header,
                    IsFeatured = e.IsFeatured,
                    IsActive = e.IsActive,
                    ShowOnMobile = e.ShowOnMobile,
                    VideoCategory = e.VideoCategory
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IActionResult> CreateVideo(string id)
        {
            if (id != null)
            {
                MediaLinkEntityModel mediaLinkEntity = new MediaLinkEntityModel();
                mediaLinkEntity.Id = new Guid(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetMediaById(mediaLinkEntity);
                mediaLinkEntity = response.Data;
                return View(mediaLinkEntity);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateVideo(MediaLinkEntityModel mediaLinkEntity)
        {
            if(mediaLinkEntity.ImageLink != null && mediaLinkEntity.ImageLink.Contains("instagram"))
            {
                mediaLinkEntity.ImageLink = await VideoThumbnail(mediaLinkEntity.ImageLink);
            }
            mediaLinkEntity.VideoCategoryId = Convert.ToInt32(mediaLinkEntity.CategoryId);
            var loginUserId = Convert.ToInt32(Request.Cookies["UserId"]);
            var response = await MyavanaAdminApiClientFactory.Instance.SaveMediaLink(mediaLinkEntity,loginUserId);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        public async Task<string> VideoThumbnail(string videourl)
        {
            string customThumbNail = RandomNumbers() + ".jpg";
            string instagramVideoUrl = videourl;
            string requestUrl = videourl + "/?__a=1&__d=dis";

            try
            {
                HttpClient _httpClient = new HttpClient();

                // Set the request headers
                _httpClient.DefaultRequestHeaders.Add("Referer", instagramVideoUrl);
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.96 Safari/537.36");
                _httpClient.DefaultRequestHeaders.Add("Accept", "text/html");
                _httpClient.DefaultRequestHeaders.Add("Accept-Charset", "UTF-8");
                HttpResponseMessage response =  _httpClient.GetAsync(requestUrl).Result;
    
                string data = await response.Content.ReadAsStringAsync();
                dynamic result = JObject.Parse(data);
                var js = JsonConvert.SerializeObject(result);
                Console.Write("between : " + js);
                Root root = JsonConvert.DeserializeObject<Root>(js);
                var image = root.graphql.shortcode_media.display_url;
                WebClient webClient = new WebClient();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Thumbnails", customThumbNail);
                webClient.DownloadFile(image, path);
                webClient.Dispose();
                _httpClient.Dispose();
            }
            catch (Exception ex)
            {
                Console.Write("Error : " + ex.Message);
                return null;
            }
            string url = ApplicationSettings.SiteUrl+"/Thumbnails/" + customThumbNail;
            return url;
        }

        Random _random = new Random();
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  
            
            for (var i = 0; i < size; i++)
            {
                var @char = (char)_random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        public string RandomNumbers()
        {
            var passwordBuilder = new StringBuilder();

            // 4-Letters lower case   
            passwordBuilder.Append(RandomString(4, true));

            // 4-Digits between 1000 and 9999  
            passwordBuilder.Append(RandomNumber(1000, 9999));

            // 2-Letters upper case  
            passwordBuilder.Append(RandomString(2));
            return passwordBuilder.ToString();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVideo(MediaLinkEntityModel mediaLinkEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteVideo(mediaLinkEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }

       public  class Thumbnail
        {
            public string videourl { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CreateEducationalTip(EducationalTipModel tipModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.AddEducationalTip(tipModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        public async Task<IActionResult> CreateEducationalTip(string id)
        {
            if (id != null)
            {
                EducationalTipModel tipModel = new EducationalTipModel();
                tipModel.EducationTipsId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetEducationalTipById(tipModel);
                tipModel = response.Data;
                return View(tipModel);
            }
            return View();
        }
        public IActionResult EducationalTips()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetEducationalTips([DataTablesRequest] DataTablesRequest dataRequest)
        {
            var start = Convert.ToInt32(HttpContext.Request.Query["start"]);
            var length = Convert.ToInt32(HttpContext.Request.Query["length"]);
            IEnumerable<EducationalTipModel> filteredTipss = await MyavanaAdminApiClientFactory.Instance.GetEducationalTips(start, length);
            int TotalRecords = 0;
            if (filteredTipss.Count() > 0)
            {
                TotalRecords = filteredTipss.FirstOrDefault().TotalRecords;
            }
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<EducationalTipModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filteredTipss =
                                sortDirection == "asc"
                                    ? filteredTipss.OrderBy(orderingFunctionString)
                                    : filteredTipss.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }

            try
            {
                IEnumerable<EducationalTipModel> educationalTips = filteredTipss.Select(e => new EducationalTipModel
                {
                    EducationTipsId = e.EducationTipsId,
                    Description = e.Description,
                    ShowOnMobile = e.ShowOnMobile,
                });
                return Json(educationalTips.ToDataTablesResponse(dataRequest, educationalTips.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}
