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
    [Authorize(AuthenticationSchemes = "AdminCookies")]
    public class PromoCodeController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        public PromoCodeController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
        }


        public IActionResult GenerateCode()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PromoCode(PromoCodeModel promoCodeModel)
        {
            promoCodeModel.ExpireDate = Convert.ToDateTime(promoCodeModel.InitialDate);
            var response = await MyavanaAdminApiClientFactory.Instance.SavePromoCode(promoCodeModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }

        public IActionResult ViewCodes()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetPromoCodes([DataTablesRequest] DataTablesRequest dataRequest)
        {            
            IEnumerable<CodeListModel> filteredCodes = await MyavanaAdminApiClientFactory.Instance.GetPromoCodes();
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<CodeListModel, string> orderingFunctionString = null;
                Func<CodeListModel, int> orderingFunctionInt = null;
                Func<CodeListModel, DateTime?> orderingFunctionDateTime = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.PromoCode);
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
                }
            }
            try
            {
                IEnumerable<CodeListModel> codes = filteredCodes.Select(e => new CodeListModel
                {
                    PromoCode = e.PromoCode,
                    CreatedDate = e.CreatedDate,
                    ExpireDate = e.ExpireDate,
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
        public async Task<IActionResult> DeletePromoCode(PromoCodeModel promoCodeModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeletePromoCode(promoCodeModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }
    }
}
