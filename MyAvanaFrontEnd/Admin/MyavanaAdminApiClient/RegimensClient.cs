using MyavanaAdminModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyavanaAdminApiClient
{
	public partial class ApiClient
	{
        public async Task<Message<RegimensModel>> SaveRegimens(RegimensModel regimensModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Article/UploadArticles"));
            var result = await PostAsync<RegimensModel>(requestUrl, regimensModel);
            return result;
        }

        public async Task<List<RegimensModel>> GetRegimensList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Regimens/GetRegimens"));
            var response = await GetAsyncData<RegimensModel>(requestUrl);
            List<RegimensModel> blogPosts = JsonConvert.DeserializeObject<List<RegimensModel>>(Convert.ToString(response.data));
            return blogPosts;
        }

        public async Task<Message<RegimensModel>> GetRegimensById(RegimensModel regimensModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Regimens/GetRegimensById"));
            var result = await PostAsync<RegimensModel>(requestUrl, regimensModel);
            return result;
        }

        public async Task<Message<RegimensModel>> DeleteRegimens(RegimensModel regimensModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Regimens/DeleteRegimens"));
            var result = await PostAsync<RegimensModel>(requestUrl, regimensModel);
            return result;
        }
    }
}
