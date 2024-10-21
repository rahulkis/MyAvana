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
        public async Task<Message<MobileHelpFAQ>> SaveMobileHelpFAQ(MobileHelpFAQ mobileHelpFAQEntity)
        {
            
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "MobileHelp/SaveMobileHelpFAQ"));
                var result = await PostAsync<MobileHelpFAQ>(requestUrl, mobileHelpFAQEntity);
            return result;
            
        }
        public async Task<List<MobileHelpFAQ>> GetMobileHelpFaqList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "MobileHelp/GetMobileHelpFaqList")); try
            {
                var response = await GetAsyncData<MobileHelpFAQ>(requestUrl);

                List<MobileHelpFAQ> FAQs = JsonConvert.DeserializeObject<List<MobileHelpFAQ>>(Convert.ToString(response.data));
                return FAQs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Message<MobileHelpFAQ>> GetMobileHelpFaqById(MobileHelpFAQ FAQ)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "MobileHelp/GetMobileHelpFaqById"));
            var result = await PostAsync<MobileHelpFAQ>(requestUrl, FAQ);
            return result;
        }
        public async Task<Message<MobileHelpFAQ>> DeleteMobileHelpFaq(MobileHelpFAQ mobileHelpFaqModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "MobileHelp/DeleteMobileHelpFaq"));
            var result = await PostAsync<MobileHelpFAQ>(requestUrl, mobileHelpFaqModel);
            return result;
        }


        public async Task<List<MobileHelpTopic>> GetMobileHelpTopicList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "MobileHelp/GetMobileHelpTopicList"));
            var response = await GetAsyncResponse<MobileHelpTopic>(requestUrl);
            List<MobileHelpTopic> media = response; // JsonConvert.DeserializeObject<List<MediaLinkEntityModel>>(Convert.ToString(response.value.media));
            return media;
        }
    }
}
