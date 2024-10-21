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
        public async Task<List<SubscriberModel>> GetSubscriberList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Subscribers/GetSubscriberList"));
             var response = await GetAsyncData<SubscriberModel>(requestUrl);
            List<SubscriberModel> lstsubscribers = JsonConvert.DeserializeObject<List<SubscriberModel>>(Convert.ToString(response.data));
            return lstsubscribers;
        }

        public async Task<Message<SubscriberModel>> CancelSubscription(SubscriberModel subscriberModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Subscribers/CancelSubscription"));
            var result = await PostAsync<SubscriberModel>(requestUrl, subscriberModel);
            return result;
        }
    }
}
