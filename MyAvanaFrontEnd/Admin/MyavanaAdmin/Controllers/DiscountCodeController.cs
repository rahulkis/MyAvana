using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;

namespace MyavanaAdmin.Controllers
{
    public class DiscountCodeController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        public DiscountCodeController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
        }

        public IActionResult ViewDiscountCode()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetDiscountCodes([DataTablesRequest] DataTablesRequest dataRequest)
        {
            IEnumerable<DiscountCodeListModel> filteredCodes = await
                MyavanaAdminApiClientFactory.Instance.GetDiscountCodes();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<DiscountCodeListModel, string> orderingFunctionString = null;
                Func<DiscountCodeListModel, int> orderingFunctionInt = null;
                Func<DiscountCodeListModel, DateTime?> orderingFunctionDateTime = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.DiscountCode);
                            filteredCodes =
                                sortDirection == "asc"
                                    ? filteredCodes.OrderBy(orderingFunctionString)
                                    : filteredCodes.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 1:
                        {
                            orderingFunctionDateTime = (c => c.CreatedDate);
                            filteredCodes =
                                sortDirection == "asc"
                                    ? filteredCodes.OrderBy(orderingFunctionDateTime)
                                    : filteredCodes.OrderByDescending(orderingFunctionDateTime);
                            break;
                        }
                    case 2:
                        {
                            orderingFunctionDateTime = (c => c.ExpireDate);
                            filteredCodes =
                                sortDirection == "asc"
                                    ? filteredCodes.OrderBy(orderingFunctionDateTime)
                                    : filteredCodes.OrderByDescending(orderingFunctionDateTime);
                            break;
                        }
                    case 3:
                        {
                            orderingFunctionInt = (c => c.DiscountPercent);
                            filteredCodes =
                                sortDirection == "asc"
                                    ? filteredCodes.OrderBy(orderingFunctionInt)
                                    : filteredCodes.OrderByDescending(orderingFunctionInt);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<DiscountCodeListModel> codes = filteredCodes.Select(e => new DiscountCodeListModel
                {
                    DiscountCodeId = e.DiscountCodeId,
                    DiscountCode = e.DiscountCode,
                    CreatedDate = e.CreatedDate,
                    ExpireDate = e.ExpireDate,
                    DiscountPercent = e.DiscountPercent,
                    IsActive = e.IsActive
                }).OrderByDescending(x => x.CreatedDate);
                return Json(codes.ToDataTablesResponse(dataRequest, codes.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscountCode(DiscountCodeListModel saveDiscountCode)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.SaveDiscountCode(saveDiscountCode);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        public async Task<IActionResult> CreateDiscountCode(string id)
        {
            if (id != null)
            {
                DiscountCodesModel discountCodeEntity = new DiscountCodesModel();
                discountCodeEntity.DiscountCodeId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetDiscountCodeById(discountCodeEntity);
                discountCodeEntity = response.Data;
                return View(discountCodeEntity);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDiscountCode(DiscountCodesModel discountCodeEntity)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteDiscountCode(discountCodeEntity);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }
    }
}
