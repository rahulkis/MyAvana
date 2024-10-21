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
		public async Task<List<HairProfileCustomersModel>> GetHairProfileCustomerList(int userId)
		{
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetHairProfileCustomerList"), "?userId=" + userId);
			var response = await GetAsyncData<HairProfileCustomersModel>(requestUrl);
			List<HairProfileCustomersModel> questionaire = JsonConvert.DeserializeObject<List<HairProfileCustomersModel>>(Convert.ToString(response.data));
			return questionaire;
		}

		public async Task<Message<HairProfileCustomerModel>> GetHairProfileCustomer(HairProfileCustomerModel hairProfileModel)
		{
            try
            {
				var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetHairProfileCustomer"));
				var result = await PostAsync<HairProfileCustomerModel>(requestUrl, hairProfileModel);
				return result;
			}
            catch (Exception ex)
            {

                throw ex;
            }
			
		}

		public async Task<List<HairProfileSelectModel>> GetHHCPList(string userId)
		{
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetHHCPList"), "?userId=" + userId);
			var response = await GetAsyncData<HairProfileSelectModel>(requestUrl);
			List<HairProfileSelectModel> list = JsonConvert.DeserializeObject<List<HairProfileSelectModel>>(Convert.ToString(response.value));
			return list;
		}

		public async Task<Message<CreateHHCPModel>> CreateHHCPHairKitUser(CreateHHCPModel model)
		{
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/CreateHHCPHairKitUser"));
			var result = await PostAsync<CreateHHCPModel>(requestUrl, model);
			return result;
		}
		public async Task<Message<HHCPParam>> CreateHHCPUsingScalpAnalysis(HHCPParam model)
		{
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/CreateHHCPUsingScalpAnalysis"));
			var result = await PostAsync<HHCPParam>(requestUrl, model);
			return result;
		}

		public async Task<Message<EnableDisableProfileModel>> EnableDisableProfileView(EnableDisableProfileModel enableDisableProfileModel)
		{
			try
			{
				var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/EnableDisableProfileView"));
				var result = await PostAsync<EnableDisableProfileModel>(requestUrl, enableDisableProfileModel);
				return result;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public async Task<Message<HairProfileCustomerModel>> GetHairProfileCustomerTab2(HairProfileCustomerModel hairProfileModel)
		{
			try
			{
				var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetHairProfileCustomerTab2"));
				var result = await PostAsync<HairProfileCustomerModel>(requestUrl, hairProfileModel);
				return result;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public async Task<Message<HairProfileCustomerModel>> GetHairProfileCustomerExceptTab2(HairProfileCustomerModel hairProfileModel)
		{
			try
			{
				var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetHairProfileCustomerExceptTab2"));
				var result = await PostAsync<HairProfileCustomerModel>(requestUrl, hairProfileModel);
				return result;
			}
			catch (Exception ex)
			{

				throw ex;
			}

		}
		
		public async Task<List<HairStrandUploadNotificationModel>> GetHairStrandUploadNotificationList()
		{
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetHairStrandUploadNotificationList"));
			try
			{
				var response = await GetAsyncData<BrandModelList>(requestUrl);
				List<HairStrandUploadNotificationModel> notificationlist = JsonConvert.DeserializeObject<List<HairStrandUploadNotificationModel>>(Convert.ToString(response.value));
				return notificationlist;
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		public async Task<Message<HairStrandUploadNotificationModel>> UpdateNotificationAsRead(HairStrandUploadNotificationModel notification)
		{
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/UpdateNotificationAsRead"));
			var result = await PostAsync<HairStrandUploadNotificationModel>(requestUrl, notification);
			return result;
		}
		public async Task<List<DailyRoutineTrackerNotificationModel>> GetHairDiarySubmitNotificationList()
		{
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/GetHairDiarySubmitNotificationList"));
			var response = await GetAsyncData<DailyRoutineTrackerNotificationModel>(requestUrl);
			List<DailyRoutineTrackerNotificationModel> hairdiarynotificationlist = JsonConvert.DeserializeObject<List<DailyRoutineTrackerNotificationModel>>(Convert.ToString(response.value));
			return hairdiarynotificationlist;
		}
		public async Task<Message<DailyRoutineTrackerNotificationModel>> UpdateNotificationHairDiaryAsRead(DailyRoutineTrackerNotificationModel notification)
		{
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairProfile/UpdateNotificationHairDiaryAsRead"));
			var result = await PostAsync<DailyRoutineTrackerNotificationModel>(requestUrl, notification);
			return result;
		}
		public async Task<Message<HairScopeModel>> GetHairScopeResultData(HairScopeModel HairScopeModelParam)
		{
			try
			{			
				var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "HairScope/GetHairScopeResultDataWeb"));
				var result = await PostAsync<HairScopeModel>(requestUrl, HairScopeModelParam);
				return result ;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
	}
}
