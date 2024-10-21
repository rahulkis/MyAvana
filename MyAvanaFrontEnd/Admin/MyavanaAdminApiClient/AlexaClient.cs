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
        public async Task<Message<AlexaFAQModel>> AddAlexaFAQ(AlexaFAQModel alexaFAQModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Alexa/AddAlexaFAQ"));
            var result = await PostAsync<AlexaFAQModel>(requestUrl, alexaFAQModel);
            return result;
        }

        public async Task<List<AlexaFAQModel>> GetAlexaFAQs(int start, int length)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Alexa/GetAlexaFAQs"), "?start=" + start + "&length=" + length);
            List<AlexaFAQModel> response = await GetAsyncList<AlexaFAQModel>(requestUrl);
            return response;
        }

        public async Task<Message<AlexaFAQModel>> GetAlexaFAQById(AlexaFAQModel alexaFAQModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Alexa/GetAlexaFAQById"));
            var result = await PostAsync<AlexaFAQModel>(requestUrl, alexaFAQModel);
            return result;
        }

        public async Task<Message<AlexaFAQModel>> DeleteAlexaFAQ(AlexaFAQModel alexaFAQModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Alexa/DeleteAlexaFAQ"));
            var result = await PostAsync<AlexaFAQModel>(requestUrl, alexaFAQModel);
            return result;
        }
       
    }
}
