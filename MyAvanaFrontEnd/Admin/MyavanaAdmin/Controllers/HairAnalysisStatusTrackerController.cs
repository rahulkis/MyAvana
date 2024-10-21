using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using MyavanaAdmin.Factory;
using MyavanaAdminModels;
using System.Net.Http;
using MyavanaAdmin.Utility;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace MyavanaAdmin.Controllers
{
    public class HairAnalysisStatusTrackerController : Controller
    {
        public IActionResult HairAnalysisStatusTracker()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveHairAnalysisStatus(StatusTrackerModel trackerModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveHairAnalysisStatus(trackerModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
        [HttpGet]
        public async Task<IActionResult> GetStatusTrackerList([DataTablesRequest] DataTablesRequest dataRequest)
        {


            IEnumerable<StatusTrackerModel> trackers = await MyavanaAdminApiClientFactory.Instance.GetStatusTrackerList();
            
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                //Func<StatusTrackerModel, string> orderingFunctionString = null;
                //switch (sortColumnIndex)
                //{
                //    case 0:
                //        {
                //            orderingFunctionString = (c => c.CustomerName);
                //            trackers =
                //                sortDirection == "asc"
                //                    ? trackers.OrderBy(orderingFunctionString)
                //                    : trackers.OrderByDescending(orderingFunctionString);
                //            break;
                //        }
                //    case 1:
                //        {
                //            orderingFunctionString = (c => c.HairAnalysisStatus);
                //            trackers =
                //                sortDirection == "asc"
                //                    ? trackers.OrderBy(orderingFunctionString)
                //                    : trackers.OrderByDescending(orderingFunctionString);
                //            break;
                //        }
                //}
            }
            try
            {
                IEnumerable<StatusTrackerModel> codes = trackers.Select(e => new StatusTrackerModel
                {
                    StatusTrackerId=e.StatusTrackerId,
                    CustomerName=e.CustomerName,
                    HairAnalysisStatus=e.HairAnalysisStatus,
                    CustomerId = e.CustomerId,
                    CustomerEmail = e.CustomerEmail,
                    KitSerialNumber = e.KitSerialNumber
                }).OrderByDescending(x => x.StatusTrackerId);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeHairAnalysisStatus(StatusTrackerModel trackerModel)
        {
            trackerModel.LastModifiedBy = Request.Cookies["UserId"];
            trackerModel.LastUpdatedOn = DateTime.Now;
            
            var response = await MyavanaAdminApiClientFactory.Instance.ChangeHairAnalysisStatus(trackerModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }
        [HttpPost]
        public async Task<IActionResult> AddToStatusTracker(StatusTrackerModel trackerModel)
        {
            trackerModel.LastModifiedBy = Request.Cookies["UserId"];
            trackerModel.LastUpdatedOn = DateTime.Now;
            
            var response = await MyavanaAdminApiClientFactory.Instance.AddToStatusTracker(trackerModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerHairAnalysisStatusHistory([DataTablesRequest] DataTablesRequest dataRequest, string Id)
        {

            using (var client = new HttpClient())
            {
                try
                {
                    var requestUrl = client.GetAsync(ApplicationSettings.WebApiUrl + "HairAnalysisStatus/GetHairAnalysisStatusHistoryList?id=" + Id).Result;
                    var data = await requestUrl.Content.ReadAsStringAsync();
                    dynamic result = JObject.Parse(data);
                    IEnumerable<HairAnalysisStatusHistoryModel> customerHistory = JsonConvert.DeserializeObject<List<HairAnalysisStatusHistoryModel>>(Convert.ToString(result.data));
                    IEnumerable<HairAnalysisStatusHistoryModel> records = customerHistory.Select(e => new HairAnalysisStatusHistoryModel
                    {
                        OldStatusName = e.OldStatusName,
                        StatusName = e.StatusName,
                        CreatedDate = e.CreatedOn.ToString("yyyy-MM-dd"),
                        UpdatedByUser = e.UpdatedByUser
                    }).OrderByDescending(x => x.CreatedOn);
                    return Json(records.ToDataTablesResponse(dataRequest, records.Count()));
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

    }
}
