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
        public async Task<Message<StylistModel>> AddUpdateStylist(StylistModel stylistModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Stylist/AddUpdateStylist"));
            var result = await PostAsync<StylistModel>(requestUrl, stylistModel);
            return result;
        }

        public async Task<Message<StylishAdminModel>> GetStylishAdmin(StylishAdminModel stylishAdminModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Stylist/GetStylishAdmin"));
            var result = await PostAsync<StylishAdminModel>(requestUrl, stylishAdminModel);
            return result;
        }

        public async Task<List<StylistSpecialtyModel>> StylistSpecialty()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Stylist/GetStylistSpecialty"));
            var response = await GetAsyncData<StylistSpecialtyModel>(requestUrl);
            List<StylistSpecialtyModel> stylistSpecialty = JsonConvert.DeserializeObject<List<StylistSpecialtyModel>>(Convert.ToString(response.value));
            return stylistSpecialty;
        }
        public async Task<List<StylistListModel>> GetStylistList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Stylist/GetStylistList"));
            var response = await GetAsyncData<StylistListModel>(requestUrl);
            List<StylistListModel> stylish = JsonConvert.DeserializeObject<List<StylistListModel>>(Convert.ToString(response.data));
            return stylish;
        }

        public async Task<Message<StylistModel>> DeleteStylist(StylistModel stylist)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Stylist/DeleteStylist"));
            var result = await PostAsync<StylistModel>(requestUrl, stylist);
            return result;
        }

		public async Task<List<StylistListModel>> AddStylistList(List<StylistListModel> stylistModel)
		{
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Stylist/AddStylistList"));
			var result = await PostAsync<List<StylistListModel>>(requestUrl, stylistModel);
			return result.Data;
		}
	}
}
