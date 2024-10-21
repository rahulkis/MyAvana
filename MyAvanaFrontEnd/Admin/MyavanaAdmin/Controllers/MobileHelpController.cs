using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;
using System.Diagnostics;

namespace MyavanaAdmin.Controllers
{
    public class MobileHelpController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        private const string Video_thumbnail_FOLDER = "wwwroot/MobileHelpFaq/Videos";
        private const string IMAGE_FOLDER = "wwwroot/MobileHelpFaq/Images";
        public MobileHelpController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
            ApplicationSettings.SiteUrl = app.Value.SiteUrl;
        }

        public IActionResult MobileHelpFAQList()
        {
            return View();
        }

        public async Task<IActionResult> CreateMobileHelpFAQ(string id)
        {
            if (id != null)
            {
                MobileHelpFAQ FAQ = new MobileHelpFAQ();
                FAQ.MobileHelpFAQId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetMobileHelpFaqById(FAQ);
                FAQ = response.Data;
                return View(FAQ);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMobileHelpFAQ(MobileHelpFAQ mobileHelpFAQ, IFormFile File, IFormFile imageFile)
        {
            var loginUserId = Convert.ToInt32(Request.Cookies["UserId"]);
            mobileHelpFAQ.CreatedBy = loginUserId.ToString();
            if (File != null)
            {
                Random generator = new Random();
                string random = generator.Next(0, 1000000).ToString("D6");
                string imgExt = Path.GetExtension(File.FileName);
                string fileName = File.FileName.Substring(0, File.FileName.IndexOf(".")) + "_" + random + imgExt;

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), IMAGE_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, IMAGE_FOLDER));
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), IMAGE_FOLDER, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await File.CopyToAsync(stream);
                    mobileHelpFAQ.ImageLink = ApplicationSettings.SiteUrl + "/MobileHelpFaq/Images/" + fileName;
                    //mobileHelpFAQ.ImageLink = "https://admin.myavana.com/MobileHelpFaq/Images/" + fileName;
                }
            }

            if (imageFile != null)
            {
                Random generator = new Random();
                String random = generator.Next(0, 1000000).ToString("D6");
                string videothumbnailExt = Path.GetExtension(imageFile.FileName);
                string imageFileName = imageFile.FileName.Substring(0, imageFile.FileName.IndexOf(".")) + "_" + random + videothumbnailExt;

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), Video_thumbnail_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, Video_thumbnail_FOLDER));
                }

                var videoPath = Path.Combine(Directory.GetCurrentDirectory(), Video_thumbnail_FOLDER, imageFileName);

                using (var videoStream = new FileStream(videoPath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(videoStream);
                    //mobileHelpFAQ.VideoThumbnail = "https://admin.myavana.com/MobileHelpFaq/Video-thumbnail/" + imageFileName;

                    mobileHelpFAQ.VideoThumbnail = ApplicationSettings.SiteUrl + "/MobileHelpFaq/Videos/" + imageFileName;
                    //mobileHelpFAQ.VideoThumbnail = "https://admin.myavana.com/MobileHelpFaq/Videos/" + imageFileName;
                }
            }

            var response = await MyavanaAdminApiClientFactory.Instance.SaveMobileHelpFAQ(mobileHelpFAQ);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        [HttpGet]
        public async Task<IActionResult> GetMobileHelpFaqList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            
            IEnumerable<MobileHelpFAQ> lstVideos = await MyavanaAdminApiClientFactory.Instance.GetMobileHelpFaqList();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<MobileHelpFAQ, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Videolink);
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
                IEnumerable<MobileHelpFAQ> codes = lstVideos.Select(e => new MobileHelpFAQ
                {
                    MobileHelpFAQId = e.MobileHelpFAQId,
                    Videolink = e.Videolink,
                    Description = e.Description,
                    Title = e.Title,
                    ImageLink = e.ImageLink,
                    VideoThumbnail = e.VideoThumbnail,
                    MobileHelpTopicId=e.MobileHelpTopicId,
                    MobileHelpTopicDescription=e.MobileHelpTopicDescription
                    
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteMobileHelpFaq(MobileHelpFAQ mobileHelpFaqModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteMobileHelpFaq(mobileHelpFaqModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }



    }
}
