using System;
using System.Collections.Generic;
using System.Text;
using MyavanaAdminModels;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyavanaAdminApiClient
{
    public partial class ApiClient
    {
        public async Task<Message<StatusTrackerModel>> SaveHairAnalysisStatus(StatusTrackerModel model)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "account/SaveHairAnalysisStatus"));
            var result = await PostAsync<StatusTrackerModel>(requestUrl, model);
            return result;
        }
        public async Task<List<StatusTrackerModel>> GetStatusTrackerList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairAnalysisStatus/GetStatusTrackerList"));
            List<StatusTrackerModel> response = await GetAsyncList<StatusTrackerModel>(requestUrl);
            return response;
        }

        public async Task<List<HairAnalysisStatusModel>> GetHairAnalysisStatusList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairAnalysisStatus/GetHairAnalysisStatusList"));
            var response = await GetAsyncData<HairAnalysisStatusModel>(requestUrl);
            List<HairAnalysisStatusModel> questionaire = JsonConvert.DeserializeObject<List<HairAnalysisStatusModel>>(Convert.ToString(response.data));
            return questionaire;
        }
        public async Task<Message<StatusTrackerModel>> ChangeHairAnalysisStatus(StatusTrackerModel trackerModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairAnalysisStatus/ChangeHairAnalysisStatus"));
            var result = await PostAsync<StatusTrackerModel>(requestUrl, trackerModel);
            return result;
        }
        public async Task<Message<StatusTrackerModel>> AddToStatusTracker(StatusTrackerModel trackerModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairAnalysisStatus/AddToStatusTracker"));
            var result = await PostAsync<StatusTrackerModel>(requestUrl, trackerModel);
            return result;
        }
    }
}
