using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HubSpot.NET.Core.Requests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using MyAvana.CRM.Api.Contract;
using MyAvana.Framework.TokenService;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace MyAvana.CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnaireController : ControllerBase
    {
        private readonly IQuestionaire _questionaireService;
        private readonly IBaseBusiness _baseBusiness;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserEntity> _userManager;
        private IHttpContextAccessor _httpContextAccessor;
        private IHostingEnvironment _env;
        private readonly IConfiguration _configuration;

        public QuestionnaireController(IQuestionaire questionaireService, IBaseBusiness baseBusiness, IHostingEnvironment environment,
            ITokenService tokenService, UserManager<UserEntity> userManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _questionaireService = questionaireService;
            _baseBusiness = baseBusiness;
            _tokenService = tokenService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _env = environment;
            _configuration = configuration;
            var x = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier); //get in the constructor

        }

        [HttpPost("AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser()
        {
            var res = Request.HasFormContentType;
            string token = Request.Form["Token"];

            try
            {
                var stream = token;
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

                string email = tokenS.Claims.First(claim => claim.Type == "sub").Value;

                var result = await _questionaireService.AuthenticateUser(email);
                if (result.success)
                {
                    string id = (((Microsoft.AspNetCore.Identity.IdentityUser<System.Guid>)result.result.Value).Id).ToString();
                    return Ok(result);

                }
                return BadRequest(new JsonResult(result.error) { StatusCode = (int)HttpStatusCode.BadRequest });
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        [HttpPost("SaveSurvey")]
        public JObject SaveSurvey(IEnumerable<Questionaire> questionaires)
        {
            IEnumerable<Questionaire> result = _questionaireService.SaveSurvey(questionaires);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        } 
        
        [HttpPost("SaveSurveyImage")]
        public JObject SaveSurveyImage(Questionaire questionaire)
        {
            Questionaire result = _questionaireService.SaveSurveyImage(questionaire);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("SaveSurveyImageAdmin")]
        public JObject SaveSurveyImageAdmin(Questionaire questionaire)
        {
            Questionaire result = _questionaireService.SaveSurveyImageAdmin(questionaire);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("ChangeCustomerType")]
        public JObject ChangeCustomerType(UserEntity userModel)
        {
            UserEntity result = _questionaireService.ChangeCustomerType(userModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("ActivateCustomer")]
        public JObject ActivateCustomer(UserEntity userModel)
        {
            UserEntity result = _questionaireService.ActivateCustomer(userModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("SaveSurveyAdmin")]
        public JObject SaveSurveyAdmin(IEnumerable<Questionaire> questionaires)
        {
            IEnumerable<Questionaire> result = _questionaireService.SaveSurveyAdmin(questionaires);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }


        [HttpGet("GetQuestionnaireAdmin")]
        public async Task<JObject> GetQuestionnaire(int start, int length, int userId)
        {
            List<QuestionAnswerModel> result = await _questionaireService.GetQuestionnaire(start,length,userId);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetQuestionnaireForCustomer")]
        public async Task<JObject> GetQuestionnaireForCustomer(string userId)
        {
            List<QuestionAnswerModel> result = await _questionaireService.GetQuestionnaireForCustomer(userId);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetQuestionnaireAbsenceUserList")]
        public async Task<JObject> GetQuestionnaireAbsenceUserList(QuestionnaireAbsenceModel questionnaireAbsenceModel)
        {
            List<QuestionAnswerModel> result = await _questionaireService.GetQuestionnaireAbsenceUserList(questionnaireAbsenceModel);
            questionnaireAbsenceModel.QuestionAnswerModels = result;
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", questionnaireAbsenceModel);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetQuestionnaireCustomer")]
        public async Task<JObject> GetQuestionnaireCustomer(string id, int QA)
        {
            QuestionAnswerModel result = await _questionaireService.GetQuestionnaireCustomer(id,QA);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpGet("GetQuestionnaireCustomerAll")]
        public async Task<JObject> GetQuestionnaireCustomerAll(string id)
        {
            QuestionAnswerModel result = await _questionaireService.GetQuestionnaireCustomerAll(id);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpGet("GetCustomerMessagesList")]
        public async Task<JObject> GetCustomerMessagesList(string id)
        {
            List<CustomerMessageList> result = await _questionaireService.GetCustomerMessagesList(new Guid(id));
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpGet("GetCustomerTypeHistory")]
        public async Task<JObject> GetCustomerTypeHistory(string id)
        {
            List<CustomerTypeHistoryModel> result = await _questionaireService.GetCustomerTypeHistory(new Guid(id));
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("GetQuestionnaireCustomerList")]
        //public IActionResult GetQuestionnaireCustomerList()
        //{
        //  List<QuestionnaireCustomerList> result = _questionaireService.GetQuestionnaireCustomerList();
        //    if (result != null)
        //        return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

        //    return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        //}

        public async Task<JObject> GetQuestionnaireCustomerList(SearchCustomerResponse searchCustomerResponse)
        {
            SearchCustomerResponse result = await _questionaireService.GetQuestionnaireCustomerList(searchCustomerResponse);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }



        [HttpPost]
        [Route("DeleteQuest")]
        public JObject DeleteQuest(QuestModel quest)
        {
            bool result = _questionaireService.DeleteQuest(quest);
            if (result)
                return _baseBusiness.AddDataOnJson("Success", "1", quest);
            else
                return _baseBusiness.AddDataOnJson("Data not Found", "0", string.Empty);
        }

        [HttpGet("GetFilledSurvey")]
        public JObject GetFilledSurvey()
        {
            List<Questionaire> result = _questionaireService.GetFilledSurvey(HttpContext.User);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpGet("GetQuestionsForGraph")]
        public JObject GetQuestionsForGraph(string id)
        {
            List<QuestionGraph> result = _questionaireService.GetQuestionsForGraph(id);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost]
        [Route("SaveCustomerMessage")]
        public JObject SaveCustomerMessage([FromForm] CustomerMessageModel customerMessageModel)
        {

            CustomerMessageModel result = _questionaireService.SaveCustomerMessage(customerMessageModel);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);

            return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpGet("GetCustomerEmailTemplate")]
        public JObject GetCustomerEmailTemplate()
        {
            EmailTemplate result = _questionaireService.GetCustomerEmailTemplate();
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }

        [HttpPost("SaveSurveyImagefromMobile")]
        public JObject SaveSurveyImagefromMobile([FromForm] QuestionaireImageModel questionImageModel)
        {
            Questionaire questionaire = new Questionaire();
            string WebApiUrl = _configuration["WebApiUrl"];

            if (questionImageModel.File.Length > 0)
            {

                string fileName = questionImageModel.File.FileName;
                const string UPLOAD_FOLDER = "questionnaireFile";
                if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_FOLDER, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    questionImageModel.File.CopyTo(stream);
                    questionaire.UserId = questionImageModel.userId;
                    questionaire.DescriptiveAnswer = WebApiUrl+"/Questionnaire/GetImage?imageName=" + fileName;
                    Questionaire result = _questionaireService.SaveSurveyImagefromMobile(questionaire);
                    if (result != null)
                        return _baseBusiness.AddDataOnJson("Success", "1", result);
                    else
                        return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
                }
            }
            else
            {
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
            }   
            
        }
        [HttpGet("GetImage")]
        public IActionResult GetImage(string imageName)
        {
            if (string.IsNullOrEmpty(imageName))
            {
                return BadRequest("Image name cannot be empty.");
            }
            var imagePath = Path.Combine(_env.ContentRootPath, "questionnaireFile", imageName);

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound();
            }

            return PhysicalFile(imagePath, "image/jpeg");
        }

        [HttpGet("GetDailyRoutineWeb")]
        public JObject GetDailyRoutineWeb(string userId, string trackTime)
        {
            DailyRoutineTracker result = _questionaireService.GetDailyRoutineWeb(userId, trackTime);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Hairstyle for this User doesn't exist.", "0", result);

        }
        [HttpPost("SaveSurveyImagefromMobileForHHCP")]
        public  JObject SaveSurveyImagefromMobileForHHCP([FromForm] QuestionaireImageModel questionImageModel)
        {
            Questionaire questionaire = new Questionaire();
            string WebApiUrl = _configuration["WebApiUrl"];

            if (questionImageModel.File.Length > 0)
            {

                string fileName = questionImageModel.File.FileName;
                const string UPLOAD_FOLDER = "questionnaireFile";
                if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER)))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
                }
                var path = Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_FOLDER, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    questionImageModel.File.CopyTo(stream);
                    questionaire.UserId = questionImageModel.userId;
                    questionaire.DescriptiveAnswer = WebApiUrl + "/Questionnaire/GetImage?imageName=" + fileName;
                    Questionaire result = _questionaireService.SaveSurveyImagefromMobileForHHCP(questionaire);
                    if (result != null)
                        return _baseBusiness.AddDataOnJson("Success", "1", result);
                    else
                        return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
                }
            }
            else
            {
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
            }

        }

        [HttpGet("GetProfileImageCustomer")]
        public JObject GetProfileImageCustomer(string userId)
        {
            UserProfileImageModel result = _questionaireService.GetProfileImageCustomer(userId);
            if (result != null)
                return _baseBusiness.AddDataOnJson("Success", "1", result);
            else
                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        }
        [HttpGet("GetAnalystList")]
        public IActionResult GetAnalystList()
        {
            List<HairProfileAnayst> result = _questionaireService.GetAnalystList();
            if (result != null)
                return Ok(new JsonResult(result) { StatusCode = (int)HttpStatusCode.OK });

            return BadRequest(new JsonResult("") { StatusCode = (int)HttpStatusCode.BadRequest });
        }
        //[HttpPost("SaveCompleteSurveyFromMobile")]
        //public JObject SaveCompleteSurveyFromMobile([FromForm] IEnumerable<Questionaire> questionaires, [FromForm] QuestionaireImageModel questionImageModel)
        //{
        //    Questionaire questionaire = new Questionaire();
        //    string WebApiUrl = _configuration["WebApiUrl"];

        //    if (questionImageModel.File.Length > 0)
        //    {

        //        string fileName = questionImageModel.File.FileName;
        //        const string UPLOAD_FOLDER = "questionnaireFile";
        //        if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER)))
        //        {
        //            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, UPLOAD_FOLDER));
        //        }
        //        var path = Path.Combine(Directory.GetCurrentDirectory(), UPLOAD_FOLDER, fileName);

        //        using (var stream = new FileStream(path, FileMode.Create))
        //        {
        //            questionImageModel.File.CopyToAsync(stream);
        //            questionaire.UserId = questionImageModel.userId;
        //            questionaire.QuestionId = 22;
        //            questionaire.DescriptiveAnswer = WebApiUrl + "/Questionnaire/GetImage?imageName=" + fileName;

        //            var updatedQuestionaires = questionaires.Concat(new[] { questionaire });
        //            IEnumerable<Questionaire> result = _questionaireService.SaveCompleteSurveyFromMobile(updatedQuestionaires);
        //            if (result != null)
        //                return _baseBusiness.AddDataOnJson("Success", "1", result);
        //            else
        //                return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        //        }
        //    }
        //    else
        //    {
        //        return _baseBusiness.AddDataOnJson("Failed", "0", string.Empty);
        //    }
        //}
    }
}
