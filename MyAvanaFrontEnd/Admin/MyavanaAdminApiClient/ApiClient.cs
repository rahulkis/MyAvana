using MyavanaAdminModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace MyavanaAdminApiClient
{
	public partial class ApiClient
	{

		private readonly HttpClient _httpClient;
		private Uri BaseEndpoint { get; set; }

		public ApiClient(Uri baseEndpoint)
		{
			if (baseEndpoint == null)
			{
				throw new ArgumentNullException("baseEndpoint");
			}
			BaseEndpoint = new Uri("http://localhost:5004/api/");
			_httpClient = new HttpClient();
		}

		private async Task<Message<T>> PostAsync<T>(Uri requestUrl, T content)
		{
			addHeaders();
			var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T>(content));
			response.EnsureSuccessStatusCode();
			var data = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<Message<T>>(data);
		}

		private async Task<List<T>> GetAsyncList<T>(Uri requestUrl)
		{
			addHeaders();
			var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
			response.EnsureSuccessStatusCode();
			var data = await response.Content.ReadAsStringAsync();
			dynamic result = JObject.Parse(data);
			return JsonConvert.DeserializeObject<List<T>>(Convert.ToString(result.data));
		}
		private async Task<dynamic> GetAsyncData<T>(Uri requestUrl)
		{
			addHeaders();
			var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
			response.EnsureSuccessStatusCode();
			var data = await response.Content.ReadAsStringAsync();
			dynamic result = JObject.Parse(data);
			return result;
		}
		private async Task<List<T>> GetAsyncResponse<T>(Uri requestUrl)
		{
			addHeaders();
			var response = await _httpClient.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead);
			response.EnsureSuccessStatusCode();
			var data = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<List<T>>(data);
		}

		private Uri CreateRequestUri(string relativePath, string queryString = "")
		{
			var endpoint = new Uri(BaseEndpoint, relativePath);
			var uriBuilder = new UriBuilder(endpoint);
			uriBuilder.Query = queryString;
			return uriBuilder.Uri;
		}

		private HttpContent CreateHttpContent<T>(T content)
		{
			var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
			return new StringContent(json, Encoding.UTF8, "application/json");
		}

		private static JsonSerializerSettings MicrosoftDateFormatSettings
		{
			get
			{
				return new JsonSerializerSettings
				{
					DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
				};
			}
		}

		private void addHeaders()
		{
			_httpClient.DefaultRequestHeaders.Remove("userIP");
			_httpClient.DefaultRequestHeaders.Add("userIP", "192.168.1.1");
		}
	}
}
