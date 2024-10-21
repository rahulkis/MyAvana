using Microsoft.PowerBI.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyavanaAdminModels
{
    public class ReportEmbedConfig
    {
        // Report to be embedded
        public List<EmbedReport> EmbedReports { get; set; }

        // Embed Token for the Power BI report
        public EmbedToken EmbedToken { get; set; }
    }

    public class EmbedReport
    {
        // Id of Power BI report to be embedded
        public Guid ReportId { get; set; }

        // Name of the report
        public string ReportName { get; set; }

        // Embed URL for the Power BI report
        public string EmbedUrl { get; set; }
    }
    public class DashboardEmbedConfig
    {
        public Guid DashboardId { get; set; }

        public string EmbedUrl { get; set; }

        public EmbedToken EmbedToken { get; set; }
    }
    public class TileEmbedConfig
    {
        public Guid TileId { get; set; }

        public string EmbedUrl { get; set; }

        public EmbedToken EmbedToken { get; set; }

        public Guid DashboardId { get; set; }
    }

    public class ErrorModel
    {
        public string ErrorMessage { get; set; }
    }

    public class PowerBiConfigModel
    {
        public string applicationId { get; set; }
        public string workspaceId { get; set; }
        public string reportId { get; set; }
        public string authenticationType { get; set; }
        public string applicationSecret { get; set; }
        public string tenant { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
