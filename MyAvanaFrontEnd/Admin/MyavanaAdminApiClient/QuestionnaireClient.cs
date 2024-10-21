using MyavanaAdminModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace MyavanaAdminApiClient
{
    public partial class ApiClient
    {
        public async Task<List<QuestionAnswerModel>> GetQuestionnaireList(int start, int length, int userId)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/GetQuestionnaireAdmin"), "?start=" + start + "&length=" + length+ "&userId=" + userId);
            var response = await GetAsyncData<QuestionAnswerModel>(requestUrl);
            List<QuestionAnswerModel> questionaire = JsonConvert.DeserializeObject<List<QuestionAnswerModel>>(Convert.ToString(response.data));
            return questionaire;
        }
        public async Task<List<QuestionAnswerModel>> GetQuestionnaireForCustomer(string userId)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/GetQuestionnaireForCustomer"), "?userId=" + userId);
            var response = await GetAsyncData<QuestionAnswerModel>(requestUrl);
            List<QuestionAnswerModel> questionaire = JsonConvert.DeserializeObject<List<QuestionAnswerModel>>(Convert.ToString(response.data));
            return questionaire;
        }
        public async Task<List<QuestionAnswerModel>> GetQuestionnaireAbsenceUserList(string userId)
        {
            var res = new List<QuestionAnswerModel> ();
            QuestionnaireAbsenceModel questionnaireAbsenceModel = new QuestionnaireAbsenceModel()
            {
                userId = Convert.ToInt32(userId)
            };
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/GetQuestionnaireAbsenceUserList"));
            var response = await PostAsync<QuestionnaireAbsenceModel>(requestUrl, questionnaireAbsenceModel);
            //QuestionnaireAbsenceModel questionaire = JsonConvert.DeserializeObject<QuestionnaireAbsenceModel>(Convert.ToString(response.data));
            if(response.Data!=null && response.Data.QuestionAnswerModels.Count > 0)
            {
                res = response.Data.QuestionAnswerModels;
            }
            return res;
        }

        public async Task<Message<HairProfileAdminModel>> GetHairProfileAdmin(HairProfileAdminModel hairProfileModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetHairProfileAdmin"));
            var result = await PostAsync<HairProfileAdminModel>(requestUrl, hairProfileModel);
            return result;
        }

        public async Task<Message<QuestionaireSelectedAnswer>> GetQuestionaireAnswer(QuestionaireSelectedAnswer hairProfileModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetQuestionaireAnswer"));
            var result = await PostAsync<QuestionaireSelectedAnswer>(requestUrl, hairProfileModel);
            return result;
        }
        public async Task<Message<HairProfile>> AutoGenerateHHCP(HairProfile hairProfile)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/AutoGenerateHHCP"));
                var result = await PostAsync<HairProfile>(requestUrl, hairProfile);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<List<QuestionnaireCustomerList>> QuestionnaireCustomerList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/GetQuestionnaireCustomerList"));
            var response = await GetAsyncData<QuestionnaireCustomerList>(requestUrl);
            List<QuestionnaireCustomerList> blogPosts = JsonConvert.DeserializeObject<List<QuestionnaireCustomerList>>(Convert.ToString(response.value));
            return blogPosts;

        }
        public async Task<Message<SearchCustomerResponse>> GetQuestionnaireCustomerList(SearchCustomerResponse gridParams)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/GetQuestionnaireCustomerList"));
            var response = await PostAsync(requestUrl, gridParams);
            return response;
        }

        public async Task<List<CustomerMessageViewModel>> CustomerMessagesList(string userId)
        {
            QuestionaireModel questionaireModel = new QuestionaireModel { Userid = userId };
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/GetCustomerMessagesList?id=" + userId));
            var response = await GetAsyncData<CustomerMessageViewModel>(requestUrl);
            List<CustomerMessageViewModel> custMsgs = JsonConvert.DeserializeObject<List<CustomerMessageViewModel>>(Convert.ToString(response.value));
            return custMsgs;

        }

        public async Task<Message<IEnumerable<Questionaire>>> SaveQuestionnaireSurvey(IEnumerable<Questionaire> questionaire)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/SaveSurveyAdmin"));
            var result = await PostAsync<IEnumerable<Questionaire>>(requestUrl, questionaire);
            return result;
        }

        public async Task<Message<QuestModel>> DeleteQuest(QuestModel questModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/DeleteQuest"));
            var result = await PostAsync<QuestModel>(requestUrl, questModel);
            return result;
        }

        public async Task<Message<UserModel>> ChangeCustomerType(UserModel questModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/ChangeCustomerType"));
            var result = await PostAsync<UserModel>(requestUrl, questModel);
            return result;
        }
        public async Task<Message<UserModel>> ActivateCustomer(UserModel questModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/ActivateCustomer"));
            var result = await PostAsync<UserModel>(requestUrl, questModel);
            return result;
        }

        public async Task<List<QuestionGraph>> GetQuestionsForGraph(string userId)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/GetQuestionsForGraph"), "?id=" + userId);
            var response = await GetAsyncData<QuestionGraph>(requestUrl);
            List<QuestionGraph> questionaire = JsonConvert.DeserializeObject<List<QuestionGraph>>(Convert.ToString(response.data));
            return questionaire;
        }

        public async Task<Message<CustomerMessageModel>> SaveCustomerMessage(CustomerMessageModel customerMessageModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Questionnaire/SaveCustomerMessage"));
            var result = await PostAsync<CustomerMessageModel>(requestUrl, customerMessageModel);
            return result;
        }

        public async Task<EmailTemplate> GetCustomerEmailTemplate()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/GetCustomerEmailTemplate"));
            var result = await GetAsyncData<EmailTemplate>(requestUrl);
            EmailTemplate emailTemplate = JsonConvert.DeserializeObject<EmailTemplate>(Convert.ToString(result.data));
            return emailTemplate;
        }
        public async Task<List<MessageTemplateModel>> GetMessageTempleteList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetMessageTempleteList"));
            var response = await GetAsyncData<MessageTemplateModel>(requestUrl);
            List<MessageTemplateModel> lstTemplates = JsonConvert.DeserializeObject<List<MessageTemplateModel>>(Convert.ToString(response.value));
            return lstTemplates;
        }
        public async Task<Message<QuestionaireModel>> GetQuestionaireDetails(QuestionaireModel questionaire)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetQuestionaireDetails"));
                var result = await PostAsync<QuestionaireModel>(requestUrl, questionaire);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<Message<DigitalAssessmentModel>> SaveCustomerAIResultForMobile(DigitalAssessmentModel digitalAssessmentModel)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/SaveCustomerAIResultForMobile"));
                var result = await PostAsync<DigitalAssessmentModel>(requestUrl, digitalAssessmentModel);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<Message<QuestionaireImage>> SaveSurveyImage(QuestionaireImage QuestionaireImageParam)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/SaveSurveyImageAdmin"));
                var result = await PostAsync<QuestionaireImage>(requestUrl, QuestionaireImageParam);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<CustomerAIResult> GetLatestCustomerAIResult(string userId)
        {
            
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetLatestCustomerAIResultAdmin"), "?userId=" + userId);
            var response = await GetAsyncData<CustomerAIResult>(requestUrl); // Assuming GetAsyncData returns HttpResponseMessage
            CustomerAIResult questionaire = JsonConvert.DeserializeObject<CustomerAIResult>(Convert.ToString(response.data));
            return questionaire;
        }

        public async Task<List<HairProfileAnayst>> GetAnalystList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/GetAnalystList"));
            var response = await GetAsyncData<HairProfileAnayst>(requestUrl);
            List<HairProfileAnayst> list = JsonConvert.DeserializeObject<List<HairProfileAnayst>>(Convert.ToString(response.value));
            return list;
        }

    }
}
