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
        public async Task<List<HairTypeUserEntity>> GetHairTypeUsers()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "groups/gethairtypeusers"));
            var response = await GetAsyncData<HairTypeUserEntity>(requestUrl);
            List<HairTypeUserEntity> users = JsonConvert.DeserializeObject<List<HairTypeUserEntity>>(Convert.ToString(response.value));
            return users;
        }

        public async Task<Message<IEnumerable<Group>>> CreateGroup(IEnumerable<Group> group)
        {
                var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "groups/creategroup"));
                var result = await PostAsync<IEnumerable<Group>>(requestUrl, group);
                return result;
        }

        public async Task<Message<IEnumerable<Group>>> UpdateGroup(IEnumerable<Group> group)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "groups/updategroup"));
            var result = await PostAsync<IEnumerable<Group>>(requestUrl, group);
            return result;
        }

        public async Task<List<GroupsModel>> GetGroupList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "groups/GetGroupList"));
            var response = await GetAsyncData<GroupsModel>(requestUrl);
            List<GroupsModel> questionaire = JsonConvert.DeserializeObject<List<GroupsModel>>(Convert.ToString(response.data));
            return questionaire;
        }

        public async Task<Message<GroupsModel>> DeleteGroup(GroupsModel questModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "groups/DeleteGroup"));
            var result = await PostAsync<GroupsModel>(requestUrl, questModel);
            return result;
        }

        public async Task<List<GroupRequestModel>> GetGroupRequestsList()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "groups/GetGroupRequestList"));
            var response = await GetAsyncData<GroupRequestModel>(requestUrl);
            List<GroupRequestModel> questionaire = JsonConvert.DeserializeObject<List<GroupRequestModel>>(Convert.ToString(response.data));
            return questionaire;
        }
        public async Task<Message<RequestApproveModel>> ApproveRequest(RequestApproveModel questModel)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "groups/ApproveRequest"));
            var result = await PostAsync<RequestApproveModel>(requestUrl, questModel);
            return result;
        }
    }
}
