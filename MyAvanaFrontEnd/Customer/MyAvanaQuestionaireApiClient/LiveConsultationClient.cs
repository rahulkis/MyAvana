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
        public async Task<TimeZoneLiveSchedule> GetTime()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "LiveSchedules/GetTime"));
            var response = await GetAsyncData<TimeZoneLiveSchedule>(requestUrl);

            TimeZoneLiveSchedule scheduleTimes = JsonConvert.DeserializeObject<TimeZoneLiveSchedule>(Convert.ToString(response.value));
           
            int id = GetTimeScheduleId();
           
            scheduleTimes.Time = scheduleTimes.Time.FindAll(x => x.ScheduleTimeId > id);
            return scheduleTimes;
        }
        public async Task<Message<LiveConsultationUserDetails>> SaveConsultationDetails(LiveConsultationUserDetails LiveConsultationUserDetails)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "LiveSchedules/SaveConsultationDetails"));
            var result = await PostAsync<LiveConsultationUserDetails>(requestUrl, LiveConsultationUserDetails);
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
        public async Task<LiveConsultationModel> UpdateLiveConsultationInformation(LiveConsultationModel liveConsultationModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "LiveSchedules/UpdateLiveConsultationInformation"));
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

        private int GetTimeScheduleId()
        {
            var currentTimeHour = DateTime.Now.Hour;
            var currentTimeMin = DateTime.Now.Minute;
            int id = 0;
            if (currentTimeHour == 9 && currentTimeMin < 30)
            {
                id = 1;
            }
            else if (currentTimeHour == 9 && currentTimeMin > 30)
            {
                id = 2;
            }
            else if (currentTimeHour == 10 && currentTimeMin < 30)
            {
                id = 3;
            }
            else if (currentTimeHour == 10 && currentTimeMin > 30)
            {
                id = 4;
            }
            else if (currentTimeHour == 11 && currentTimeMin < 30)
            {
                id = 5;
            }
            else if (currentTimeHour == 11 && currentTimeMin > 30)
            {
                id = 6;
            }
            else if (currentTimeHour == 12 && currentTimeMin < 30)
            {
                id = 7;
            }
            else if (currentTimeHour == 12 && currentTimeMin > 30)
            {
                id = 8;
            }
            else if (currentTimeHour == 13 && currentTimeMin < 30)
            {
                id = 9;
            }
            else if (currentTimeHour == 13 && currentTimeMin > 30)
            {
                id = 10;
            }
            else if (currentTimeHour == 14 && currentTimeMin < 30)
            {
                id = 11;
            }
            else if (currentTimeHour == 14 && currentTimeMin > 30)
            {
                id = 12;
            }
            else if (currentTimeHour == 15 && currentTimeMin < 30)
            {
                id = 13;
            }
            else if (currentTimeHour == 15 && currentTimeMin > 30)
            {
                id = 14;
            }
            else if (currentTimeHour == 16 && currentTimeMin < 30)
            {
                id = 15;
            }
            else if (currentTimeHour == 16 && currentTimeMin > 30)
            {
                id = 16;
            }
            else if (currentTimeHour == 17 && currentTimeMin < 30)
            {
                id = 17;
            }
            else if (currentTimeHour == 17 && currentTimeMin > 30)
            {
                id = 18;
            }
            else if (currentTimeHour == 18 && currentTimeMin < 30)
            {
                id = 18;
            }
            return id;
        }
    }
}
