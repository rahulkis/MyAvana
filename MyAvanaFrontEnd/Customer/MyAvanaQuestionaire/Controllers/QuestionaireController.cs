using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using MyAvanaQuestionaire.Factory;
using MyAvanaQuestionaire.Models;
using MyAvanaQuestionaire.Utility;
using MyAvanaQuestionaireModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace MyAvanaQuestionaire.Controllers
{
    [Authorize(AuthenticationSchemes = "CustomerCookies")]

    public class QuestionaireController : Controller
    {
        private const string UPLOAD_FOLDER = "wwwroot/imageUpload";

        private readonly HttpClient _httpClient;
        private readonly IOptions<AppSettingsModel> config;
        private Uri BaseEndpoint;
        public QuestionaireController(IOptions<AppSettingsModel> config)
        {
            this.config = config;
            _httpClient = new HttpClient();
            BaseEndpoint = new Uri(config.Value.WebApiBaseUrl);
            ApplicationSettings.WebApiUrl = config.Value.WebApiBaseUrl;
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

        public IActionResult start(string id)
        {
            if (id != null)
            {
                ViewBag.token = id;
            }
            else
            {
                var claimsIdentity1 = (ClaimsIdentity)User.Identity;
                string userId = (claimsIdentity1.Claims).Select(x => x.Value).FirstOrDefault();
                var requestUrl = _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "Questionnaire/GetQuestionnaireCustomer?id=" + userId).Result;

                var data = requestUrl.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                dynamic result = JObject.Parse(data);
                QuestionAnswerModel questionaire = JsonConvert.DeserializeObject<QuestionAnswerModel>(Convert.ToString(result.data));
                ViewBag.Token = userId;
                if (questionaire.questionModel != null && questionaire.questionModel.Count > 0)
                {
                    var QA = questionaire.questionModel.FirstOrDefault().AnswerList.ToList().FirstOrDefault().QA;
                    return this.RedirectToAction
                   ("Questionaire", new { userId = userId, id = "update", qa = QA });
                }
            }
            return View();
        }
        public IActionResult Questionaire(string id, string userId)
        {
            ViewBag.Token = userId;
            ViewBag.Check = id;
            return View();
        }
        public async Task<IActionResult> EditQuestionaire(string id, string qa, string userId)
        {
            ViewBag.Token = userId;
            ViewBag.Check = id;
            if (!string.IsNullOrEmpty(qa))
            {
                var claimsIdentity1 = (ClaimsIdentity)User.Identity;

                MyAvanaQuestionaireModel.QuestionAnswerModel questionaireModel = new MyAvanaQuestionaireModel.QuestionAnswerModel();
                questionaireModel.UserId = userId;
                questionaireModel.QA = Convert.ToInt32(qa);

                var resp = await MyavanaCustomerApiClientFactory.Instance.GetCustomerQuestionaireDetails(questionaireModel);
                return View(resp.Data);
            }
            return View();
        }

        public async Task<QuestionAnswerModel> GetUserQuestionaire()
        {
            var claimsIdentity1 = (ClaimsIdentity)User.Identity;
            string userId = (claimsIdentity1.Claims).Select(x => x.Value).FirstOrDefault();

            var requestUrl = _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "Questionnaire/GetQuestionnaireCustomer?id=" + userId).Result;

            var data = await requestUrl.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(data);
            QuestionAnswerModel questionaire = JsonConvert.DeserializeObject<QuestionAnswerModel>(Convert.ToString(result.data));
            //foreach (var item in questionaire.questionModel)
            //{
            //    if (item.AnswerList.Count() == 2)
            //    {
            //        item.AnswerList = item.AnswerList.Where(x => x.QA == 2).ToList();
            //    }
            //    else
            //    {
            //        item.AnswerList = item.AnswerList.Where(x => x.QA == 1).ToList();
            //    }
            //}
            return questionaire;

        }

        public IActionResult update()
        {
            var claimsIdentity1 = (ClaimsIdentity)User.Identity;
            string userId = (claimsIdentity1.Claims).Select(x => x.Value).FirstOrDefault();
            return RedirectToRoute(new { controller = "Questionaire", action = "Questionaire", id = userId });
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

        public class QuestionaireModel
        {
            public string questionaire { get; set; }
            public IFormFile File { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> SaveQuestionaire([FromBody] IEnumerable<Questionaire> questionaire)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = BaseEndpoint;
                    if (questionaire.Count() != 0)
                    {
                        questionaire.FirstOrDefault().UserId = HttpContext.Session.GetString("id");
                        var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/SaveSurvey"));
                        addHeaders();
                        var response = await client.PostAsync(requestUrl.ToString(), CreateHttpContent<IEnumerable<Questionaire>>(questionaire));

                        if (response.StatusCode.ToString() == "OK")
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
        public async Task<IActionResult> EditQuestionaire([FromBody] IEnumerable<Questionaire> questionaire)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = BaseEndpoint;
                    if (questionaire.Count() != 0)
                    {
                        questionaire.FirstOrDefault().UserId = HttpContext.Session.GetString("id");
                        var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/EditSurvey"));
                        addHeaders();
                        var response = await client.PostAsync(requestUrl.ToString(), CreateHttpContent<IEnumerable<Questionaire>>(questionaire));

                        if (response.StatusCode.ToString() == "OK")
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
        public async Task<IActionResult> SaveImage(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    string fileName = RandomNumbers() + ".jpg"; // file.FileName;
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

        QuestionAnswerModel globalQuestionaire = new QuestionAnswerModel();
        public IActionResult RefreshQuestionaire(string typeOfQA)
        {
            QuestionAnswerModel questionaire = HttpContext.Session.GetObjectFromJson<QuestionAnswerModel>("questionaires");
            List<SelectListItem> qas = new List<SelectListItem>();
            var ddList = questionaire.questionModel.Select(x => x.QAType).Distinct().ToList();
            //int index = 0;
            foreach (var dd in ddList)
            {
                //index =Convert.ToInt32(dd);
                var qADate = questionaire.questionModel.SelectMany(x => x.AnswerList).FirstOrDefault(x => x.QA == int.Parse(dd))?.QAdate;
                if (typeOfQA == dd.ToString())
                    qas.Add(new SelectListItem() { Text = "QA" + dd + " - " + qADate.Value.ToString("MMM dd,yyyy | HH:mm"), Value = dd.ToString(), Selected = true });
                else
                    qas.Add(new SelectListItem() { Text = "QA" + dd + " - " + qADate.Value.ToString("MMM dd,yyyy | HH:mm"), Value = dd.ToString() });
                // index++;
            }

            ViewBag.qas = qas;
            questionaire.questionModel = questionaire.questionModel.Where(x => x.QAType == typeOfQA).ToList();
            //return View(questionaire);
            return View("Questionnaire", questionaire);
        }
        
        //public async Task<IActionResult> Questionnaire(string qa)
        //{
        //    var claimsIdentity1 = (ClaimsIdentity)User.Identity;
        //    string userId = (claimsIdentity1.Claims).Select(x => x.Value).LastOrDefault();

        //    MyAvanaQuestionaireModel.QuestionaireModel questionaireModel = new MyAvanaQuestionaireModel.QuestionaireModel();
        //    questionaireModel.Userid = userId;

        //    var resp = await MyavanaCustomerApiClientFactory.Instance.GetQuestionaireDetails(questionaireModel);
        //    if (resp.Data.IsExist == true)
        //    {

        //        ViewBag.IsExist = true;
        //    }
        //    else
        //    {
        //        ViewBag.IsExist = false;
        //        //return RedirectToAction("start", "Questionaire", new { id = userId });
        //    }
        //    ViewBag.UserId = userId.ToString();
        //    var requestUrl = _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "Questionnaire/GetQuestionnaireCustomer?id=" + userId).Result;

        //    var data = await requestUrl.Content.ReadAsStringAsync();
        //    dynamic result = JObject.Parse(data);
        //    QuestionAnswerModel questionaire = JsonConvert.DeserializeObject<QuestionAnswerModel>(Convert.ToString(result.data));


        //    List<SelectListItem> qas = new List<SelectListItem>();

        //    List<QuestionModels> questionAnswerModels = new List<QuestionModels>();
        //    foreach (var quest in questionaire.questionModel)
        //    {
        //        int count = quest.AnswerList.Select(x => x.QA).Distinct().Count();
        //        for (int i = 1; i <= count; i++)
        //        {
        //            QuestionModels questionModels = new QuestionModels();
        //            questionModels.SerialNo = quest.SerialNo;
        //            questionModels.QuestionId = quest.QuestionId;
        //            questionModels.Question = quest.Question;
        //            questionModels.QAType = i.ToString();
        //            questionModels.QA = quest.AnswerList.Select(x => x.QA).Distinct().FirstOrDefault();
        //            questionModels.AnswerList = quest.AnswerList.Where(x => x.QA == i).ToList();
        //            questionAnswerModels.Add(questionModels);
        //        }

        //    }
        //    //questionaire.questionModel = questionAnswerModels;


        //    var ddList = questionAnswerModels.Select(x => x.QA).Distinct().ToList();
        //    int index = 0;
        //    foreach (var dd in ddList)
        //    {
        //        index++;
        //        if (index == 1)
        //            qas.Add(new SelectListItem() { Text = "QA" + dd, Value = dd.ToString(), Selected = true });
        //        else
        //            qas.Add(new SelectListItem() { Text = "QA" + dd, Value = dd.ToString() });
        //    }

        //    ViewBag.qas = qas;
        //    HttpContext.Session.SetObjectAsJson("questionaires", questionaire);
        //    globalQuestionaire = questionaire;
        //    //questionaire.questionModel = questionAnswerModels.Where(x => x.QAType == index.ToString()).ToList();
        //    return View(questionaire);
        //}
        public async Task<IActionResult> Questionnaire(string qa)
        {
            var claimsIdentity1 = (ClaimsIdentity)User.Identity;
            string userId = (claimsIdentity1.Claims).Select(x => x.Value).LastOrDefault();

            MyAvanaQuestionaireModel.QuestionaireModel questionaireModel = new MyAvanaQuestionaireModel.QuestionaireModel();
            questionaireModel.Userid = userId;

            var resp = await MyavanaCustomerApiClientFactory.Instance.GetQuestionaireDetails(questionaireModel);
            if (resp.Data.IsExist == true)
            {

                ViewBag.IsExist = true;
            }
            else
            {
                ViewBag.IsExist = false;
                //return RedirectToAction("start", "Questionaire", new { id = userId });
            }
            ViewBag.UserId = userId.ToString();
            var requestUrl = _httpClient.GetAsync(ApplicationSettings.WebApiUrl + "Questionnaire/GetQuestionnaireCustomerAll?id=" + userId).Result;

            var data = await requestUrl.Content.ReadAsStringAsync();
            dynamic result = JObject.Parse(data);
            QuestionAnswerModel questionaire = JsonConvert.DeserializeObject<QuestionAnswerModel>(Convert.ToString(result.data));


            List<SelectListItem> qas = new List<SelectListItem>();

            List<QuestionModels> questionAnswerModels = new List<QuestionModels>();
            foreach (var quest in questionaire.questionModel)
            {
                var distinctQAValues = quest.AnswerList.Select(x => x.QA).Distinct().ToList();

                foreach (var distinctQA in distinctQAValues)
                {
                    QuestionModels questionModels = new QuestionModels();
                    questionModels.SerialNo = quest.SerialNo;
                    questionModels.QuestionId = quest.QuestionId;
                    questionModels.Question = quest.Question;
                    questionModels.QAType = distinctQA.ToString();
                    questionModels.QA = distinctQA;
                    questionModels.AnswerList = quest.AnswerList.Where(x => x.QA == distinctQA).ToList();
                    questionAnswerModels.Add(questionModels);
                }
               

            }
            questionaire.questionModel = questionAnswerModels;


            var ddList = questionAnswerModels.Select(x => x.QA).Distinct().ToList();           
            int index = 0;
            //int maxdd = (int)ddList.Max();
            int maxdd = ddList != null && ddList.Any() ? (int)ddList.Max() : 0;
            foreach (var dd in ddList)
            {
                //index++;
                var qADate = questionAnswerModels.SelectMany(x => x.AnswerList).FirstOrDefault(x => x.QA == dd)?.QAdate;
                if (dd == maxdd)
                {
                    qas.Add(new SelectListItem() { Text = "QA" + dd + " - " + qADate.Value.ToString("MMM dd,yyyy | HH:mm"), Value = dd.ToString(), Selected = true });

                }
                else
                    qas.Add(new SelectListItem() { Text = "QA" + dd + " - " + qADate.Value.ToString("MMM dd,yyyy | HH:mm"), Value = dd.ToString() });
            }

            ViewBag.qas = qas;
            HttpContext.Session.SetObjectAsJson("questionaires", questionaire);
            globalQuestionaire = questionaire;
            questionaire.questionModel = questionAnswerModels.Where(x => x.QAType == maxdd.ToString()).ToList();
            return View(questionaire);
        }
        [HttpGet]
        public async Task<bool> Logout()
        {
            Response.Cookies.Delete("CustomerCookies");
            return true;
        }

        #region Digital Assessment
        public async Task<IActionResult> DigitalAssessment(string userId)
        {
            var claimsIdentity1 = (ClaimsIdentity)User.Identity;
            string usrId = (claimsIdentity1.Claims).Select(x => x.Value).FirstOrDefault();

            HairProfileCustomerModel hairProfileModel = new HairProfileCustomerModel();
            hairProfileModel.UserId = usrId;

            MyAvanaQuestionaireModel.QuestionaireModel questionaire = new MyAvanaQuestionaireModel.QuestionaireModel();
            questionaire.Userid = usrId;

            var result = await MyavanaCustomerApiClientFactory.Instance.GetQuestionaireDetails(questionaire);
           // int subscriptionTypeValue = Convert.ToInt32(HttpContext.Session.GetString("SubscriptionType"));
            
            if (result.Data.IsExist == true)
            {
                if (result.Data.QuestionAnswerCount < 4)
                {
                    return RedirectToAction("start", "Questionaire", new { id = usrId });
                }

                //customers will be allowed to do Hair Analysis till paymentId is null.
                ViewBag.PaymentId = result.Data.PaymentId;
                
                
                ViewBag.IsExist = true;
                ViewBag.IsAlreadyAnalysed = 0;
                ViewBag.remainingAnalysis = 1;
                //if (subscriptionTypeValue == 2 && IsHairAIAllowed)
                //{
                //    ViewBag.remainingAnalysis = 1;
                //}
                ViewBag.IsHHCPLimitExceed = 0;
               
                if (hairProfileModel != null)
                {
                    var response = await MyavanaCustomerApiClientFactory.Instance.GetHairProfileCustomer(hairProfileModel);
                    if (response.Data != null && !String.IsNullOrEmpty(response.Data.AIResultNew))
                    {
                        if (response.Data.CountAIResults > 9)
                        {
                            ViewBag.IsHHCPLimitExceed = 1;
                            return View();
                        }
                        ViewBag.IsAlreadyAnalysed = 1;
                       
                    }
                }
                if (result.Data.PaymentId==null)
                {
                    TempData["ExpiredMessage"] =result.Data.ExpiredMessage ;
                    return RedirectToAction("HairAI", "HairProfile");
                }
            }
            else
            {
                ViewBag.IsExist = false;
                return RedirectToAction("start", "Questionaire", new { id = usrId });
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Capture(string name)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {

                                string fileName = RandomNumbers() + ".jpg"; // file.FileName;
                                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "questionnaireFile")))
                                {
                                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "questionnaireFile"));
                                }
                                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "questionnaireFile", fileName);
                                using (var stream = new FileStream(path, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }
                                if (!string.IsNullOrEmpty(fileName))
                                {
                                    using (var client = new HttpClient())
                                    {
                                        client.BaseAddress = BaseEndpoint;
                                        QuestionaireImage questionaire = new QuestionaireImage();
                                        questionaire.UserId = HttpContext.Session.GetString("id");
                                        questionaire.QuestionId = 22;
                                        questionaire.DescriptiveAnswer = fileName;

                                        var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/SaveSurveyImage"));
                                        addHeaders();
                                        var response = await client.PostAsync(requestUrl.ToString(), CreateHttpContent<QuestionaireImage>(questionaire));

                                        if (response.StatusCode.ToString() == "OK")
                                            return Content("1");
                                        else
                                            return Content("0");
                                    }
                                }
                                return Content(fileName);
                            }
                        }
                    }
                    return Content("1");
                }
                else
                {
                    return Content("0");
                }
            }
            catch (Exception ex)
            {

                return Json(false);
            }

        }

        [HttpPost]
        public async Task<IActionResult> SaveDigitalAssessment(DigitalAssessmenRequesttModel digitalAssessmenRequesttModel)
        {
            try
            {
                if (digitalAssessmenRequesttModel != null)
                {
                    if (digitalAssessmenRequesttModel.ImageData != null)
                    {
                        var fileName = RandomNumbers() + ".jpg"; // file.FileName;

                        if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "questionnaireFile")))
                        {
                            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "questionnaireFile"));
                        }
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "questionnaireFile", fileName);
                        var bytess = Convert.FromBase64String(digitalAssessmenRequesttModel.ImageData);
                        using (var imageFile = new FileStream(path, FileMode.Create))
                        {
                            imageFile.Write(bytess, 0, bytess.Length);
                            imageFile.Flush();
                        }

                        if (!string.IsNullOrEmpty(fileName))
                        {
                            using (var client = new HttpClient())
                            {
                                client.BaseAddress = BaseEndpoint;
                                QuestionaireImage questionaire = new QuestionaireImage();
                                questionaire.UserId = HttpContext.Session.GetString("id");
                                questionaire.QuestionId = 22;
                                questionaire.DescriptiveAnswer = fileName;

                                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/SaveSurveyImage"));
                                addHeaders();
                                var response =  client.PostAsync(requestUrl.ToString(), CreateHttpContent<QuestionaireImage>(questionaire)).GetAwaiter().GetResult();

                            }
                        }
                    }

                    var claimsIdentity1 = (ClaimsIdentity)User.Identity;
                    string userId = (claimsIdentity1.Claims).Select(x => x.Value).FirstOrDefault();

                    DigitalAssessmentModel digitalAssessmentModel = new DigitalAssessmentModel();
                    digitalAssessmentModel.AIResult = JsonConvert.SerializeObject(digitalAssessmenRequesttModel.AIResult).ToString();
                    //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(digitalAssessmentModel.AIResult);
                    digitalAssessmentModel.HairType = digitalAssessmenRequesttModel.HairType;
                    digitalAssessmentModel.Userid = userId;
                    digitalAssessmentModel.PaymentId = digitalAssessmenRequesttModel.PaymentId;
                    if (!string.IsNullOrEmpty(digitalAssessmenRequesttModel.AIResult))
                    {
                        var resp = await MyavanaCustomerApiClientFactory.Instance.CreateHHCPByDigitalAssessment(digitalAssessmentModel);

                    }
                    return Json(true);
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

        [HttpGet]
        public IActionResult GetSuggestions(string hairType)
        {
            hairType = "Straight";
            if (!string.IsNullOrEmpty(hairType))
            {
                _httpClient.BaseAddress = BaseEndpoint;
                var resp = _httpClient.GetAsync("Product/suggestions?hairType=" + hairType + "").Result;
                var data = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var result = JsonConvert.DeserializeObject(data);
                var value = JsonConvert.SerializeObject(((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)result).Last).Value);
                var jsonResp = JsonConvert.DeserializeObject(value);
                var jsonList = JsonConvert.SerializeObject(((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)jsonResp).Last).Value);
                var recommendedProd = JsonConvert.DeserializeObject<List<ProductsEntity>>(jsonList);
            }
            return Json(true);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ProfileImageUpload([FromBody] ProfileImagerequest imagerequest)
        {
            if (imagerequest.fileData.Length == 0)
                return BadRequest();

            var bytes = Convert.FromBase64String(imagerequest.fileData);

            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_FOLDER)))
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
            }
            string name = Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_FOLDER, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + ".jpg");
            string imageName = name.Substring(name.LastIndexOf("/") + 1);
            string path = imageName;
            imageName = path.Substring(path.LastIndexOf("\\") + 1);


            if (bytes.Length > 0)
            {
                using (var stream = new FileStream(name, FileMode.Create))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }
            }

            fileData file = new fileData();
            file.access_token = HttpContext.Session.GetString("token");
            file.ImageURL = imageName;

            var response = await MyavanaCustomerApiClientFactory.Instance.ProfileImageUpload(file);
            if (response.message == "Success")
            {
                return Ok(new JsonResult(new Dictionary<string, object>{
            { "access_token" , response.Data.access_token },
            {"user_name",response.Data.user_name },
            {"Email",response.Data.Email },
            {"Name",response.Data.Name },
            {"AccountNo",response.Data.AccountNo },
            {"TwoFactor",response.Data.TwoFactor },
            {"hairType",response.Data.HairType },
            {"imageURL",response.Data.ImageURL }
        })
                { StatusCode = (int)HttpStatusCode.OK });
            }
            return BadRequest(new JsonResult("Something went wrong!") { StatusCode = (int)HttpStatusCode.BadRequest });
        }


        #endregion
    }

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}