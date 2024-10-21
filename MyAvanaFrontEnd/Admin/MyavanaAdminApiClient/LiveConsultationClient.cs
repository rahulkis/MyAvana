using MyavanaAdminModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MyavanaAdminApiClient
{
    public partial class ApiClient
    {
        public async Task<List<GetCustomerScheduleDetails>> GetConsultationList()
        {
             var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "LiveSchedules/GetConsultationList"));
            var response = await GetAsyncData <GetCustomerScheduleDetails>(requestUrl);
            List<GetCustomerScheduleDetails> LiveConsultation = JsonConvert.DeserializeObject<List<GetCustomerScheduleDetails>>(Convert.ToString(response.value));
            return LiveConsultation;
        }
        public async Task<Message<LiveConsultationUserDetails>> ChangeIsApproved(LiveConsultationUserDetails Isapprovedval)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "LiveSchedules/ChangeIsApproved"));
            var result = await PostAsync<LiveConsultationUserDetails>(requestUrl, Isapprovedval);
            return result;
        }
        public async Task<Message<LiveConsultationUserDetails>> FetchConsultationDetails(LiveConsultationUserDetails LiveConsultationUserDetails)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "LiveSchedules/FetchConsultationDetails"));
            var result = await PostAsync<LiveConsultationUserDetails>(requestUrl, LiveConsultationUserDetails);
            return result;

        }
        public async Task<LiveConsultationModel> JoinLiveConsultation(LiveConsultationModel liveConsultationModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "LiveSchedules/JoinLiveConsultation"));
            var result = await PostAsync<LiveConsultationModel>(requestUrl, liveConsultationModel);
            return result.Data;
        }
        public async Task<LiveConsultationModel> CheckIsOtherParticipantReady(LiveConsultationModel liveConsultationModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "LiveSchedules/CheckIsOtherParticipantReady"));
            var result = await PostAsync<LiveConsultationModel>(requestUrl, liveConsultationModel);
            return result.Data;
        }
        public async Task<LiveConsultationModel> UpdateLiveConsultationInformation(LiveConsultationModel liveConsultationModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "LiveSchedules/UpdateLiveConsultationInformation"));
            var result = await PostAsync<LiveConsultationModel>(requestUrl, liveConsultationModel);
            return result.Data;
        }

    }
}
