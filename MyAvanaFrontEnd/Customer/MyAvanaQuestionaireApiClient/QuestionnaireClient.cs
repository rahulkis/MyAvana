using MyAvanaQuestionaireModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyAvanaQuestionaireApiClient
{
    public partial class ApiClient
    {
        public async Task<Message<HairProfileCustomerModel>> GetHairProfileCustomer(HairProfileCustomerModel hairProfileModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetHairProfileCustomer"));
            var result = await PostAsync<HairProfileCustomerModel>(requestUrl, hairProfileModel);
            return result;
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
        public async Task<Message<QuestionAnswerModel>> GetCustomerQuestionaireDetails(QuestionAnswerModel questionaire)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Questionnaire/GetQuestionnaireCustomerDetails"));
            var result = await PostAsync<QuestionAnswerModel>(requestUrl, questionaire);
            return result;
        }

        public async Task<dynamic> ImageClassify(Imagerequest imagerequest)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Image/ImageClassify"));
                var result = await PostAsync(requestUrl, imagerequest);
                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
         
        }

        public async Task<Message<DigitalAssessmentModel>> CreateHHCPByDigitalAssessment(DigitalAssessmentModel digitalAssessmentModel)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/CreateHHCPByDigitalAssessment"));
                var result = await PostAsync<DigitalAssessmentModel>(requestUrl, digitalAssessmentModel);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        
        }
        public async Task<List<HairProfileSelectModel>> GetHHCPList(string userId)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetHHCPList"), "?userId=" + userId+ "&isRequestedFromCustomer=true");
            var response = await GetAsyncData<HairProfileSelectModel>(requestUrl);
            List<HairProfileSelectModel> list = JsonConvert.DeserializeObject<List<HairProfileSelectModel>>(Convert.ToString(response.value));
            return list;
        }

        public async Task<Message<HairProfileCustomerModel>> GetHairProfileCustomerTab2(HairProfileCustomerModel hairProfileModel)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetHairProfileCustomerTab2"));
                var result = await PostAsync<HairProfileCustomerModel>(requestUrl, hairProfileModel);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<Message<HairProfileCustomerModel>> GetHairProfileCustomerExceptTab2(HairProfileCustomerModel hairProfileModel)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetHairProfileCustomerExceptTab2"));
                var result = await PostAsync<HairProfileCustomerModel>(requestUrl, hairProfileModel);
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<Message<fileData>> ProfileImageUpload(fileData file)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/FileUploadCustomer"));
            var result = await PostAsync<fileData>(requestUrl, file);
            return result;
        }
        public async Task<List<SharedHHCPModel>> GetSharedHHCPList(Guid userId)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetSharedHHCPList"), "?userId=" + userId);
            try
            {
                var response = await GetAsyncData<SharedHHCPModel>(requestUrl);
                List<SharedHHCPModel> hHCPModels = JsonConvert.DeserializeObject<List<SharedHHCPModel>>(Convert.ToString(response.value));
                return hHCPModels;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Message<SharedHHCPModel>> RevokeAccessHHCP(SharedHHCPModel sharedHHCP)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/RevokeAccess"));
            var result = await PostAsync<SharedHHCPModel>(requestUrl, sharedHHCP);
            return result;
        }
        public async Task<Message<string>> ShareEmailExist(string email,int hairProfileId,Guid sharedBy)
        {
            try
            {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/ShareEmailExist"), "?email=" + email+ "&hairProfileId="+ hairProfileId+ "&sharedBy="+ sharedBy);
                var result = await PostAsync<string>(requestUrl, email);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<List<SharedHHCPModel>> GetSharedWithMeHHCPList(Guid userId)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetSharedWithMeHHCPList"), "?userId=" + userId);
            try
            {
                var response = await GetAsyncData<SharedHHCPModel>(requestUrl);
                List<SharedHHCPModel> hHCPModels = JsonConvert.DeserializeObject<List<SharedHHCPModel>>(Convert.ToString(response.value));
                return hHCPModels;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
