using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
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
    public class AlexaController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        private readonly IOptions<AppSettingsModel> config;
        public AlexaController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
        }

        public async Task<IActionResult> CreateAlexaFAQ(string id)
        {
            if (id != null)
            {
                AlexaFAQModel alexaFAQModel = new AlexaFAQModel();
                alexaFAQModel.Id = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetAlexaFAQById(alexaFAQModel);
                alexaFAQModel = response.Data;
                return View(alexaFAQModel);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlexaFAQ(AlexaFAQModel alexaFAQModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.AddAlexaFAQ(alexaFAQModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");
        }

        public IActionResult ViewAlexaFAQs()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAlexaFAQs([DataTablesRequest] DataTablesRequest dataRequest)
        {
            var start = Convert.ToInt32(HttpContext.Request.Query["start"]);
            var length = Convert.ToInt32(HttpContext.Request.Query["length"]);
            IEnumerable<AlexaFAQModel> filteredFAQs = await MyavanaAdminApiClientFactory.Instance.GetAlexaFAQs(start, length);
            int TotalRecords = 0;
            if (filteredFAQs.Count() > 0)
            {
                TotalRecords = filteredFAQs.FirstOrDefault().TotalRecords;
            }
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<AlexaFAQModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.Description);
                            filteredFAQs =
                                sortDirection == "asc"
                                    ? filteredFAQs.OrderBy(orderingFunctionString)
                                    : filteredFAQs.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 1:
                        {
                            orderingFunctionString = (c => c.DetailedResponse);
                            filteredFAQs =
                                sortDirection == "asc"
                                    ? filteredFAQs.OrderBy(orderingFunctionString)
                                    : filteredFAQs.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 2:
                        {
                            orderingFunctionString = (c => c.ShortResponse);
                            filteredFAQs =
                                sortDirection == "asc"
                                    ? filteredFAQs.OrderBy(orderingFunctionString)
                                    : filteredFAQs.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 3:
                        {
                            orderingFunctionString = (c => c.Keywords);
                            filteredFAQs =
                                sortDirection == "asc"
                                    ? filteredFAQs.OrderBy(orderingFunctionString)
                                    : filteredFAQs.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 4:
                        {
                            orderingFunctionString = (c => c.Category);
                            filteredFAQs =
                                sortDirection == "asc"
                                    ? filteredFAQs.OrderBy(orderingFunctionString)
                                    : filteredFAQs.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }

            try
            {
                IEnumerable<AlexaFAQModel> alexaFAQs = filteredFAQs.Select(e => new AlexaFAQModel
                {
                    Id = e.Id,
                    Description = e.Description,
                    ShortResponse = e.ShortResponse,
                    DetailedResponse = e.DetailedResponse,
                    IsDeleted = e.IsDeleted,
                    Category = e.Category,
                    Keywords = e.Keywords
                });
                return Json(alexaFAQs.ToDataTablesResponse(dataRequest, alexaFAQs.Count()));

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteAlexaFAQ(AlexaFAQModel alexaFAQModel)
        {
            var response = await MyavanaAdminApiClientFactory.Instance.DeleteAlexaFAQ(alexaFAQModel);

            if (response.message == "Success")
                return Content("1");
            else
                return Content("0");

        }
    }
}
