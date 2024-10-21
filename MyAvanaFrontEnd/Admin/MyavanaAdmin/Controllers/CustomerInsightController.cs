using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.PowerBI.Api;
using Microsoft.Rest;
using MyavanaAdmin.Models;
using MyavanaAdmin.Services;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MyavanaAdmin.Controllers
{
    public class CustomerInsightController : Controller
    {
        private string m_errorMessage;
        public CustomerInsightController(IOptions<PowerBiConfigModel> config)
        {
            PowerBiConfig.applicationId = config.Value.applicationId;
            PowerBiConfig.applicationSecret = config.Value.applicationSecret;
            PowerBiConfig.authenticationType = config.Value.authenticationType;
            PowerBiConfig.password = config.Value.password;
            PowerBiConfig.reportId = config.Value.reportId;
            PowerBiConfig.tenant = config.Value.tenant;
            PowerBiConfig.username = config.Value.username;
            PowerBiConfig.workspaceId = config.Value.workspaceId;
            m_errorMessage = ConfigValidatorService.GetWebConfigErrors();
        }
        public async Task<ActionResult> EmbedReport()
        {
            if (!String.IsNullOrEmpty(m_errorMessage))
            {
                return View("Error", BuildErrorModel(m_errorMessage));
            }

            try
            {
                var embedResult = await EmbedService.GetEmbedParams(ConfigValidatorService.WorkspaceId, ConfigValidatorService.ReportId);
                return View(embedResult);
            }
            catch (HttpOperationException exc)
            {
                m_errorMessage = string.Format("Status: {0} ({1})\r\nResponse: {2}\r\nRequestId: {3}", exc.Response.StatusCode, (int)exc.Response.StatusCode, exc.Response.Content, exc.Response.Headers["RequestId"].FirstOrDefault());
                return View("Error", BuildErrorModel(m_errorMessage));
            }
            catch (Exception ex)
            {
                return View("Error", BuildErrorModel(ex.Message));
            }
        }

        private ErrorModel BuildErrorModel(string errorMessage)
        {
            return new ErrorModel
            {
                ErrorMessage = errorMessage
            };
        }
    }
}
