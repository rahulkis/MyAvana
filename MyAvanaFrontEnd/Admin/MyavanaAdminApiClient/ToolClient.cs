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
        public async Task<List<ToolsModel>> GetTools()
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Tools/GetTools"));
            var response = await GetAsyncData<ToolsModel>(requestUrl);
            List<ToolsModel> tools = JsonConvert.DeserializeObject<List<ToolsModel>>(Convert.ToString(response.data));
            return tools;
        }

        public async Task<Message<ToolsModel>> SaveTools(ToolsModel toolsEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Tools/SaveTools"));
            var result = await PostAsync<ToolsModel>(requestUrl, toolsEntity);
            return result;
        }

        public async Task<Message<ToolsModel>> DeleteTool(ToolsModel productsEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Tools/DeleteTool"));
            var result = await PostAsync<ToolsModel>(requestUrl, productsEntity);
            return result;
        }
        public async Task<Message<ToolsModel>> GetToolsById(ToolsModel toolsEntity)
        {
            var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "Tools/GetToolsById"));
            var result = await PostAsync<ToolsModel>(requestUrl, toolsEntity);
            return result;
        }
    }
}
