using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;

namespace MyavanaAdmin.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminCookies")]
    public class GroupsController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        public GroupsController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
        }
        public IActionResult GroupList()
        {
            return View();
        }

        public IActionResult CreateGroup(string hairtype)
        {
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] IEnumerable<Group> groupUserListingModel)
        {
                if (groupUserListingModel.Count() != 0)
                {
                    string message = null;
                    bool isupdate = groupUserListingModel.Select(x => x.IsUpdate).FirstOrDefault();
                    if (isupdate)
                    {
                        var response = await MyavanaAdminApiClientFactory.Instance.UpdateGroup(groupUserListingModel);
                        message = response.message;
                    }
                    else
                    {
                        var response = await MyavanaAdminApiClientFactory.Instance.CreateGroup(groupUserListingModel);
                        message = response.message;
                    }
                    if (message == "Success")
                        return Content("1");
                    else
                        return Content("0");
                }
            return Content("-1");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGroup(GroupsModel grpModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteGroup(grpModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }

        public IActionResult GroupRequestList()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApproveRequest(RequestApproveModel grpModel)
        {
            var loginUserId = Convert.ToInt32(Request.Cookies["UserId"]);
            grpModel.UpdatedByUserId = loginUserId;
            var response = await MyavanaAdminApiClientFactory.Instance.ApproveRequest(grpModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }
    }
}