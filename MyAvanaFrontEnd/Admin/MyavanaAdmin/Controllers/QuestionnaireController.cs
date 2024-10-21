using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyavanaAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookies")]
    public class QuestionnaireController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        public IConfiguration configuration;
        private readonly HttpClient _httpClient;
        private readonly string thumbnailImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/HairProfile/HairProfileThumbnails");
        public QuestionnaireController(IOptions<AppSettingsModel> app, IConfiguration _configuration)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
            configuration = _configuration;
            _httpClient = new HttpClient();
        }

        public IActionResult Questionnaire()
        {
            return View();
        }
        public IActionResult QuestionnaireList(string userId)
        {
            QuestionAnswerModel model = new QuestionAnswerModel();
            if (!string.IsNullOrEmpty(userId))
            {
                model.UserId = userId;
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetQuestionnaireList(string userId,[DataTablesRequest] DataTablesRequest dataRequest)
        {
            var start = Convert.ToInt32(HttpContext.Request.Query["start"]);
            var length = Convert.ToInt32(HttpContext.Request.Query["length"]);
            var loginUserId = Convert.ToInt32(Request.Cookies["UserId"]);
            IEnumerable<QuestionAnswerModel> filterQuestionnaires;
            if(!string.IsNullOrEmpty(userId))
            {
                filterQuestionnaires = await MyavanaAdminApiClientFactory.Instance.GetQuestionnaireForCustomer(userId);
            }
            else
            {
                filterQuestionnaires = await MyavanaAdminApiClientFactory.Instance.GetQuestionnaireList(start, length, loginUserId);
            }
            int TotalRecords = 0;
            if (filterQuestionnaires.Count() > 0)
            {
                TotalRecords = filterQuestionnaires.FirstOrDefault().TotalRecords;
            }
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<QuestionAnswerModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.UserEmail);
                            filterQuestionnaires =
                                sortDirection == "asc"
                                    ? filterQuestionnaires.OrderBy(orderingFunctionString)
                                    : filterQuestionnaires.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<QuestionAnswerModel> codes = filterQuestionnaires.Select(e => new QuestionAnswerModel
                {
                    UserId = e.UserId,
                    UserEmail = e.UserEmail,
                    CreatedOn = e.CreatedOn,
                    CreatedOnDate = e.CreatedOn.ToString("yyyy-MM-dd  HH:mm:ss tt"),
                    UserName = e.UserName,
                    questionModel = e.questionModel,
                    IsDraft = e.IsDraft,
                    CustomerTypeId = e.CustomerTypeId,
                    IsHHCPExist = e.IsHHCPExist,
                    CustomerQAFrom = e.CustomerQAFrom,
                    QuestionnaireCompleted = e.QuestionCount > 20 ? "Yes" : "Partially",
                    QA = e.QA
                }).OrderByDescending(x => x.CreatedOn);
                return Json(codes.ToDataTablesResponse(dataRequest, TotalRecords));

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<IActionResult> AddHairProfile(string id, string name, int? isNewHHCP, int QaNumber)
        {
            HairProfile hair = new HairProfile();
            if (id != null)
            {
                hair.UserEmail = id.Trim();
                hair.UserId = name;
                hair.FirstName = name.Substring(0, name.IndexOf(" "));
                hair.IsNewHHCP = isNewHHCP;
                hair.QA = QaNumber;
            }
            return View(hair);
        }

        [HttpPost]
        public async Task<IActionResult> GetQuestionaireAnswer(QuestionaireSelectedAnswer hairProfileModel)
        {

            if (hairProfileModel.UserId != null)
            {
                hairProfileModel.UserEmail = hairProfileModel.UserId.Trim();
                var response = await MyavanaAdminApiClientFactory.Instance.GetQuestionaireAnswer(hairProfileModel);
                if (response.message == "Success")
                {
                    return Json(response.Data);
                }
            }
            return null;
        }

        public async Task<IActionResult> SaveProfile(HairProfile hairProfile)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);
                    MultipartFormDataContent multiContent = new MultipartFormDataContent();

                    if (hairProfile.TopLeftPhoto != null)
                    {
                        multiContent.Add(new StringContent(hairProfile.TopLeftPhoto), "TopLeftPhoto");
                    }

                    if (hairProfile.TopRightPhoto != null)
                    {
                        multiContent.Add(new StringContent(hairProfile.TopRightPhoto), "TopRightPhoto");
                    }

                    if (hairProfile.BottomLeftPhoto != null)
                    {
                        multiContent.Add(new StringContent(hairProfile.BottomLeftPhoto), "BottomLeftPhoto");
                    }

                    if (hairProfile.BottomRightPhoto != null)
                    {
                        multiContent.Add(new StringContent(hairProfile.BottomRightPhoto), "BottomRightPhoto");
                    }

                    if (hairProfile.CrownPhoto != null)
                    {
                        multiContent.Add(new StringContent(hairProfile.CrownPhoto), "CrownPhoto");
                    }

                    multiContent.Add(new StringContent(hairProfile.UserId.ToString()), "UserId");
                    if (hairProfile.HairId != null)
                        multiContent.Add(new StringContent(hairProfile.HairId.ToString()), "HairId");
                    if (hairProfile.ConsultantNotes != null)
                        multiContent.Add(new StringContent(hairProfile.ConsultantNotes.ToString()), "ConsultantNotes");
                    if (hairProfile.RecommendationNotes != null)
                        multiContent.Add(new StringContent(hairProfile.RecommendationNotes.ToString()), "RecommendationNotes");
                    if (hairProfile.HealthSummary != null)
                        multiContent.Add(new StringContent(hairProfile.HealthSummary), "HealthSummary");
                    if (hairProfile.TopLeftStrandDiameter != null)
                        multiContent.Add(new StringContent(hairProfile.TopLeftStrandDiameter), "TopLeftStrandDiameter");
                    if (hairProfile.TopRightStrandDiameter != null)
                        multiContent.Add(new StringContent(hairProfile.TopRightStrandDiameter), "TopRightStrandDiameter");
                    if (hairProfile.BottomLeftStrandDiameter != null)
                        multiContent.Add(new StringContent(hairProfile.BottomLeftStrandDiameter), "BottomLeftStrandDiameter");
                    if (hairProfile.BottoRightStrandDiameter != null)
                        multiContent.Add(new StringContent(hairProfile.BottoRightStrandDiameter), "BottoRightStrandDiameter");
                    if (hairProfile.CrownStrandDiameter != null)
                        multiContent.Add(new StringContent(hairProfile.CrownStrandDiameter), "CrownStrandDiameter");
                    if (hairProfile.TempHealth != null)
                        multiContent.Add(new StringContent(hairProfile.TempHealth), "TempHealth");

                    if (hairProfile.TempObservation != null)
                        multiContent.Add(new StringContent(hairProfile.TempObservation), "TempObservation");
                    if (hairProfile.TempObservationElasticity != null)
                        multiContent.Add(new StringContent(hairProfile.TempObservationElasticity), "TempObservationElasticity");
                    if (hairProfile.TempObservationChemicalProduct != null)
                        multiContent.Add(new StringContent(hairProfile.TempObservationChemicalProduct), "TempObservationChemicalProduct");
                    if (hairProfile.TempObservationPhysicalProduct != null)
                        multiContent.Add(new StringContent(hairProfile.TempObservationPhysicalProduct), "TempObservationPhysicalProduct");
                    //if (hairProfile.TempObservationDamage != null)
                    //    multiContent.Add(new StringContent(hairProfile.TempObservationDamage), "TempObservationDamage");
                    if (hairProfile.TempObservationBreakage != null)
                        multiContent.Add(new StringContent(hairProfile.TempObservationBreakage), "TempObservationBreakage");
                    if (hairProfile.TempObservationSplitting != null)
                        multiContent.Add(new StringContent(hairProfile.TempObservationSplitting), "TempObservationSplitting");

                    if (hairProfile.TempPororsity != null)
                        multiContent.Add(new StringContent(hairProfile.TempPororsity), "TempPororsity");
                    if (hairProfile.TempElasticity != null)
                        multiContent.Add(new StringContent(hairProfile.TempElasticity), "TempElasticity");
                    if (hairProfile.TempRecommendedProducts != null)
                        multiContent.Add(new StringContent(hairProfile.TempRecommendedProducts), "TempRecommendedProducts");
                    if (hairProfile.TempRecommendedProductsStylings != null)
                        multiContent.Add(new StringContent(hairProfile.TempRecommendedProductsStylings), "TempRecommendedProductsStylings");
                    if (hairProfile.TempRecommendedIngredients != null)
                        multiContent.Add(new StringContent(hairProfile.TempRecommendedIngredients), "TempRecommendedIngredients");
                    if (hairProfile.TempRecommendedCategories != null)
                        multiContent.Add(new StringContent(hairProfile.TempRecommendedCategories), "TempRecommendedCategories");
                    if (hairProfile.TempRecommendedProductTypes != null)
                        multiContent.Add(new StringContent(hairProfile.TempRecommendedProductTypes), "TempRecommendedProductTypes");
                    if (hairProfile.TempRecommendedTools != null)
                        multiContent.Add(new StringContent(hairProfile.TempRecommendedTools), "TempRecommendedTools");
                    if (hairProfile.TempRecommendedRegimens != null)
                        multiContent.Add(new StringContent(hairProfile.TempRecommendedRegimens), "TempRecommendedRegimens");
                    if (hairProfile.TempRecommendedVideos != null)
                        multiContent.Add(new StringContent(hairProfile.TempRecommendedVideos), "TempRecommendedVideos");
                    if (hairProfile.TempRecommendedStylist != null)
                        multiContent.Add(new StringContent(hairProfile.TempRecommendedStylist), "TempRecommendedStylist");
                    if (hairProfile.TempSelectedAnswer != null)
                        multiContent.Add(new StringContent(hairProfile.TempSelectedAnswer), "TempSelectedAnswer");
                    if (hairProfile.SaveType != null)
                        multiContent.Add(new StringContent(hairProfile.SaveType), "SaveType");
                    if (hairProfile.TabNo != null)
                        multiContent.Add(new StringContent(hairProfile.TabNo), "TabNo");
                    if (hairProfile.TempAllRecommendedProductsEssential != null)
                        multiContent.Add(new StringContent(hairProfile.TempAllRecommendedProductsEssential), "TempAllRecommendedProductsEssential");
                    if (hairProfile.TempAllRecommendedProductsStyling != null)
                        multiContent.Add(new StringContent(hairProfile.TempAllRecommendedProductsStyling), "TempAllRecommendedProductsStyling");
                    if (hairProfile.TempRecommendedStyleRecipeVideos != null)
                        multiContent.Add(new StringContent(hairProfile.TempRecommendedStyleRecipeVideos), "TempRecommendedStyleRecipeVideos");
                    if (hairProfile.TempRecommendedProductsStyleRecipe != null)
                        multiContent.Add(new StringContent(hairProfile.TempRecommendedProductsStyleRecipe), "TempRecommendedProductsStyleRecipe");
                    if (hairProfile.HairStyleId != null)
                        multiContent.Add(new StringContent(hairProfile.HairStyleId), "HairStyleId");
                    multiContent.Add(new StringContent(hairProfile.NotifyUser.ToString()), "NotifyUser");
                    if (hairProfile.IsNewHHCP != null)
                        multiContent.Add(new StringContent(hairProfile.IsNewHHCP.ToString()), "IsNewHHCP");
                    if (hairProfile.HairAnalysisNotes != null)
                        multiContent.Add(new StringContent(hairProfile.HairAnalysisNotes.ToString()), "HairAnalysisNotes");
                    if (hairProfile.MyNotes != null)
                        multiContent.Add(new StringContent(hairProfile.MyNotes.ToString()), "MyNotes");

                    var loginUserId = Request.Cookies["UserId"];
                    if (!string.IsNullOrEmpty(loginUserId))
                    {
                        hairProfile.LoginUserId = loginUserId;
                        multiContent.Add(new StringContent(hairProfile.LoginUserId.ToString()), "LoginUserId");
                    }
                    multiContent.Add(new StringContent(hairProfile.CreatedBy.ToString()), "CreatedBy");
                    if (hairProfile.HairProfileId != null)
                        multiContent.Add(new StringContent(hairProfile.HairProfileId.ToString()), "HairProfileId");

                    if (hairProfile.QA != 0)
                        hairProfile.QA = hairProfile.QA - 1;
                    else
                        hairProfile.QA = null;

                    multiContent.Add(new StringContent(hairProfile.QA.ToString()), "QA");

                    if (hairProfile.ModifiedBy != null)
                    {
                        multiContent.Add(new StringContent(hairProfile.ModifiedBy.ToString()), "ModifiedBy");
                    }

                    var result = client.PostAsync("HairProfile/SaveProfile", multiContent).Result;
                    result.EnsureSuccessStatusCode();
                    var data = await result.Content.ReadAsStringAsync();
                    dynamic ResponseData = JObject.Parse(data);
                    HairProfile Data = JsonConvert.DeserializeObject<HairProfile>(Convert.ToString(ResponseData.data));

                    //return result;
                    if ((int)result.StatusCode == 200)
                        if (hairProfile.SaveType == "normal")
                            return Content("1,"+ Data.HairProfileId);
                        else
                            return Content("2," + Data.HairProfileId);

                    else
                        return Content("0");
                }
                catch (Exception ex)
                {
                    return StatusCode(500);
                }
            }
        }

        public async Task<IActionResult> GetHairProfile(HairProfileAdminModel hairProfileModel)
        {
            try
            {
                if (hairProfileModel != null)
                {
                    if (hairProfileModel.IsNewHHCP == 1)
                    {
                        return Json(null);
                    }
                    hairProfileModel.LoginUserId = Request.Cookies["UserId"];
                    var response = await MyavanaAdminApiClientFactory.Instance.GetHairProfileAdmin(hairProfileModel);
                    if (response != null)
                    {
                        return Json(response.Data);
                    }
                    return Content("0");
                }
            }
            catch (Exception ex) { }
            return Content("0");
        }

        public IActionResult QuestionnaireCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> QuestionnaireCustomerList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][data]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            SearchCustomerResponse gridParams = new SearchCustomerResponse();
            gridParams.pageSize = pageSize;
            gridParams.skip = skip;
            gridParams.sortColumn = sortColumn;
            gridParams.sortDirection = sortColumnDirection;
            gridParams.searchValue = searchValue;
            gridParams.userId = Convert.ToInt32(Request.Cookies["UserId"]);

            var res = await MyavanaAdminApiClientFactory.Instance.GetQuestionnaireCustomerList(gridParams);
            IEnumerable<QuestionnaireCustomerList> filterQuestionnaire = res.Data.Data;

            try
            {
                IEnumerable<QuestionnaireCustomerList> blogs = filterQuestionnaire.Select(e => new QuestionnaireCustomerList
                {
                    UserId = e.UserId,
                    UserName = e.UserName,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    UserEmail = e.UserEmail,
                    IsQuestionnaire = e.IsQuestionnaire,
                    IsProCustomer = e.IsProCustomer,
                    Active = e.Active,
                    CustomerTypeDescription = e.CustomerTypeDescription,
                    StatusTrackerStatus = e.StatusTrackerStatus,
                    IsInfluencer=e.IsInfluencer

                }).OrderByDescending(x => x.CreatedAt);
                return Json(blogs.ToDataTablesResponse(dataRequest, res.Data.RecordsTotal));
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public IActionResult start(string id)
        {
            if (id != null)
            {
                ViewBag.id = id;
            }
            return View();
        }

        public IActionResult QuestionaireSurvey(string id, string userId)
        {
            ViewBag.Token = userId;
            ViewBag.Check = id;
            return View();
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
        public async Task<IActionResult> SaveImage(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    string fileName = RandomNumbers() + ".jpg";
                    string extension = file.FileName.Substring(file.FileName.LastIndexOf("."));
                    if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "questionnaireFile")))
                    {
                        Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "questionnaireFile"));
                    }
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "questionnaireFile", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return Content(fileName);
                }
                else
                    return Content("0");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
        [HttpPost]
        public async Task<QuestionAnswersModel> GetUserQuestionaire(string uId,int qaId)
        {
            if (qaId != 0)
            {
                qaId = qaId - 1;
            }
            //var claimsIdentity1 = (ClaimsIdentity)User.Identity;
            string userId = uId; // (claimsIdentity1.Claims).Select(x => x.Value).FirstOrDefault();
            using (var client = new HttpClient())
            {

                try
                {

                    var requestUrl = client.GetAsync(ApplicationSettings.WebApiUrl + "Questionnaire/GetQuestionnaireCustomer?id=" + userId+ "&QA=" + qaId).Result;

                    var data = await requestUrl.Content.ReadAsStringAsync();
                    dynamic result = JObject.Parse(data);
                    QuestionAnswersModel questionaire = JsonConvert.DeserializeObject<QuestionAnswersModel>(Convert.ToString(result.data));
                    return questionaire;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> SaveQuestionaire([FromBody] IEnumerable<Questionaire> questionaire)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //client.BaseAddress = BaseEndpoint;
                    if (questionaire.Count() != 0)
                    {
                        var response = await MyavanaAdminApiClientFactory.Instance.SaveQuestionnaireSurvey(questionaire);

                        if (response.message == "Success")
                            return Content("1");
                        else
                            return Content("0");
                    }
                    else
                    {
                        return Content("0");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        public async Task<List<FileModel>> AddTopLeftFiles(List<IFormFile> files)
        {
            if (!Directory.Exists(thumbnailImagePath))
            {
                Directory.CreateDirectory(thumbnailImagePath);
            }
            List<FileModel> fileModelListing = new List<FileModel>();
            List<FileModel> fileModifiedNames = new List<FileModel>();
            foreach (var imageFile in files)
            {
                fileModelListing.Add(new FileModel { FileName = imageFile.Name.Replace(" ", ""), FormFile = imageFile });
            }
            var path = string.Empty;
            foreach (var fileDataModel in fileModelListing)
            {
                string fileName = fileDataModel.FormFile.FileName.Replace(" ", "");
                string uniqueFileName = DateTime.Now.ToString("yyyymmddMMss") + "_" + fileDataModel.FormFile.FileName.Replace(" ", "");
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HairProfile", uniqueFileName);

                fileModifiedNames.Add(new FileModel { FileName = fileName, UniqueFileName = uniqueFileName });
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await fileDataModel.FormFile.CopyToAsync(stream);
                }
                var thumbnailPath = Path.Combine(thumbnailImagePath, uniqueFileName);
                using (var originalImage = Image.FromFile(path))
                {
                    GenerateThumbnailAndSave(originalImage, thumbnailPath);
                }
            }
            return fileModifiedNames;
        }
        public void GenerateThumbnailAndSave(Image originalImage, string outputPath)
        {
            try
            {
                double scale = 0.3;
                int newWidth = (int)(originalImage.Width * scale);
                int newHeight = (int)(originalImage.Height * scale);
                using (Bitmap thumbnail = new Bitmap(newWidth, newHeight))
                {
                    using (Graphics graphics = Graphics.FromImage(thumbnail))
                    {
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                    }
                    thumbnail.Save(outputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            catch(Exception e)
            {

            }
        }
        [AllowAnonymous]
        public string RemoveTopLeftFiles(string fileName)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HairProfile", fileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return "1";
            }
            catch (Exception)
            {
                return "0";
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        public async Task<List<FileModel>> AddTopRightFiles(List<IFormFile> files)
        {
            if (!Directory.Exists(thumbnailImagePath))
            {
                Directory.CreateDirectory(thumbnailImagePath);
            }
            List<FileModel> fileModelListing = new List<FileModel>();
            List<FileModel> fileModifiedNames = new List<FileModel>();
            foreach (var imageFile in files)
            {
                fileModelListing.Add(new FileModel { FileName = imageFile.Name.Replace(" ", ""), FormFile = imageFile });
            }
            var path = string.Empty;

            foreach (var fileDataModel in fileModelListing)
            {
                string fileName = fileDataModel.FormFile.FileName.Replace(" ", "");
                string uniqueFileName = DateTime.Now.ToString("yyyymmddMMss") + "_" + fileDataModel.FormFile.FileName.Replace(" ", "");
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HairProfile", uniqueFileName);

                fileModifiedNames.Add(new FileModel { FileName = fileName, UniqueFileName = uniqueFileName });
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await fileDataModel.FormFile.CopyToAsync(stream);
                }
                var thumbnailPath = Path.Combine(thumbnailImagePath, uniqueFileName);
                using (var originalImage = Image.FromFile(path))
                {
                    GenerateThumbnailAndSave(originalImage, thumbnailPath);
                }
            }
            return fileModifiedNames;
        }

        [AllowAnonymous]
        public string RemoveTopRightFiles(string fileName)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HairProfile", fileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return "1";
            }
            catch (Exception)
            {
                return "0";
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        public async Task<List<FileModel>> AddBottomLeftFiles(List<IFormFile> files)
        {
            if (!Directory.Exists(thumbnailImagePath))
            {
                Directory.CreateDirectory(thumbnailImagePath);
            }
            List<FileModel> fileModelListing = new List<FileModel>();
            List<FileModel> fileModifiedNames = new List<FileModel>();
            foreach (var imageFile in files)
            {
                fileModelListing.Add(new FileModel { FileName = imageFile.Name.Replace(" ", ""), FormFile = imageFile });
            }
            var path = string.Empty;
            foreach (var fileDataModel in fileModelListing)
            {
                string fileName = fileDataModel.FormFile.FileName.Replace(" ", "");
                string uniqueFileName = DateTime.Now.ToString("yyyymmddMMss") + "_" + fileDataModel.FormFile.FileName.Replace(" ", "");
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HairProfile", uniqueFileName);

                fileModifiedNames.Add(new FileModel { FileName = fileName, UniqueFileName = uniqueFileName });
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await fileDataModel.FormFile.CopyToAsync(stream);
                }
                var thumbnailPath = Path.Combine(thumbnailImagePath, uniqueFileName);
                using (var originalImage = Image.FromFile(path))
                {
                    GenerateThumbnailAndSave(originalImage, thumbnailPath);
                }
            }
            return fileModifiedNames;
        }

        [AllowAnonymous]
        public string RemoveBottomLeftFiles(string fileName)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HairProfile", fileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return "1";
            }
            catch (Exception)
            {
                return "0";
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        public async Task<List<FileModel>> AddBottomRightFiles(List<IFormFile> files)
        {
            if (!Directory.Exists(thumbnailImagePath))
            {
                Directory.CreateDirectory(thumbnailImagePath);
            }
            List<FileModel> fileModelListing = new List<FileModel>();
            List<FileModel> fileModifiedNames = new List<FileModel>();
            foreach (var imageFile in files)
            {
                fileModelListing.Add(new FileModel { FileName = imageFile.Name.Replace(" ", ""), FormFile = imageFile });
            }
            var path = string.Empty;
            foreach (var fileDataModel in fileModelListing)
            {
                string fileName = fileDataModel.FormFile.FileName.Replace(" ", "");
                string uniqueFileName = DateTime.Now.ToString("yyyymmddMMss") + "_" + fileDataModel.FormFile.FileName.Replace(" ", "");
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HairProfile", uniqueFileName);

                fileModifiedNames.Add(new FileModel { FileName = fileName, UniqueFileName = uniqueFileName });
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await fileDataModel.FormFile.CopyToAsync(stream);
                }
                var thumbnailPath = Path.Combine(thumbnailImagePath, uniqueFileName);
                using (var originalImage = Image.FromFile(path))
                {
                    GenerateThumbnailAndSave(originalImage, thumbnailPath);
                }
            }
            return fileModifiedNames;
        }

        [AllowAnonymous]
        public string RemoveBottomRightFiles(string fileName)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HairProfile", fileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return "1";
            }
            catch (Exception)
            {
                return "0";
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [DisableRequestSizeLimit]
        public async Task<List<FileModel>> AddCrownFiles(List<IFormFile> files)
        {
            if (!Directory.Exists(thumbnailImagePath))
            {
                Directory.CreateDirectory(thumbnailImagePath);
            }
            List<FileModel> fileModelListing = new List<FileModel>();
            List<FileModel> fileModifiedNames = new List<FileModel>();
            foreach (var imageFile in files)
            {
                fileModelListing.Add(new FileModel { FileName = imageFile.Name.Replace(" ", ""), FormFile = imageFile });
            }
            var path = string.Empty;
            foreach (var fileDataModel in fileModelListing)
            {
                string fileName = fileDataModel.FormFile.FileName.Replace(" ", "");
                string uniqueFileName = DateTime.Now.ToString("yyyymmddMMss") + "_" + fileDataModel.FormFile.FileName.Replace(" ", "");
                path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HairProfile", uniqueFileName);

                fileModifiedNames.Add(new FileModel { FileName = fileName, UniqueFileName = uniqueFileName });
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await fileDataModel.FormFile.CopyToAsync(stream);
                }
                var thumbnailPath = Path.Combine(thumbnailImagePath, uniqueFileName);
                using (var originalImage = Image.FromFile(path))
                {
                    GenerateThumbnailAndSave(originalImage, thumbnailPath);
                }
            }
            return fileModifiedNames;
        }

        [AllowAnonymous]
        public string RemoveCrownFiles(string fileName)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "HairProfile", fileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return "1";
            }
            catch (Exception)
            {
                return "0";
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteQuest(QuestModel quest)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteQuest(quest);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }

        [HttpPost]
        public async Task<IActionResult> ChangeCustomerType(UserModel userObj)
        {
            UserModel userModel = new UserModel();
            userModel.Id = new Guid(userObj.UserId);
            userModel.CustomerTypeId = userObj.CustomerTypeId;
            var response = await MyavanaAdminApiClientFactory.Instance.ChangeCustomerType(userModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }
        
        [HttpPost]
        public async Task<IActionResult> ActivateCustomer(UserModel userObj)
        {
            UserModel userModel = new UserModel();
            userModel.Id = new Guid(userObj.UserId);
            var response = await MyavanaAdminApiClientFactory.Instance.ActivateCustomer(userModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }

        public IActionResult QuestionnaireAbsence()
        {
            return View();
        }

        public IActionResult CustomerMessages(string userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CustomerMessageList([DataTablesRequest] DataTablesRequest dataRequest, string id)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var requestUrl = client.GetAsync(ApplicationSettings.WebApiUrl + "Questionnaire/GetCustomerMessagesList?id=" + id).Result;
                    var data = await requestUrl.Content.ReadAsStringAsync();
                    dynamic result = JObject.Parse(data);
                    IEnumerable<CustomerMessageViewModel> filterQuestionnaire = JsonConvert.DeserializeObject<List<CustomerMessageViewModel>>(Convert.ToString(result.data));
                    IEnumerable<CustomerMessageViewModel> blogs = filterQuestionnaire.Select(e => new CustomerMessageViewModel
                    {
                        EmailAddress = e.EmailAddress,
                        Subject = e.Subject,
                        Message = e.Message,
                        CreatedOn = e.CreatedOn,
                        AttachmentFile = e.AttachmentFile
                    }).OrderByDescending(x => x.CreatedOn);
                    return Json(blogs.ToDataTablesResponse(dataRequest, blogs.Count()));
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerTypeHistory([DataTablesRequest] DataTablesRequest dataRequest, string Id)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    var requestUrl = client.GetAsync(ApplicationSettings.WebApiUrl + "Questionnaire/GetCustomerTypeHistory?id=" + Id).Result;
                    var data = await requestUrl.Content.ReadAsStringAsync();
                    dynamic result = JObject.Parse(data);
                    IEnumerable<CustomerTypeHistory> customerHistory = JsonConvert.DeserializeObject<List<CustomerTypeHistory>>(Convert.ToString(result.data));
                    IEnumerable<CustomerTypeHistory> records = customerHistory.Select(e => new CustomerTypeHistory
                    {
                        OldCustomerType = e.OldCustomerType,
                        NewCustomerType = e.NewCustomerType,
                        Comment = e.Comment,
                        CreatedDate = e.CreatedOn.ToString("yyyy-MM-dd"),
                        UpdatedBy = e.UpdatedBy
                    }).OrderByDescending(x => x.CreatedOn);
                    return Json(records.ToDataTablesResponse(dataRequest, records.Count()));
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDailyRoutineWeb([DataTablesRequest] DataTablesRequest dataRequest, string userId, string trackTime)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var requestUrl = client.GetAsync(ApplicationSettings.WebApiUrl + "Questionnaire/GetDailyRoutineWeb?userId=" + userId + "&TrackTime=" + trackTime).Result;
                    var data = await requestUrl.Content.ReadAsStringAsync();

                    dynamic result = JObject.Parse(data);
                    DailyRoutineTracker dailyRoutine = JsonConvert.DeserializeObject<DailyRoutineTracker>(Convert.ToString(result.data));

                    // Assuming DailyRoutineTracker has the properties you need
                    DailyRoutineTracker record = new DailyRoutineTracker
                    {
                        ProfileImage = dailyRoutine.ProfileImage,
                        HairStyle = dailyRoutine.HairStyle,
                        Description = dailyRoutine.Description,
                        CurrentMood = dailyRoutine.CurrentMood,
                        GuidanceNeeded = dailyRoutine.GuidanceNeeded
                    };

                    return Json(new[] { record }.ToDataTablesResponse(dataRequest, 1));
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> SaveCustomerMessage(CustomerMessageModel messageModel, IFormFile File)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                PropertyInfo[] messageProperties = typeof(CustomerMessageModel).GetProperties();
                foreach (PropertyInfo prop in messageProperties)
                {
                    if (prop.Name != "Attachment" && prop.Name != "AttachmentFile" && prop.Name != "emailTemplate")
                    {
                        if (prop.GetValue(messageModel) != null)
                        {
                            multiContent.Add(new StringContent(prop.GetValue(messageModel).ToString()), prop.Name);
                        }
                        else
                        {
                            multiContent.Add(new StringContent(string.Empty), prop.Name);
                        }

                    }
                }

                //CustomerMessageModel customerMessageModel = new CustomerMessageModel();
                //customerMessageModel.EmailAddress = messageModel.EmailAddress;
                //customerMessageModel.Subject = messageModel.Subject;
                //customerMessageModel.Message = messageModel.Message;
                //customerMessageModel.UserId = messageModel.UserId;
                //customerMessageModel.UserName = messageModel.UserName;
                //customerMessageModel.emailTemplate = "EMAILTEMPLATE01";
                multiContent.Add(new StringContent("EMAILTEMPLATE01"), "emailTemplate");

                if (File != null)
                {
                    //string fileName = File.FileName.Substring(0, File.FileName.IndexOf(".")) + "_" + DateTime.Now.ToString("yyyyMMddTHHmmss") + File.FileName.Substring(File.FileName.IndexOf("."));
                    //var path = string.Empty;
                    //path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "CustomerMessageFiles", fileName);

                    //using (var stream = new FileStream(path, FileMode.Create))
                    //{
                    //    await File.CopyToAsync(stream);
                    //}
                    //multiContent.Add(new StringContent(File.FileName), "AttachmentFile");
                    multiContent.Add(new StreamContent(File.OpenReadStream())
                    {
                        Headers =
                             {
                               ContentLength = File.Length,
                               ContentType = new MediaTypeHeaderValue(File.ContentType)
                             }
                    }, "Attachment", File.FileName);

                    //customerMessageModel.AttachmentFile = fileName;
                    //customerMessageModel.Attachment = File;


                }
                try
                {


                    var response = client.PostAsync("Questionnaire/SaveCustomerMessage", multiContent).Result;

                    if ((int)response.StatusCode == 200)

                    {
                        //var data = await response.Content.ReadAsStringAsync();
                        //dynamic result1 = JObject.Parse(data);
                        //var email = result1.data.emailBody;
                        //CustomerMessageModel customerMessageModel = JsonConvert.DeserializeObject<CustomerMessageModel>(Convert.ToString(result1.data));
                        //var resultemail = await MyavanaAdminApiClientFactory.Instance.GetCustomerEmailTemplate();
                        //CommonController.SendEmailToCustomer(customerMessageModel, resultemail, File);
                        return "1";
                    }
                    else
                        return "0";
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            //var response = await MyavanaAdminApiClientFactory.Instance.SaveCustomerMessage(customerMessageModel);
            //if (response.result == "1")
            //{
            //    customerMessageModel.emailBody = response.Data.emailBody;
            //    var result1 = await MyavanaAdminApiClientFactory.Instance.GetCustomerEmailTemplate();

            //    CommonController.SendEmailToCustomer(customerMessageModel, result1, File);
            //    return "1";
            //}
            //else
            //    return "0";
        }

        public async Task<IActionResult> CreateNewCustomer(string id)
        {
            if (id != null)
            {
                
                var userDetail = _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "Account/GetUserById?userId=" + id).Result;
                if (userDetail.IsSuccessStatusCode)
                {
                    var dataUser = userDetail.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    UserEntityEditModel user = JsonConvert.DeserializeObject<UserEntityEditModel>(dataUser);
                    return View(user);
                }
            }
            UserEntityEditModel model = new UserEntityEditModel();
            return View(model);
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
        [HttpPost]
        public async Task<IActionResult> CreateNewCustomer(Signup signup)
        {
            _httpClient.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);
            var loginUserId = Convert.ToInt32(Request.Cookies["UserId"]);
            int userType =Convert.ToInt32(Request.Cookies["UserTypeId"]);
            if (userType == (int)UserTypeEnum.B2B)
            {
                var SalonResult = _httpClient.GetAsync("Account/GetSalonIdByUserId?UserId=" + loginUserId).Result;
                if (SalonResult.IsSuccessStatusCode)
                {
                    var salonId = SalonResult.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    signup.SalonId = Convert.ToInt32(salonId);
                    signup.CustomerType = false;
                    signup.CustomerTypeId = 2;  
                    signup.CreatedByUserId = loginUserId;
                }
            }
            else
            {
                signup.CustomerType = false;
                signup.CustomerTypeId = 2;
                signup.CreatedByUserId = loginUserId;
            }
            var response = _httpClient.PostAsync("Account/WebSignup", CreateHttpContent<Signup>(signup)).Result;

            var data = await response.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(data);
            Response res = null;
            try
            {
                res = JsonConvert.DeserializeObject<Response>(Convert.ToString(result));

            }
            catch (Exception ex)
            {
                return Content("0");
            }

            if (res.value.item1 != null)
                return Content(res.value.item1);
            else
                return Content("-1");
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<string> ResetCustomerPassword(SetPassword setPassword)
        {
            try
            {
                MultipartFormDataContent multiContent = new MultipartFormDataContent();

                multiContent.Add(new StringContent(setPassword.Email.ToString()), "Email");
                multiContent.Add(new StringContent(setPassword.Password), "Password");

                _httpClient.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);

                var response = _httpClient.PostAsync("Account/setcustomerpass", CreateHttpContent<SetPassword>(setPassword)).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return "1";
                else
                    return "0";
            }

            catch (Exception ex)
            {
                return "0";
            }
        }

        public PartialViewResult GetProductTab()
        {
            var response = MyavanaAdminApiClientFactory.Instance.GetAllProducts2().GetAwaiter().GetResult();
            return PartialView("_addHairProfile", response);
        }

        public List<ProductTypesList> GetProductTabData()
        {
            var response = MyavanaAdminApiClientFactory.Instance.GetProductTypes().GetAwaiter().GetResult();
            return response;
        }
        public List<ProductsModelList> GetStylingProducts()
        {
            var response = MyavanaAdminApiClientFactory.Instance.GetStylingProducts().GetAwaiter().GetResult();
            return response;
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(UpdateUserModel updateUserModel)
        {
            _httpClient.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);
   
            var loginUserId = Convert.ToInt32(Request.Cookies["UserId"]);
            updateUserModel.CreatedByUserId = loginUserId;
            var response = _httpClient.PostAsync("Account/updateCustomer", CreateHttpContent<UpdateUserModel>(updateUserModel)).Result;

            if (response.IsSuccessStatusCode)
            {
                    return Content("1");
            }
            else
            {
                var contentString = response.Content.ReadAsStringAsync().Result;
                dynamic responseObject = JsonConvert.DeserializeObject(contentString);
                if (responseObject.value != "")
                {
                    string message = responseObject.value;
                    if(message== "Email already exists.")
                    {
                        return Content("2");
                    }
                    else if(message== "Unable to update.")
                    {
                        return Content("3");
                    }
                    else if (message == "Payment exists.")
                    {
                        return Content("4");
                    }

                }
                return Content("-1");
            }
        }
        public async Task<IActionResult> DigitalAssessment(string userId)
        {
            var claimsIdentity1 = (ClaimsIdentity)User.Identity;

            CustomerAIResult hairProfileModel = new CustomerAIResult();

            Guid guidUserId = Guid.Parse(userId);
            hairProfileModel.UserId = guidUserId;

            QuestionaireModel questionaire = new QuestionaireModel();
            questionaire.Userid = userId;

            var result = await MyavanaAdminApiClientFactory.Instance.GetQuestionaireDetails(questionaire);   

            if (result.Data.IsExist == true)
            {
                if (result.Data.QuestionAnswerCount < 4)
                {
                    return RedirectToAction("start", "Questionnaire", new { id = userId });
                }

                ViewBag.IsExist = true;
                ViewBag.IsAlreadyAnalysed = 0;
                ViewBag.remainingAnalysis = 1;
               
                ViewBag.IsHHCPLimitExceed = 0;
                return View();

            }
            else
            {
                ViewBag.IsExist = false;
                return RedirectToAction("start", "Questionnaire", new { id = userId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveCustomerAIResultandImage( DigitalAssessmentModel digitalAssessmentModelParam)
        {
            try
            {
                if (digitalAssessmentModelParam != null)
                {
                    if (digitalAssessmentModelParam.ImageData != null)
                    {
                        var fileName = RandomNumbers() + ".jpg"; // file.FileName;

                        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "questionnaireFile")))
                        {
                            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "questionnaireFile"));
                        }
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "questionnaireFile", fileName);
                        var bytess = Convert.FromBase64String(digitalAssessmentModelParam.ImageData);
                        using (var imageFile = new FileStream(path, FileMode.Create))
                        {
                            imageFile.Write(bytess, 0, bytess.Length);
                            imageFile.Flush();
                        }

                        if (!string.IsNullOrEmpty(fileName))
                        {
                            using (var client = new HttpClient())
                            {
                                QuestionaireImage questionaire = new QuestionaireImage();
                                questionaire.UserId = digitalAssessmentModelParam.Userid;
                                questionaire.QuestionId = 22;
                                questionaire.DescriptiveAnswer = fileName;
                                var respAIResult = await MyavanaAdminApiClientFactory.Instance.SaveCustomerAIResultForMobile(digitalAssessmentModelParam);
                                var resp = await MyavanaAdminApiClientFactory.Instance.SaveSurveyImage(questionaire);                           
                            }
                        }
                    }
                    return Json(true);
                }
            }
            catch (Exception)
            {
                throw;
            }                
            return View();
        }


        public async Task<IActionResult> ScalpAnalysisResult(Guid userId)
        {
            HairScopeModel HairScopM = new HairScopeModel();

            HairScopM.AccessCode = userId;

            var respAIResult = await MyavanaAdminApiClientFactory.Instance.GetHairScopeResultData(HairScopM);
            HairScopM = respAIResult.Data;
            return View(HairScopM);
        }
        [HttpPost]
        public async Task<IActionResult> AutoGenerateHHCP(HairProfile hairProfile)
        {
            try
            {
                if (hairProfile != null)
                {
                    
                        var response = await MyavanaAdminApiClientFactory.Instance.AutoGenerateHHCP(hairProfile);
                        if (response.message == "Success")
                        {
                            return Json(response.Data);
                        }
                    else
                    {
                        return Json(false);
                    }
                }
                
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {

                return Json(false);
            }

        }
    }
}
