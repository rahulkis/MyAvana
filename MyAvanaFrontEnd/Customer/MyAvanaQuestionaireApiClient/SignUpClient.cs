using MyAvanaQuestionaireModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MyAvanaQuestionaire.Models;

namespace MyAvanaQuestionaireApiClient
{
    public partial class ApiClient
    {
        public async Task<List<Countries>> GetCountriesList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Account/GetCountriesList"));
            var response = await GetAsyncData<Countries>(requestUrl);
            List<Countries> lstCountries = JsonConvert.DeserializeObject<List<Countries>>(Convert.ToString(response.value));
            return lstCountries;
            
        }
        //public async Task<List<States>> GetStatesList(int CountryId)
        //{
        //    var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Account/GetStatesList"), "?userId=" + userId);
        //    var response = await GetAsyncData<States>(requestUrl);
        //    List<States> list = JsonConvert.DeserializeObject<List<States>>(Convert.ToString(response.value));
        //    return list;
        //}
    }
}
