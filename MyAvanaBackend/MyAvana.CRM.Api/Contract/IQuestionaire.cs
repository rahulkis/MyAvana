using Microsoft.AspNetCore.Mvc;
using MyAvana.Models.Entities;
using MyAvana.Models.ViewModels;
using MyAvanaApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IQuestionaire
    {
        Task<(JsonResult result, bool success, string error)> AuthenticateUser(string email);
        IEnumerable<Questionaire> SaveSurvey(IEnumerable<Questionaire> questionaires);
        IEnumerable<Questionaire> SaveSurveyAdmin(IEnumerable<Questionaire> questionaires);
        Task<List<QuestionAnswerModel>> GetQuestionnaire(int start, int length, int userId);
        Task<List<QuestionAnswerModel>> GetQuestionnaireForCustomer(string userId);
        Task<List<QuestionAnswerModel>> GetQuestionnaireAbsenceUserList(QuestionnaireAbsenceModel questionnaireAbsenceModel);
        List<QuestionnaireCustomerList> GetQuestionnaireCustomerList();
        Task<SearchCustomerResponse> GetQuestionnaireCustomerList(SearchCustomerResponse searchCustomerResponse);
        Task<List<CustomerMessageList>> GetCustomerMessagesList(Guid userId);
        Task<List<CustomerTypeHistoryModel>> GetCustomerTypeHistory(Guid userId);
        Task<QuestionAnswerModel> GetQuestionnaireCustomer(string userId,int QA);
        Task<QuestionAnswerModel> GetQuestionnaireCustomerAll(string userId);
        bool DeleteQuest(QuestModel quest);
        List<Questionaire> GetFilledSurvey(ClaimsPrincipal user);
        List<QuestionGraph> GetQuestionsForGraph(string userId);
        CustomerMessageModel SaveCustomerMessage(CustomerMessageModel customerMessageModel);
        EmailTemplate GetCustomerEmailTemplate();
        Questionaire SaveSurveyImage(Questionaire questionaire);
        Questionaire SaveSurveyImageAdmin(Questionaire questionaire);
        UserEntity ChangeCustomerType(UserEntity userModel);
        UserEntity ActivateCustomer(UserEntity userModel);
        Questionaire SaveSurveyImagefromMobile(Questionaire questionaire);
        DailyRoutineTracker GetDailyRoutineWeb(string userId, string trackTime);
        Questionaire SaveSurveyImagefromMobileForHHCP(Questionaire questionaire);
        UserProfileImageModel GetProfileImageCustomer(string userId);
        List<HairProfileAnayst> GetAnalystList();
       // IEnumerable<Questionaire> SaveCompleteSurveyFromMobile(IEnumerable<Questionaire> questionaires);
    }
}
