using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyavanaAdmin.Factory;
using MyavanaAdmin.Models;
using MyavanaAdmin.Utility;
using MyavanaAdminModels;

namespace MyavanaAdmin.Controllers
{
    public class SalonController : Controller
    {
        private readonly AppSettingsModel apiSettings;
        private readonly HttpClient _httpClient;
        public SalonController(IOptions<AppSettingsModel> app)
        {
            ApplicationSettings.WebApiUrl = app.Value.WebApiBaseUrl;
          
            _httpClient = new HttpClient();
        }
        public IActionResult Salons()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSalonsList([DataTablesRequest] DataTablesRequest dataRequest)
        {

            var start =Convert.ToInt32(HttpContext.Request.Query["start"]);
            var length = Convert.ToInt32(HttpContext.Request.Query["length"]);
            IEnumerable<SalonModel> filterSalons = await MyavanaAdminApiClientFactory.Instance.GetSalons(start, length);
            int TotalRecords = 0;

            if(filterSalons.Count()>0)
            {
                TotalRecords = filterSalons.FirstOrDefault().TotalRecords;
            }
            if (dataRequest.Orders.Any())
            {
                int sortColumnIndex = dataRequest.Orders.FirstOrDefault().Column;
                string sortDirection = dataRequest.Orders.FirstOrDefault().Dir;
                Func<SalonModel, string> orderingFunctionString = null;
                switch (sortColumnIndex)
                {
                    case 0:
                        {
                            orderingFunctionString = (c => c.SalonName);
                            filterSalons =
                                sortDirection == "asc"
                                    ? filterSalons.OrderBy(orderingFunctionString)
                                    : filterSalons.OrderByDescending(orderingFunctionString);
                            break;
                        }
                    case 1:
                        {
                            orderingFunctionString = (c => c.EmailAddress);
                            filterSalons =
                                sortDirection == "asc"
                                    ? filterSalons.OrderBy(orderingFunctionString)
                                    : filterSalons.OrderByDescending(orderingFunctionString);
                            break;
                        }
                }
            }
            try
            {
                IEnumerable<SalonModel> codes = filterSalons.Select(e => new SalonModel
                {
                    SalonId = e.SalonId,
                    EmailAddress = e.EmailAddress,
                    SalonName = e.SalonName,
                    Address = e.Address,
                    PhoneNumber = e.PhoneNumber,
                    IsActive = e.IsActive,
                    PublicNotes = e.IsPublicNotes==true?"True":"False",
                    SalonLogo=e.SalonLogo

                }).OrderByDescending(x => x.SalonName);
                return Json(codes.ToDataTablesResponse(dataRequest, TotalRecords));

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IActionResult> CreateNewSalon(string id)
        {
            if (id != null)
            {
                SalonModel salonDetails = new SalonModel();
                salonDetails.SalonId = Convert.ToInt32(id);
                var response = await MyavanaAdminApiClientFactory.Instance.GetSalonById(salonDetails);
                salonDetails = response.Data;
                return View(salonDetails);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewSalon(SalonModel model, IFormFile File)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApplicationSettings.WebApiUrl);
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                PropertyInfo[] messageProperties = typeof(SalonModel).GetProperties();
                foreach (PropertyInfo prop in messageProperties)
                {
                    if (prop.Name != "SalonLogo" && prop.Name != "File")
                    {
                        if (prop.GetValue(model) != null)
                        {
                            multiContent.Add(new StringContent(prop.GetValue(model).ToString()), prop.Name);
                        }
                        else
                        {
                            multiContent.Add(new StringContent(string.Empty), prop.Name);
                        }

                    }
                }
                if (File != null)
                {

                    multiContent.Add(new StreamContent(File.OpenReadStream())
                    {
                        Headers =
                             {
                               ContentLength = File.Length,
                               ContentType = new MediaTypeHeaderValue(File.ContentType)
                             }
                    }, "File", File.FileName);
                }
                
                var response = client.PostAsync("Salons/AddNewSalon", multiContent).Result;
                //var response = await MyavanaAdminApiClientFactory.Instance.CreateNewSalon(model);

                if ((int)response.StatusCode == 200)
                    return Content("1");
                else
                    return Content("0");
            }
        }
    }
}