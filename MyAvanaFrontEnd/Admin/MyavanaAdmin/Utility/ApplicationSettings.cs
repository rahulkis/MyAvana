using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyavanaAdmin.Utility
{
	public class ApplicationSettings
	{
		public static string WebApiUrl { get; set; }
		public static string SiteUrl { get; set; }
	}

    public class PowerBiConfig
    {
        public static string applicationId { get; set; }
        public static string workspaceId { get; set; }
        public static string reportId { get; set; }
        public static string authenticationType { get; set; }
        public static string applicationSecret { get; set; }
        public static string tenant { get; set; }
        public static string username { get; set; }
        public static string password { get; set; }
    }
}
