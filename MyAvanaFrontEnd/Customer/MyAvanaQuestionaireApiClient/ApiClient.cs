using MyAvanaQuestionaireModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace MyAvanaQuestionaireApiClient
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
			BaseEndpoint = baseEndpoint;
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
			//_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJtYW5pc2hhQG1haWxpbmF0b3IuY29tIiwianRpIjoiM2FiZDA5Y2YtNzJmZC00OWFmLThiZWItMGRlYmYzMDQwOTlmIiwiaWF0IjoxNjcyMzk4OTQxLCJuYmYiOjE2NzIzOTg5NDEsImV4cCI6MTY3MzYwODU0MSwiaXNzIjoiaHR0cDovL3d3d3cubXlhdmFuYS5jb20iLCJhdWQiOiJNeUF2YW5hIn0.ExBPQFR3_P1YO6QcYoLgM1D5ZtYNcpUtWFzvzTtp5Kg");
		}
	}
}
