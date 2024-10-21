using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAvana.CRM.Api.Contract
{
    public interface IAppConfiguration
    {
        string AwsAccessKey { get; set; }
        string AwsSecretAccessKey { get; set; }
        string AwsSessionToken { get; set; }
        string BucketName { get; set; }
        string Region { get; set; }
    }
}
