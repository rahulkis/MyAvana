using MyAvanaQuestionaireModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyAvanaQuestionaireApiClient
{
	public partial class ApiClient
	{
		public async Task<Message<SetPassword>> ResetPassword(SetPassword hairProfileModel)
		{
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Account/setpass"));
			var result = await PostAsync<SetPassword>(requestUrl, hairProfileModel);
			return result;
		}

	}
}
