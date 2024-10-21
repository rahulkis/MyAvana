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
		public async Task<List<ProductsEntity>> GetProducts()
		{
			var requestUrl = CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Products/GetProducts"));
			var response = await GetAsyncData<ProductsEntity>(requestUrl);
			List<ProductsEntity> blogPosts = JsonConvert.DeserializeObject<List<ProductsEntity>>(Convert.ToString(response.value));
			return blogPosts;
		}

	}
}
