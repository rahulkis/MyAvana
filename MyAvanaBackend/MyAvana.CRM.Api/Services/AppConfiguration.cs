using MyAvana.CRM.Api.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Services
{
    public class AppConfiguration : IAppConfiguration
    {
		public AppConfiguration()
		{
			BucketName = "";
			Region = "";
			AwsAccessKey = "";
			AwsSecretAccessKey = "";
			AwsSessionToken = "";
		}

		public string BucketName { get; set; }
		public string Region { get; set; }
		public string AwsAccessKey { get; set; }
		public string AwsSecretAccessKey { get; set; }
		public string AwsSessionToken { get; set; }
	}
}
