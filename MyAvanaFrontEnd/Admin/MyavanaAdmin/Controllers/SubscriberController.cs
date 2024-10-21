using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;
using DataTables.AspNetCore.Mvc.Binder;

namespace MyavanaAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookies")]
    public class SubscriberController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        public SubscriberController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
        }
        public IActionResult Subscriber()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSubscriberList([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<SubscriberModel> filterQuestionnaire = await MyavanaAdminApiClientFactory.Instance.GetSubscriberList();

            try
            {
                IEnumerable<SubscriberModel> lst = filterQuestionnaire.Select(e => new SubscriberModel
                {
                    UserId = e.UserId,
                    UserName = e.UserName,
                    UserEmail = e.UserEmail,
                    IsActive= e.IsActive,
                    ProviderName=e.ProviderName

                });
                return Json(lst.ToDataTablesResponse(dataRequest, lst.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> CancelSubscription(SubscriberModel subscriberModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.CancelSubscription(subscriberModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }


    }
}
