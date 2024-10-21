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
        public async Task<Message<WebLogin>> Login(WebLogin webLogin)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "WebLogin/Login"));
            var result = await PostAsync<WebLogin>(requestUrl, webLogin);
            return result;
        }

        public async Task<List<WebLogin>> GetUsers(string userId)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "WebLogin/GetUsers"), "?id=" + userId);
            List<WebLogin> response = await GetAsyncList<WebLogin>(requestUrl);
            return response;
        }

        public async Task<Message<WebLoginModel>> GetUserById(WebLoginModel webLogin)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "WebLogin/GetUserByid"));
            var result = await PostAsync<WebLoginModel>(requestUrl, webLogin);
            return result;
        }

        public async Task<Message<WebLoginModel>> CreateNewUser(WebLoginModel webLogin)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "WebLogin/AddNewUser"));
            var result = await PostAsync<WebLoginModel>(requestUrl, webLogin);
            return result;
        }

        public async Task<Message<WebLogin>> DeleteUser(WebLogin webLogin)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "WebLogin/DeleteUser"));
            var result = await PostAsync<WebLogin>(requestUrl, webLogin);
            return result;
        }

        public async Task<Message<fileData>> ImageUpload(fileData file)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/FileUpload"));
            var result = await PostAsync<fileData>(requestUrl, file);
            return result;
        }
        public async Task<List<UserSalonOwnerModel>> UserOwnerSalons()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "WebLogin/GetOwnerSalons"));
            var response = await GetAsyncData<UserSalonOwnerModel>(requestUrl);
            List<UserSalonOwnerModel> stylistSpecialty = JsonConvert.DeserializeObject<List<UserSalonOwnerModel>>(Convert.ToString(response.value));
            return stylistSpecialty;
        }
        public async Task<Message<ResetPasswordModel>> ResetPassword(ResetPasswordModel resetPassword)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "WebLogin/ResetAdminPassword"));
            var result = await PostAsync(requestUrl, resetPassword);
            return result;
        }

        public async Task<List<UserType>> GetUserTypeList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "WebLogin/GetUserTypeList"));
            var response = await GetAsyncData<UserType>(requestUrl);
            List<UserType> userTypes = JsonConvert.DeserializeObject<List<UserType>>(Convert.ToString(response.value));
            return userTypes;
        }
    }
}
