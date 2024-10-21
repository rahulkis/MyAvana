using MyAvanaQuestionaireApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyAvanaQuestionaire.Factory
{
	internal static class MyavanaCustomerApiClientFactory
	{
		private static Uri apiUri;

		private static Lazy<ApiClient> restClient = new Lazy<ApiClient>(
		  () => new ApiClient(apiUri),
		  LazyThreadSafetyMode.ExecutionAndPublication);

		static MyavanaCustomerApiClientFactory()
		{
			apiUri = new Uri(Utility.ApplicationSettings.WebApiUrl);
		}

		public static ApiClient Instance
		{
			get
			{
				return restClient.Value;
			}
		}
	}
}
