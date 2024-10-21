using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyAvana.CRM.Api.Contract;
using MyAvana.DAL.Auth;
using MyAvana.Framework.TokenService;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using MyAvanaApi.Models.ViewModels;
using NLog.Web.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Mail;
using ZendeskApi_v2.Models.Requests;
using System.Web;
using MyAvanaApi.Contract;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace MyAvana.CRM.Api.Services
{
    public class QuestionaireService : IQuestionaire
    {
        private readonly AvanaContext _context;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserEntity> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly string _adminUrl;
        private readonly Logger.Contract.ILogger _logger;



        public QuestionaireService(AvanaContext avanaContext, ITokenService tokenService, UserManager<UserEntity> userManager, IEmailService emailService, IConfiguration configuration, Logger.Contract.ILogger logger)
        {
            _context = avanaContext;
            _tokenService = tokenService;
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
            _logger = logger;
            _adminUrl = configuration["AdminUrl"];
        }

        public UserEntity GetUserEmail(ClaimsPrincipal claims)
        {
            try
            {
                Claim claim = claims.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                if (claim.Value != "")
                {
                    UserEntity user = new UserEntity();
                    user.UserName = claim.Value;
                    return user;
                }
                return null;
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetUserEmail, Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public async Task<(JsonResult result, bool success, string error)> AuthenticateUser(String email)
        {
            try
            {
                //Claim claim = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                var user = await _userManager.FindByEmailAsync(email);
                UserEntityModel userEntity = new UserEntityModel();
                if (user != null)
                {
                    var loginUser = _context.Users.Where(s => s.Email.ToLower() == email.ToLower()).FirstOrDefault();

                    userEntity.Id = loginUser.Id;
                    userEntity.UserName = loginUser.UserName;
                    userEntity.NormalizedUserName = loginUser.NormalizedUserName;
                    userEntity.Email = loginUser.Email;
                    userEntity.NormalizedEmail = loginUser.NormalizedEmail;
                    userEntity.EmailConfirmed = loginUser.EmailConfirmed;
                    userEntity.PasswordHash = loginUser.PasswordHash;
                    userEntity.SecurityStamp = loginUser.SecurityStamp;
                    userEntity.PhoneNumber = loginUser.PhoneNumber;
                    userEntity.FirstName = loginUser.FirstName;
                    userEntity.LastName = loginUser.LastName;

                    return (new JsonResult(userEntity) { StatusCode = (int)HttpStatusCode.OK }, true, "");
                }
                return (new JsonResult("") { StatusCode = (int)HttpStatusCode.OK }, false, "");
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: AuthenticateUser, Email:" + email + ", Error: " + ex.Message, ex);
                return (new JsonResult("") { StatusCode = (int)HttpStatusCode.OK }, false, "");
            }
        }
        public IEnumerable<Questionaire> SaveSurvey1(IEnumerable<Questionaire> questionaires)
        {
            List<Questionaire> objQuestionaire = new List<Questionaire>();
            string userId = questionaires.Select(x => x.UserId).LastOrDefault();
            try
            {
                var questions1 = _context.Questionaires.Where(x => x.UserId == userId && x.QA == 2).ToList();
                if (questions1 != null && questions1.Count() != 0)
                {
                    _context.Questionaires.RemoveRange(questions1);
                    _context.SaveChanges();
                    foreach (var ques in questionaires)
                    {
                        if (ques.QuestionId == 22)
                        {
                            Questionaire questionaire = new Questionaire();
                            questionaire.QuestionId = ques.QuestionId;
                            questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                            //questionaire.DescriptiveAnswer = _configuration.GetSection("WebUrl").Value + "/questionnaireFile/" + ques.DescriptiveAnswer;
                            questionaire.DescriptiveAnswer = "https://customer.myavana.com/questionnaireFile/" + ques.DescriptiveAnswer;
                            questionaire.CreatedOn = DateTime.Now;
                            questionaire.IsActive = true;
                            questionaire.UserId = userId;
                            questionaire.QA = 2;
                            objQuestionaire.Add(questionaire);
                        }
                        else
                        {
                            Questionaire questionaire = new Questionaire();
                            questionaire.QuestionId = ques.QuestionId;
                            questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                            questionaire.DescriptiveAnswer = ques.DescriptiveAnswer;
                            questionaire.CreatedOn = DateTime.Now;
                            questionaire.IsActive = true;
                            questionaire.UserId = userId;
                            questionaire.QA = 2;

                            objQuestionaire.Add(questionaire);
                        }
                    }
                    _context.AddRange(objQuestionaire);

                    _context.SaveChanges();
                }
                else
                {
                    var questions = _context.Questionaires.Where(x => x.UserId == userId && x.QA == 1).ToList();
                    if (questions != null && questions.Count() != 0)
                    {

                        foreach (var ques in questionaires)
                        {
                            if (ques.QuestionId == 22)
                            {
                                Questionaire questionaire = new Questionaire();
                                questionaire.QuestionId = ques.QuestionId;
                                questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                                //questionaire.DescriptiveAnswer = _configuration.GetSection("WebUrl").Value + "/questionnaireFile/" + ques.DescriptiveAnswer;
                                questionaire.DescriptiveAnswer = "https://customer.myavana.com/questionnaireFile/" + ques.DescriptiveAnswer;
                                questionaire.CreatedOn = DateTime.Now;
                                questionaire.IsActive = true;
                                questionaire.UserId = userId;
                                questionaire.QA = 2;
                                objQuestionaire.Add(questionaire);
                            }
                            else
                            {
                                Questionaire questionaire = new Questionaire();
                                questionaire.QuestionId = ques.QuestionId;
                                questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                                questionaire.DescriptiveAnswer = ques.DescriptiveAnswer;
                                questionaire.CreatedOn = DateTime.Now;
                                questionaire.IsActive = true;
                                questionaire.UserId = userId;
                                questionaire.QA = 2;

                                objQuestionaire.Add(questionaire);
                            }
                        }
                        _context.AddRange(objQuestionaire);
                        _context.SaveChanges();
                    }

                    else
                    {
                        foreach (var ques in questionaires)
                        {
                            if (ques.QuestionId == 22)
                            {
                                Questionaire questionaire = new Questionaire();
                                questionaire.QuestionId = ques.QuestionId;
                                questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                                //questionaire.DescriptiveAnswer = _configuration.GetSection("WebUrl").Value + "/questionnaireFile/" + ques.DescriptiveAnswer;
                                questionaire.DescriptiveAnswer = "https://customer.myavana.com/questionnaireFile/" + ques.DescriptiveAnswer;
                                questionaire.CreatedOn = DateTime.Now;
                                questionaire.IsActive = true;
                                questionaire.UserId = userId;
                                questionaire.QA = 1;
                                objQuestionaire.Add(questionaire);
                            }
                            else
                            {
                                Questionaire questionaire = new Questionaire();
                                questionaire.QuestionId = ques.QuestionId;
                                questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                                questionaire.DescriptiveAnswer = ques.DescriptiveAnswer;
                                questionaire.CreatedOn = DateTime.Now;
                                questionaire.IsActive = true;
                                questionaire.UserId = userId;
                                questionaire.QA = 1;
                                objQuestionaire.Add(questionaire);
                            }
                        }
                        _context.AddRange(objQuestionaire);

                        _context.SaveChanges();
                    }
                }


            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveSurvey1, UserId:" + userId + ", Error: " + ex.Message, ex);
            }

            return questionaires;
        }
        public IEnumerable<Questionaire> SaveSurvey(IEnumerable<Questionaire> questionaires)
        {
            List<Questionaire> objQuestionaire = new List<Questionaire>();
            bool sendCompleteQAEmail = false;
            //int QaInput=3;//= objQuestionaire[30].QA;
            int QAvalEdit = questionaires.Where(x => x.QuestionId == 22).Select(q => q.QA).FirstOrDefault();

            if (QAvalEdit > 0)
            {
                EditSurvey(questionaires);
                sendCompleteQAEmail = true;
            }
            else
            {
                int QAval;
                string userId = questionaires.Select(x => x.UserId).LastOrDefault();
                var user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
                try
                {
                    var questions = _context.Questionaires.Where(x => x.UserId == userId).ToList();
                    if (questions == null || questions.Count() == 0)
                    {
                        QAval = 1;
                    }
                    else
                    {
                        QAval = questions.Select(x => x.QA).Distinct().Max();
                    }

                    foreach (var ques in questionaires)
                    {
                        if (ques.QuestionId == 22)
                        {
                            Questionaire questionaire = new Questionaire();
                            questionaire.QuestionId = ques.QuestionId;
                            questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                            questionaire.DescriptiveAnswer = "https://customer.myavana.com/questionnaireFile/" + ques.DescriptiveAnswer;
                            //questionaire.DescriptiveAnswer = _configuration.GetSection("WebUrl").Value +"/questionnaireFile/" + ques.DescriptiveAnswer;
                            questionaire.CreatedOn = DateTime.Now;
                            questionaire.IsActive = true;
                            questionaire.QA = QAval + 1;
                            questionaire.UserId = userId;

                            objQuestionaire.Add(questionaire);
                            sendCompleteQAEmail = true;
                        }
                        else
                        {
                            Questionaire questionaire = new Questionaire();
                            questionaire.QuestionId = ques.QuestionId;
                            questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                            questionaire.DescriptiveAnswer = ques.DescriptiveAnswer;
                            questionaire.CreatedOn = DateTime.Now;
                            questionaire.IsActive = true;
                            questionaire.QA = QAval + 1;
                            questionaire.UserId = userId;

                            objQuestionaire.Add(questionaire);
                        }
                    }
                    _context.AddRange(objQuestionaire);

                    _context.SaveChanges();

                    EmailInformation emailInformation = new EmailInformation
                    {
                        Code = user.Email
                    };
                    if (sendCompleteQAEmail == true)
                    {
                        var emailRes = _emailService.SendEmail("COMPQA", emailInformation);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Method: SaveSurvey, UserId:" + userId + ", Error: " + ex.Message, ex);
                }
            }
            return questionaires;
        }

        public IEnumerable<Questionaire> EditSurvey(IEnumerable<Questionaire> questionaires)
        {
            List<Questionaire> objQuestionaire = new List<Questionaire>();

            int QAval = questionaires.Where(x => x.QuestionId == 22).Select(q => q.QA).FirstOrDefault();
            string userId = questionaires.Select(x => x.UserId).LastOrDefault();
            try
            {
                var questions = _context.Questionaires.Where(x => x.UserId == userId && x.QA == QAval).ToList();

                _context.Questionaires.RemoveRange(questions);

                foreach (var ques in questionaires)
                {
                    if (ques.QuestionId == 22)
                    {
                        Questionaire questionaire = new Questionaire();
                        questionaire.QuestionId = ques.QuestionId;
                        questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                        //questionaire.DescriptiveAnswer = _configuration.GetSection("WebUrl").Value + "/questionnaireFile/" + ques.DescriptiveAnswer;
                        questionaire.DescriptiveAnswer = "https://customer.myavana.com/questionnaireFile/" + ques.DescriptiveAnswer;
                        questionaire.CreatedOn = DateTime.Now;
                        questionaire.IsActive = true;
                        questionaire.QA = QAval;
                        questionaire.UserId = userId;

                        objQuestionaire.Add(questionaire);
                    }
                    else
                    {
                        Questionaire questionaire = new Questionaire();
                        questionaire.QuestionId = ques.QuestionId;
                        questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                        questionaire.DescriptiveAnswer = ques.DescriptiveAnswer;
                        questionaire.CreatedOn = DateTime.Now;
                        questionaire.IsActive = true;
                        questionaire.QA = QAval;
                        questionaire.UserId = userId;

                        objQuestionaire.Add(questionaire);
                    }
                }
                _context.AddRange(objQuestionaire);

                _context.SaveChanges();


            }
            catch (Exception ex)
            {
                _logger.LogError("Method: EditSurvey, UserId:" + userId + ", Error: " + ex.Message, ex);
            }

            return questionaires;
        }

        public IEnumerable<Questionaire> SaveSurveyAdmin(IEnumerable<Questionaire> questionaires)
        {
            List<Questionaire> objQuestionaire = new List<Questionaire>();
            string userId = questionaires.Select(x => x.UserId).LastOrDefault();
            try
            {
                var questions = _context.Questionaires.Where(x => x.UserId == userId).ToList();
                if (questions != null && questions.Count() != 0)
                {
                    _context.Questionaires.RemoveRange(questions);
                    _context.SaveChanges();
                }
                foreach (var ques in questionaires)
                {
                    if (ques.QuestionId == 22)
                    {
                        Questionaire questionaire = new Questionaire();
                        questionaire.QuestionId = ques.QuestionId;
                        questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                        questionaire.DescriptiveAnswer = "http://admin.myavana.com/questionnaireFile/" + ques.DescriptiveAnswer;
                        questionaire.CreatedOn = DateTime.Now;
                        questionaire.IsActive = true;
                        questionaire.UserId = userId;

                        objQuestionaire.Add(questionaire);
                    }
                    else
                    {
                        Questionaire questionaire = new Questionaire();
                        questionaire.QuestionId = ques.QuestionId;
                        questionaire.AnswerId = ques.AnswerId == 0 ? null : ques.AnswerId;
                        questionaire.DescriptiveAnswer = ques.DescriptiveAnswer;
                        questionaire.CreatedOn = DateTime.Now;
                        questionaire.IsActive = true;
                        questionaire.UserId = userId;

                        objQuestionaire.Add(questionaire);
                    }

                }
                _context.AddRange(objQuestionaire);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveSurveyAdmin, UserId:" + userId + ", Error: " + ex.Message, ex);
            }

            return questionaires;
        }

        public async Task<List<QuestionAnswerModel>> GetQuestionnaireForCustomer(string userId)
        {
            List<QuestionAnswerModel> questionnaireModels = new List<QuestionAnswerModel>();
            try
            {
                string email = _context.Users.Where(x => x.Id.ToString() == userId).Select(x => x.Email).FirstOrDefault();
                var hairProfile = await _context.HairProfiles.Where(x => x.UserId == email).GroupBy(x => x.AttachedQA).Select(x => x.FirstOrDefault()).ToListAsync();
                var qstnrs = await _context.Questionaires.Where(x => x.IsActive == true && x.UserId == userId).GroupBy(x => new { x.UserId, x.QA }).Select(x => new { x.Key.UserId, x.Key.QA, cnt = x.Count() }).ToListAsync();
                questionnaireModels = (from quest in _context.Questionaires
                                       join usr in _context.Users
                                       on quest.UserId equals usr.Id.ToString()
                                       join custtype in _context.CustomerTypes
                                       on usr.CustomerTypeId equals custtype.CustomerTypeId
                                       //  join hp in hairProfile
                                       //   on usr.Email equals hp.UserId into g
                                       //  from a in g.DefaultIfEmpty()
                                       where quest.IsActive == true && usr.Id == new Guid(userId)
                                       select new QuestionAnswerModel()
                                       {
                                           UserId = quest.UserId,
                                           UserName = usr.FirstName + " " + usr.LastName,
                                           UserEmail = usr.Email,
                                           CustomerType = usr.CustomerType,
                                           CreatedOn = quest.CreatedOn,
                                           IsDraft = hairProfile.Where(h => h.AttachedQA == quest.QA).Count() > 0 ? hairProfile.Where(h => h.AttachedQA == quest.QA).FirstOrDefault().IsDrafted : false,
                                           CustomerTypeId = usr.CustomerTypeId,
                                           IsHHCPExist = hairProfile.Where(h => h.AttachedQA == quest.QA).Count() > 0 ? hairProfile.Where(h => h.AttachedQA == quest.QA).FirstOrDefault().IsActive : false,
                                           CustomerQAFrom = custtype.Description,
                                           QA = quest.QA + 1,
                                           QuestionCount = qstnrs.Where(y => y.UserId == quest.UserId && y.QA == quest.QA).FirstOrDefault().cnt
                                       }).GroupBy(x => x.QA).Select(g => g.FirstOrDefault()).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetQuestionnaireForCustomer, UserId:" + userId.ToString() + ", Error: " + ex.Message, ex);
            }
            return questionnaireModels;
        }

        public async Task<List<QuestionAnswerModel>> GetQuestionnaire(int start, int length, int userId)
        {
            List<QuestionAnswerModel> questionnaireModels = new List<QuestionAnswerModel>();
            try
            {
                this._context.Database.SetCommandTimeout(280);
                var hairProfile = await _context.HairProfiles.GroupBy(x => x.UserId).Select(x => x.LastOrDefault()).ToListAsync();
                var qstnrs = await _context.Questionaires.Where(x => x.IsActive == true).GroupBy(x => new { x.UserId, x.QA }).Select(x => new { x.Key.UserId, x.Key.QA, cnt = x.Count() }).ToListAsync();
                //var cnt = hairProfile.Count();

                var userType = _context.WebLogins.FirstOrDefault(x => x.UserId == userId).UserTypeId;
                if (userType == (int)UserTypeEnum.B2B)
                {
                    var salonIds = _context.SalonsOwners.Where(x => x.UserId == userId && x.IsActive == true).Select(x => x.SalonId).ToArray();
                    questionnaireModels = (from quest in _context.Questionaires
                                           join usr in _context.Users
                                           on quest.UserId equals usr.Id.ToString()
                                           join custtype in _context.CustomerTypes
                                           on usr.CustomerTypeId equals custtype.CustomerTypeId
                                           join hp in hairProfile
                                           on usr.Email equals hp.UserId into g
                                           from a in g.DefaultIfEmpty()
                                           where quest.IsActive == true && salonIds.Contains(usr.SalonId ?? 0)
                                           select new QuestionAnswerModel()
                                           {
                                               UserId = quest.UserId,
                                               UserName = usr.FirstName + " " + usr.LastName,
                                               UserEmail = usr.Email,
                                               CustomerType = usr.CustomerType,
                                               CreatedOn = quest.CreatedOn,
                                               IsDraft = a.IsDrafted,
                                               CustomerTypeId = usr.CustomerTypeId,
                                               IsHHCPExist = g.FirstOrDefault().IsActive,
                                               CustomerQAFrom = custtype.Description,
                                               QuestionCount = qstnrs.Where(y => y.UserId == quest.UserId && y.QA == quest.QA).FirstOrDefault().cnt
                                           }).GroupBy(x => x.UserId.ToLower()).Select(x => x.LastOrDefault()).ToList();
                }
                else
                {

                    questionnaireModels = (from quest in _context.Questionaires
                                           join usr in _context.Users
                                           on quest.UserId equals usr.Id.ToString()
                                           join custtype in _context.CustomerTypes
                                           on usr.CustomerTypeId equals custtype.CustomerTypeId
                                           join hp in hairProfile
                                           on usr.Email equals hp.UserId into g
                                           from a in g.DefaultIfEmpty()
                                           where quest.IsActive == true
                                           select new QuestionAnswerModel()
                                           {
                                               UserId = quest.UserId,
                                               UserName = usr.FirstName + " " + usr.LastName,
                                               UserEmail = usr.Email,
                                               CustomerType = usr.CustomerType,
                                               CreatedOn = quest.CreatedOn,
                                               IsDraft = a.IsDrafted,
                                               CustomerTypeId = usr.CustomerTypeId,
                                               IsHHCPExist = g.FirstOrDefault().IsActive,
                                               CustomerQAFrom = custtype.Description,
                                               QuestionCount = qstnrs.Where(y => y.UserId == quest.UserId && y.QA == quest.QA).FirstOrDefault().cnt
                                           }).GroupBy(x => x.UserId.ToLower()).Select(x => x.LastOrDefault()).ToList();

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetQuestionnaire, UserId:" + userId + ", Error: " + ex.Message, ex);
            }
            return questionnaireModels;
        }

        public async Task<List<QuestionAnswerModel>> GetQuestionnaireAbsenceUserList(QuestionnaireAbsenceModel questionnaireAbsenceModel)
        {
            List<QuestionAnswerModel> questionnaireModels = new List<QuestionAnswerModel>();
            try
            {
                List<Guid> netUserIds = new List<Guid>();
                List<string> userIds = _context.Questionaires.Where(z => z.IsActive == true).Select(x => x.UserId).Distinct().ToList();
                try
                {
                    foreach (string userId in userIds)
                    {
                        if (userId != null && userId != "")
                        {
                            Guid id = new Guid(userId);
                            netUserIds.Add(id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Method: GetQuestionnaireAbsenceUserList, Error: " + ex.Message, ex);

                }

                var users = _context.Users.Where(x => !netUserIds.Contains(x.Id)).ToList();
                if (questionnaireAbsenceModel != null)
                {
                    var userType = _context.WebLogins.FirstOrDefault(x => x.UserId == questionnaireAbsenceModel.userId)?.UserTypeId;
                    if (userType == (int)UserTypeEnum.B2B)
                    {
                        var salonIds = _context.SalonsOwners.FirstOrDefault(x => x.UserId == questionnaireAbsenceModel.userId && x.IsActive == true)?.SalonId;
                        if (salonIds > 0)
                        {
                            users = users.Where(x => x.SalonId == salonIds).ToList();
                        }
                    }
                }

                foreach (var user in users)
                {
                    QuestionAnswerModel questionAnswerModel = new QuestionAnswerModel();
                    questionAnswerModel.UserId = user.Id.ToString();
                    questionAnswerModel.UserName = user.FirstName + " " + user.LastName;
                    questionAnswerModel.UserEmail = user.UserName;
                    questionAnswerModel.CustomerType = user.CustomerType;
                    questionAnswerModel.CreatedOn = DateTime.Now;
                    questionnaireModels.Add(questionAnswerModel);
                }



                return questionnaireModels;
            }

            catch (Exception ex)
            {
                _logger.LogError("Method: GetQuestionnaireAbsenceUserList, Error: " + ex.Message, ex);
                return null;
            }
        }

        public string GetCustomerQAFrom(string userId)
        {
            List<QuestionAnswerModel> questionnaireModels = new List<QuestionAnswerModel>();
            QuestionAnswerModel questionAnswerModel = new QuestionAnswerModel();
            string QAFrom = "";
            try
            {
                int i = 0;
                int[] elements = new int[500];


                int latestQA = _context.Questionaires.Where(x => x.UserId == userId && x.IsActive == true).Max(x => x.QA);
                var QuestionList = _context.Questionaires.Include(x => x.Questions).Where(x => x.UserId == userId && x.IsActive == true && x.QA == latestQA).Select(x => x.Questions).ToList();

                foreach (var question in QuestionList)
                {
                    if (!elements.Contains(question.QuestionId))
                    {
                        elements[i] = question.QuestionId;
                        i++;
                    }

                }
                if (QuestionList.Count == 3)
                {
                    QAFrom = "Mobile SignUp";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetCustomerQAFrom, UserId:" + userId + ", Error: " + ex.Message, ex);
            }
            return QAFrom;
        }

        public async Task<QuestionAnswerModel> GetQuestionnaireCustomer(string userId,int QA)
        {
            List<QuestionAnswerModel> questionnaireModels = new List<QuestionAnswerModel>();
            QuestionAnswerModel questionAnswerModel = new QuestionAnswerModel();

            try
            {
                int i = 0;
                int[] elements = new int[500];

                var userName = await _userManager.FindByIdAsync(userId);
                questionAnswerModel.UserId = userId;
                questionAnswerModel.UserName = userName.FirstName;
                questionAnswerModel.UserEmail = userName.UserName;

                var QuestionList = _context.Questionaires.Include(x => x.Questions).Where(x => x.UserId == userId && x.IsActive == true && x.QA == QA).Select(x => x.Questions).ToList();
                List<QuestionModels> questionModels = new List<QuestionModels>();

                foreach (var question in QuestionList)
                {
                    if (!elements.Contains(question.QuestionId))
                    {
                        elements[i] = question.QuestionId;
                        i++;
                        if (question.QuestionId == 29)
                        {
                            string abc = "ww";
                        }
                        QuestionModels questions = new QuestionModels();
                        questions.QuestionId = question.QuestionId;
                        questions.Question = question.Question;
                        questions.SerialNo = question.SerialNo;

                        questions.AnswerList = (from m in _context.Questionaires
                                                join ans in _context.Answers
                                                on m.AnswerId equals ans.AnswerId into answer
                                                from qAns in answer.DefaultIfEmpty()
                                                where m.QuestionId == question.QuestionId
                                                && m.UserId == userId && m.QA == QA
                                                select new AnswerModel()
                                                {
                                                    QA = (m.QA),
                                                    QAdate = (m.CreatedOn),
                                                    AnswerId = qAns.AnswerId,
                                                    Answer = (m.AnswerId == null || m.AnswerId == 112) ? m.DescriptiveAnswer : qAns.Description
                                                }).ToList();
                        questionModels.Add(questions);
                    }

                }
                questionAnswerModel.questionModel = questionModels;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetQuestionnaireCustomer, UserId:" + userId + ", Error: " + ex.Message, ex);
            }
            return questionAnswerModel;
        }

        public async Task<QuestionAnswerModel> GetQuestionnaireCustomerAll(string userId)
        {
            List<QuestionAnswerModel> questionnaireModels = new List<QuestionAnswerModel>();
            QuestionAnswerModel questionAnswerModel = new QuestionAnswerModel();

            try
            {
                int i = 0;
                int[] elements = new int[500];

                var userName = await _userManager.FindByIdAsync(userId);
                questionAnswerModel.UserId = userId;
                questionAnswerModel.UserName = userName.FirstName;
                questionAnswerModel.UserEmail = userName.UserName;

                var QuestionList = _context.Questionaires.Include(x => x.Questions).Where(x => x.UserId == userId).Select(x => x.Questions).ToList();
                List<QuestionModels> questionModels = new List<QuestionModels>();

                foreach (var question in QuestionList)
                {
                    if (!elements.Contains(question.QuestionId))
                    {
                        elements[i] = question.QuestionId;
                        i++;
                        if (question.QuestionId == 29)
                        {
                            string abc = "ww";
                        }
                        QuestionModels questions = new QuestionModels();
                        questions.QuestionId = question.QuestionId;
                        questions.Question = question.Question;
                        questions.SerialNo = question.SerialNo;

                        questions.AnswerList = (from m in _context.Questionaires
                                                join ans in _context.Answers
                                                on m.AnswerId equals ans.AnswerId into answer
                                                from qAns in answer.DefaultIfEmpty()
                                                where m.QuestionId == question.QuestionId
                                                && m.UserId == userId //&& m.QA == latestQA
                                                select new AnswerModel()
                                                {
                                                    QA = (m.QA),
                                                    QAdate = (m.CreatedOn),
                                                    AnswerId = qAns.AnswerId,
                                                    Answer = (m.AnswerId == null || m.AnswerId == 112) ? m.DescriptiveAnswer : qAns.Description
                                                }).ToList();
                        questionModels.Add(questions);
                    }

                }
                questionAnswerModel.questionModel = questionModels;

                //questionnaireModels.Add(questionAnswerModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetQuestionnaireCustomerAll, UserId:" + userId + ", Error: " + ex.Message, ex);
            }
            return questionAnswerModel;
        }
        public async Task<QuestionAnswerModel> GetQuestionnaireCustomerDetails(QuestionAnswerModel quesModel)
        {
            List<QuestionAnswerModel> questionnaireModels = new List<QuestionAnswerModel>();
            QuestionAnswerModel questionAnswerModel = new QuestionAnswerModel();

            try
            {
                int i = 0;
                int[] elements = new int[50];

                var userName = await _userManager.FindByIdAsync(quesModel.UserId);
                questionAnswerModel.UserId = quesModel.UserId;
                questionAnswerModel.UserName = userName.FirstName;
                questionAnswerModel.UserEmail = userName.UserName;

                var QuestionList = _context.Questionaires.Include(x => x.Questions).Where(x => x.UserId == quesModel.UserId && x.IsActive == true && x.QA == quesModel.QA).Select(x => x.Questions).ToList();
                List<QuestionModels> questionModels = new List<QuestionModels>();

                foreach (var question in QuestionList)
                {
                    if (!elements.Contains(question.QuestionId))
                    {
                        elements[i] = question.QuestionId;
                        i++;
                        if (question.QuestionId == 29)
                        {
                            string abc = "ww";
                        }
                        QuestionModels questions = new QuestionModels();
                        questions.QuestionId = question.QuestionId;
                        questions.Question = question.Question;
                        questions.SerialNo = question.SerialNo;

                        questions.AnswerList = (from m in _context.Questionaires
                                                join ans in _context.Answers
                                                on m.AnswerId equals ans.AnswerId into answer
                                                from qAns in answer.DefaultIfEmpty()
                                                where m.QuestionId == question.QuestionId
                                                && m.UserId == quesModel.UserId
                                                select new AnswerModel()
                                                {
                                                    QA = (m.QA),
                                                    QAdate = (m.CreatedOn),
                                                    AnswerId = qAns.AnswerId,
                                                    Answer = (m.AnswerId == null || m.AnswerId == 112) ? m.DescriptiveAnswer : qAns.Description
                                                }).ToList();
                        questionModels.Add(questions);
                    }

                }
                questionAnswerModel.questionModel = questionModels;

                //questionnaireModels.Add(questionAnswerModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetQuestionnaireCustomerDetails, Error: " + ex.Message, ex);
            }
            return questionAnswerModel;
        }

        public List<QuestionnaireCustomerList> GetQuestionnaireCustomerList()
        {
            List<QuestionnaireCustomerList> questionnaireModels = new List<QuestionnaireCustomerList>();
            List<UserEntity> user = new List<UserEntity>();
            try
            {
                //List<string> userIds = _context.Questionaires.Select(x => x.UserId).Distinct().ToList();
                //user = _context.Users.Where(x => !userIds.Contains(x.Id.ToString())).ToList();       

                user = _context.Users.OrderByDescending(x => x.CreatedAt).ToList();
                foreach (var us in user)
                {
                    QuestionnaireCustomerList questionnaireCustomerList = new QuestionnaireCustomerList();

                    questionnaireCustomerList.UserId = us.Id.ToString();
                    questionnaireCustomerList.UserName = us.FirstName + " " + us.LastName;
                    questionnaireCustomerList.UserEmail = us.UserName;
                    questionnaireCustomerList.CreatedAt = us.CreatedAt;
                    questionnaireCustomerList.CustomerType = us.CustomerType;
                    questionnaireCustomerList.IsInfluencer = us.IsInfluencer;
                    var findUser = _context.Questionaires.FirstOrDefault(x => x.UserId == us.Id.ToString());
                    if (findUser != null)
                        questionnaireCustomerList.IsQuestionnaire = true;
                    else
                        questionnaireCustomerList.IsQuestionnaire = false;

                    questionnaireModels.Add(questionnaireCustomerList);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetQuestionnaireCustomerList, Error: " + ex.Message, ex);
            }
            return questionnaireModels;
        }

        public async Task<List<CustomerMessageList>> GetCustomerMessagesList(Guid userId)
        {
            List<CustomerMessageList> messagesModels = new List<CustomerMessageList>();
            try
            {
                messagesModels = (from m in _context.CustomerMessage
                                  where m.UserId == userId
                                  select new CustomerMessageList()
                                  {
                                      Subject = m.Subject,
                                      Message = m.Message,
                                      EmailAddress = m.EmailAddress,
                                      CreatedOn = m.CreatedOn,
                                      AttachmentFile = m.AttachmentFile
                                  }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetCustomerMessagesList, UserId:" + userId + ", Error: " + ex.Message, ex);
            }
            return messagesModels;
        }
        public async Task<List<CustomerTypeHistoryModel>> GetCustomerTypeHistory(Guid userId)
        {

            List<CustomerTypeHistoryModel> customerHistory = new List<CustomerTypeHistoryModel>();
            try
            {
                customerHistory = (from m in _context.CustomerTypeHistory
                                   where m.CustomerId == userId
                                   select new CustomerTypeHistoryModel()
                                   {
                                       OldCustomerType = m.OldCustomerType.Description,
                                       NewCustomerType = m.NewCustomerType.Description,
                                       UpdatedBy = m.UpdatedBy.UserEmail,
                                       CreatedOn = m.CreatedOn,
                                       Comment = m.Comment
                                   }).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetCustomerTypeHistory, UserId:" + userId + ", Error: " + ex.Message, ex);
            }
            return customerHistory;
        }
        public bool DeleteQuest(QuestModel quest)
        {
            if (quest.QA != 0)
            {
                quest.QA = quest.QA - 1;
            }
            Guid userId = _context.Users.Where(x => x.Email == quest.Email).Select(z => z.Id).FirstOrDefault();
            try
            {
              var objCode = _context.Questionaires.Where(x => x.UserId == userId.ToString() && x.QA == quest.QA).ToList();
                foreach (var result in objCode)
                {
                    result.IsActive = false;
                }
                var Hairprofile = _context.HairProfiles.Where(x => x.UserId == quest.Email && x.AttachedQA == quest.QA).ToList();
                foreach (var Ids in Hairprofile)
                {
                    Ids.AttachedQA = null;
                }
                _context.SaveChanges();
                return true;
            }

            catch (Exception Ex)
            {
                _logger.LogError("Method: DeleteQuest, UserId:" + userId + ", Error: " + Ex.Message, Ex);
                return false;
            }
        }

        public List<Questionaire> GetFilledSurvey(ClaimsPrincipal claim)
        {
            Questionaire customerQuestionnaire = new Questionaire();
            var usr = _tokenService.GetAccountNo(claim);

            if (usr != null)
            {
                try
                {
                    List<Questionaire> questionaires = _context.Questionaires.Where(x => x.UserId == usr.Id.ToString()).ToList();
                    return questionaires;
                }
                catch (Exception Ex)
                {
                    _logger.LogError("Method: GetFilledSurvey, Error: " + Ex.Message, Ex);
                    return null;
                }
            }
            return null;
        }


        public List<QuestionGraph> GetQuestionsForGraph(string userId)
        {
            try
            {
                var userType = _context.WebLogins.FirstOrDefault(x => x.UserId == Convert.ToInt32(userId)).UserTypeId;
                if (userType == (int)UserTypeEnum.B2B)
                {
                    var salonIds = _context.SalonsOwners.Where(x => x.UserId == Convert.ToInt32(userId)).Select(x => x.SalonId).ToArray();
                    var userIds = _context.Users.Where(u => salonIds.Any(x => x == u.SalonId)).Select(x => x.Id).ToArray();
                    var questionaires = _context.Questionaires.Include(x => x.Questions).Where(y => y.IsActive == true && !string.IsNullOrEmpty(y.UserId) && userIds.Any(u => u == new Guid(y.UserId))).OrderBy(x => x.QuestionId).ToList();
                    List<QuestionGraph> questionGraphs = new List<QuestionGraph>();
                    List<int> questQuestionIds = questionaires.Select(x => x.QuestionId).Distinct().ToList();
                    List<int> questionIds = new List<int>();
                    foreach (var questionId in questQuestionIds)
                    {
                        QuestionGraph questionGraph = new QuestionGraph();
                        questionIds.Add(questionId);
                        questionGraph.QuestionId = questionId;
                        questionGraph.Question = questionaires.Where(x => x.QuestionId == questionId).Select(q => q.Questions.Question).FirstOrDefault();
                        var answers = _context.Answers.Where(q => q.QuestionId == questionId).Select(x => new { AnswerId = x.AnswerId, Answer = x.Description }).ToList();

                        List<AnswerCount> answerCounts = new List<AnswerCount>();
                        foreach (var answer in answers)
                        {
                            if (answer.Answer != "Free response")
                            {
                                AnswerCount answerCount = new AnswerCount();
                                answerCount.AnswerId = answer.AnswerId;
                                answerCount.Answer = answer.Answer;
                                answerCount.Count = questionaires.Where(x => x.AnswerId == answer.AnswerId).Count();
                                answerCounts.Add(answerCount);
                            }
                        }
                        if (answerCounts.Count() > 0)
                        {
                            questionGraph.AnswerCounts = answerCounts;
                            questionGraphs.Add(questionGraph);
                        }
                    }

                    return questionGraphs;
                }
                else
                {
                    var questionaires = _context.Questionaires.Include(x => x.Questions).Where(y => y.IsActive == true).OrderBy(x => x.QuestionId).ToList();
                    List<QuestionGraph> questionGraphs = new List<QuestionGraph>();
                    List<int> questQuestionIds = questionaires.Select(x => x.QuestionId).Distinct().ToList();
                    List<int> questionIds = new List<int>();
                    foreach (var questionId in questQuestionIds)
                    {
                        QuestionGraph questionGraph = new QuestionGraph();
                        questionIds.Add(questionId);
                        questionGraph.QuestionId = questionId;
                        questionGraph.Question = questionaires.Where(x => x.QuestionId == questionId).Select(q => q.Questions.Question).FirstOrDefault();
                        var answers = _context.Answers.Where(q => q.QuestionId == questionId).Select(x => new { AnswerId = x.AnswerId, Answer = x.Description }).ToList();

                        List<AnswerCount> answerCounts = new List<AnswerCount>();
                        foreach (var answer in answers)
                        {
                            if (answer.Answer != "Free response")
                            {
                                AnswerCount answerCount = new AnswerCount();
                                answerCount.AnswerId = answer.AnswerId;
                                answerCount.Answer = answer.Answer;
                                answerCount.Count = questionaires.Where(x => x.AnswerId == answer.AnswerId).Count();
                                answerCounts.Add(answerCount);
                            }
                        }
                        if (answerCounts.Count() > 0)
                        {
                            questionGraph.AnswerCounts = answerCounts;
                            questionGraphs.Add(questionGraph);
                        }
                    }

                    return questionGraphs;
                }
            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetQuestionsForGraph, UserId:" + userId + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

        public CustomerMessageModel SaveCustomerMessage(CustomerMessageModel customerMessageModel)
        {
            CustomerMessage customerMessage = new CustomerMessage();
            try
            {
                if (customerMessageModel.Attachment != null)
                {
                    var _aws3Services = new Aws3Services(_configuration, _context, _logger);
                    string fileName = customerMessageModel.Attachment.FileName.Substring(0, customerMessageModel.Attachment.FileName.IndexOf(".")) + "_" + DateTime.Now.ToString("yyyyMMddTHHmmss") + customerMessageModel.Attachment.FileName.Substring(customerMessageModel.Attachment.FileName.IndexOf("."));
                    var result = _aws3Services.UploadAttchmentFile(customerMessageModel.Attachment, fileName).GetAwaiter().GetResult();
                    if (result == true)
                    {
                        customerMessageModel.AttachmentFile = _configuration.GetSection("AWSBucket").Value + "attachment/" + fileName;
                    }

                }
                customerMessage.EmailAddress = customerMessageModel.EmailAddress;
                customerMessage.Message = customerMessageModel.Message;
                customerMessage.UserId = customerMessageModel.UserId;
                customerMessage.Subject = customerMessageModel.Subject;
                customerMessage.AttachmentFile = customerMessageModel.AttachmentFile;
                customerMessage.CreatedOn = DateTime.UtcNow;
                customerMessage.IsActive = true;
                _context.Add(customerMessage);
                _context.SaveChanges();
                var emailRes = _emailService.SendAttachmentEmail(customerMessageModel, customerMessageModel.Attachment);
                //customerMessageModel.emailBody = _context.GenericSettings.Where(s => s.SettingName == customerMessageModel.emailTemplate).Select(s => s.DefaultTextMax).FirstOrDefault();
            }
            catch (Exception ex)
            {
                customerMessageModel.ErrorMessage = ex.Message;
                _logger.LogError("Method: SaveCustomerMessage, UserId:" + customerMessageModel.UserId + ", Error: " + ex.Message, ex);
            }
            return customerMessageModel;
        }

        public EmailTemplate GetCustomerEmailTemplate()
        {
            try
            {
                EmailTemplate emailTemplateResult = _context.EmailTemplates.Where(p => p.TemplateCode == "CUSTMSG").FirstOrDefault();
                return emailTemplateResult;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetCustomerEmailTemplate, Error: " + ex.Message, ex);
                return null;
            }
        }

        public async Task<SearchCustomerResponse> GetQuestionnaireCustomerList(SearchCustomerResponse searchCustomerResponse)
        {
            try
            {
                var dp_params = new DynamicParameters();
                dp_params.Add("PageSize", searchCustomerResponse.pageSize, DbType.Int64);
                dp_params.Add("Skip", searchCustomerResponse.skip, DbType.Int64);
                dp_params.Add("TotalRows", 0, DbType.Int32, ParameterDirection.Output);
                dp_params.Add("Search", searchCustomerResponse.searchValue, DbType.String);
                dp_params.Add("SortColumn", searchCustomerResponse.sortColumn, DbType.String);
                dp_params.Add("SortDirection", searchCustomerResponse.sortDirection, DbType.String);
                var userType = _context.WebLogins.FirstOrDefault(x => x.UserId == Convert.ToInt32(searchCustomerResponse.userId)).UserTypeId;
                if (userType == (int)UserTypeEnum.B2B)
                {
                    var salonIds = _context.SalonsOwners.Where(x => x.UserId == Convert.ToInt32(searchCustomerResponse.userId) && x.IsActive == true).Select(x => x.SalonId).ToArray();
                    if (salonIds.Count() > 0)
                    {
                        using (var table = new DataTable())
                        {
                            table.Columns.Add("Item", typeof(int));

                            for (int i = 0; i < salonIds.Length; i++)
                                table.Rows.Add(Convert.ToInt32(salonIds[i]));
                            dp_params.Add("@SalonList", table.AsTableValuedParameter());
                        }
                    }
                    else
                    {
                        List<QuestionnaireCustomerList> lstQuestion = new List<QuestionnaireCustomerList>();
                        searchCustomerResponse.Data = lstQuestion;
                        searchCustomerResponse.RecordsTotal = 0;
                        return searchCustomerResponse;
                    }
                }
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var result = await connection.QueryAsync<QuestionnaireCustomerList>("sp_SelectAllCustomers", dp_params, commandType: CommandType.StoredProcedure);
                    searchCustomerResponse.Data = result.ToList();
                    searchCustomerResponse.RecordsTotal = dp_params.Get<int>("TotalRows");
                    return searchCustomerResponse;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetQuestionnaireCustomerList, Error: " + ex.Message, ex);
                return null;
            }

        }

        public Questionaire SaveSurveyImage(Questionaire questionaire)
        {
            List<Questionaire> objQuestionaire = new List<Questionaire>();

            var imageQuestionaire = _context.Questionaires.Where(x => x.QuestionId == 22 && x.UserId == questionaire.UserId && x.IsActive == true).LastOrDefault();

            try
            {
                if (imageQuestionaire != null)
                {
                    //imageQuestionaire.DescriptiveAnswer = _configuration.GetSection("WebUrl").Value + "/questionnaireFile/" + questionaire.DescriptiveAnswer;
                    imageQuestionaire.DescriptiveAnswer = "https://customer.myavana.com/questionnaireFile/" + questionaire.DescriptiveAnswer;
                    imageQuestionaire.CreatedOn = DateTime.Now;
                    imageQuestionaire.IsActive = true;
                }
                // _context.Update(imageQuestionaire);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveSurveyImage, UserId:" + questionaire.UserId + ", Error: " + ex.Message, ex);
            }

            return questionaire;
        }

        public Questionaire SaveSurveyImageAdmin(Questionaire questionaire)
        {
            List<Questionaire> objQuestionaire = new List<Questionaire>();

            var imageQuestionaire = _context.Questionaires.Where(x => x.QuestionId == 22 && x.UserId == questionaire.UserId && x.IsActive == true).LastOrDefault();

            try
            {
                if (imageQuestionaire != null)
                {
                    //imageQuestionaire.DescriptiveAnswer = _configuration.GetSection("WebUrl").Value + "/questionnaireFile/" + questionaire.DescriptiveAnswer;
                    imageQuestionaire.DescriptiveAnswer = _adminUrl + "questionnaireFile/" + questionaire.DescriptiveAnswer;
                    imageQuestionaire.CreatedOn = DateTime.Now;
                    imageQuestionaire.IsActive = true;
                }
                // _context.Update(imageQuestionaire);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveSurveyImageAdmin, UserId:" + questionaire.UserId + ", Error: " + ex.Message, ex);
            }

            return questionaire;
        }

        public UserEntity ChangeCustomerType(UserEntity userModel)
        {

            var userObj = _context.Users.Where(x => x.Id == userModel.Id).FirstOrDefault();

            try
            {
                if (userObj != null)
                {
                    //userObj.IsProCustomer = true;
                    CustomerTypeHistory customerTypeHistory = new CustomerTypeHistory();
                    customerTypeHistory.CustomerId = userModel.Id;
                    customerTypeHistory.OldCustomerTypeId = userObj.CustomerTypeId;
                    customerTypeHistory.NewCustomerTypeId = (int)userModel.CustomerTypeId;
                    customerTypeHistory.CreatedOn = DateTime.Now;
                    customerTypeHistory.IsActive = true;
                    customerTypeHistory.UpdatedByUserId = 1;
                    customerTypeHistory.Comment = "Updated by admin.";
                    _context.CustomerTypeHistory.Add(customerTypeHistory);

                    userObj.CustomerTypeId = userModel.CustomerTypeId;
                    if (userModel.CustomerTypeId == (int)CustomerTypeEnum.DigitalAnalysis || userModel.CustomerTypeId == (int)CustomerTypeEnum.HairKitPlus)
                    {
                        userObj.IsPaid = true;
                    }
                    else
                    {
                        userObj.IsPaid = false;
                    }
                }
                _context.Update(userObj);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: ChangeCustomerType, CustomerId:" + userModel.Id + ", Error: " + ex.Message, ex);
            }

            return userObj;
        }

        public UserEntity ActivateCustomer(UserEntity userModel)
        {

            var userObj = _context.Users.Where(x => x.Id == userModel.Id).FirstOrDefault();

            try
            {
                if (userObj != null)
                {
                    if (userObj.Active == true)
                    {
                        userObj.Active = false;
                    }
                    else
                    {
                        userObj.Active = true;
                    }
                }
                _context.Update(userObj);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: ActivateCustomer, CustomerId:" + userModel.Id + ", Error: " + ex.Message, ex);
            }

            return userObj;
        }
        public Questionaire SaveSurveyImagefromMobile(Questionaire questionaire)
        {
            List<Questionaire> objQuestionaire = new List<Questionaire>();
            int? latestQA = _context.Questionaires.Where(x => x.UserId == questionaire.UserId && x.IsActive == true).OrderByDescending(x => x.QA).FirstOrDefault()?.QA;

            var imageQuestionaire = _context.Questionaires.Where(x => x.QuestionId == 22 && x.UserId == questionaire.UserId && x.IsActive == true && x.QA == latestQA).LastOrDefault();

            try
            {
                if (imageQuestionaire != null)
                {
                    imageQuestionaire.DescriptiveAnswer = questionaire.DescriptiveAnswer;
                    imageQuestionaire.CreatedOn = DateTime.Now;
                    imageQuestionaire.IsActive = true;
                }
                else
                {
                    Questionaire questionaire1 = new Questionaire();
                    questionaire1.DescriptiveAnswer = questionaire.DescriptiveAnswer;
                    questionaire1.CreatedOn = DateTime.Now;
                    questionaire1.IsActive = true;
                    questionaire1.QuestionId = 22;
                    questionaire1.UserId = questionaire.UserId;
                    questionaire1.QA = latestQA ?? 0;
                    _context.Add(questionaire1);
                }

                var entity = _userManager.FindByIdAsync(questionaire.UserId).GetAwaiter().GetResult();
                if (entity != null)
                {
                    var hhcp = _context.HairProfiles.FirstOrDefault(x => x.UserId == entity.Email && x.IsActive == true);
                    if (hhcp == null)
                    {
                        UserEntity us = _context.Users.Where(x => x.Id == entity.Id).FirstOrDefault();
                        var hair = new Models.Entities.HairProfile();

                        hair.UserId = us.Email;
                        if (us.CustomerTypeId == (int)CustomerTypeEnum.DigitalAnalysis)
                        {
                            hair.HealthSummary = "Hi " + us.FirstName + " ! " + "\r\nYour Digital Hair Profile includes your introductory product recommendations based on a quick analysis of your hair. We incorporated your Hair Goals, Hair Challenges, Product Recommendations, and % breakdown of your Unique Hair Type Combination. To get a comprehensive healthy hair care plan, make sure to get your hair analysis kit in the menu to your left. This should get you started in the meantime! If you have any questions, please email us at support@myavana.com\r\nLove, \r\nMYAVANA ";
                        }
                        else
                        {
                            hair.HealthSummary = "Hi " + us.FirstName + " ! " + "\r\nYour Healthy Hair Care Plan includes your Hair ID, Hair Goals and Challenges, Hair Strand Analysis, Product Recommendations, Ingredients, Regimens, and Education. We have also included some personal notes from your MYAVANA lab analyst and hair consultant.\r\nLove, \r\nTanisha \r\nHair Analyst";
                        }
                        hair.IsActive = true;
                        hair.CreatedOn = DateTime.Now;
                        hair.IsDrafted = false;
                        hair.IsViewEnabled = true;
                        _context.Add(hair);
                    }

                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveSurveyImagefromMobile, UserId:" + questionaire.UserId + ", Error: " + ex.Message, ex);
            }

            return questionaire;
        }

        public DailyRoutineTracker GetDailyRoutineWeb(string userId, string trackTime)
        {
            try
            {
                if (userId != null)
                {
                    if (DateTime.TryParse(trackTime, out DateTime trackTimeDate))
                    {
                        var selectedTracker = _context.DailyRoutineTracker
                            .Where(x => x.UserId == userId && x.TrackTime.Date == trackTimeDate.Date)
                            .FirstOrDefault();

                        if (selectedTracker != null)
                        {
                            DailyRoutineTracker dailyRoutine = new DailyRoutineTracker
                            {
                                ProfileImage = selectedTracker.ProfileImage,
                                HairStyle = selectedTracker.HairStyle,
                                Description = selectedTracker.Description,
                                CurrentMood = selectedTracker.CurrentMood,
                                GuidanceNeeded = selectedTracker.GuidanceNeeded,
                            };

                            return dailyRoutine;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetDailyRoutine, Error: " + ex.Message, ex);
                return null;
            }
        }

        public Questionaire SaveSurveyImagefromMobileForHHCP(Questionaire questionaire)
        {
            List<Questionaire> objQuestionaire = new List<Questionaire>();
            int? latestQA = _context.Questionaires.Where(x => x.UserId == questionaire.UserId && x.IsActive == true).OrderByDescending(x => x.QA).FirstOrDefault()?.QA;

            var imageQuestionaire = _context.Questionaires.Where(x => x.QuestionId == 22 && x.UserId == questionaire.UserId && x.IsActive == true && x.QA == latestQA).LastOrDefault();

            try
            {
                if (imageQuestionaire != null)
                {

                    imageQuestionaire.DescriptiveAnswer = questionaire.DescriptiveAnswer;
                    imageQuestionaire.CreatedOn = DateTime.Now;
                    imageQuestionaire.IsActive = true;
                }
                else
                {
                    Questionaire questionaire1 = new Questionaire();
                    questionaire1.DescriptiveAnswer = questionaire.DescriptiveAnswer;
                    questionaire1.CreatedOn = DateTime.Now;
                    questionaire1.IsActive = true;
                    questionaire1.QuestionId = 22;
                    questionaire1.UserId = questionaire.UserId;
                    questionaire1.QA = latestQA ?? 0;
                    _context.Add(questionaire1);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: SaveSurveyImagefromMobileForHHCP, UserId:" + questionaire.UserId + ", Error: " + ex.Message, ex);
            }

            return questionaire;
        }

        public UserProfileImageModel GetProfileImageCustomer(string userId)
        {
            try
            {
                if (userId != null)
                {
                    var user = _context.Users
                        .Where(s => s.Id.ToString().ToLower() == userId)
                        .FirstOrDefault();

                    if (user != null)
                    {
                        UserProfileImageModel userEntity = new UserProfileImageModel
                        {
                            Id = user.Id.ToString(),
                            Email = user.Email,
                            ImageURL = user.ImageURL
                        };

                        return userEntity;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Method: GetProfileImageCustomer, Error: " + ex.Message, ex);
                return null;
            }
        }
        public List<HairProfileAnayst> GetAnalystList()
        {
            try
            {
                var analystlist = (from hr in _context.HairAnalyst
                                   select new HairProfileAnayst
                                   {
                                       HairAnalystId = hr.HairAnalystId,
                                       AnalystName = hr.AnalystName
                                   }).OrderByDescending(x => x.HairAnalystId).ToList();
                return analystlist;


            }
            catch (Exception Ex)
            {
                _logger.LogError("Method: GetAnalystList" + ", Error: " + Ex.Message, Ex);
                return null;
            }
        }

    }
}
