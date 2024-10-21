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
        public async Task<Message<SalonModel>> CreateNewSalon(SalonModel model)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Salons/AddNewSalon"));
            var result = await PostAsync<SalonModel>(requestUrl, model);
            return result;
        }

        public async Task<List<SalonModel>> GetSalons(int start,int length)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Salons/GetSalons"), "?start="+start+"&length="+length);
                List<SalonModel> response = await GetAsyncList<SalonModel>(requestUrl);
                return response;
        }
       
        public async Task<Message<SalonModel>> GetSalonById(SalonModel salonDetails)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Salons/GetSalonByid"));
            var result = await PostAsync<SalonModel>(requestUrl, salonDetails);
            return result;
        }

        public async Task<List<SalonsListModel>> GetSalonList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Salons/GetSalonList"));
            var response = await GetAsyncData<SalonsListModel>(requestUrl);
            List<SalonsListModel> lstsubscribers = JsonConvert.DeserializeObject<List<SalonsListModel>>(Convert.ToString(response.data));
            return lstsubscribers;
        }

        public async Task<Message<SalonHairProfileModel>> UpdateHairProfileSalon(SalonHairProfileModel model)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Salons/UpdateHairProfileSalon"));
            var result = await PostAsync<SalonHairProfileModel>(requestUrl, model);
            return result;
        }
        public async Task<Message<SalonNotesModel>> SaveSalonNotes(SalonNotesModel model)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/SaveSalonNotes"));
            var result = await PostAsync<SalonNotesModel>(requestUrl, model);
            return result;
        }
    }
}
